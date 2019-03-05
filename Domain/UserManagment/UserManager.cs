using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Common.Entities;
using Common.Infrastructure;
using Journalist;
using UserManagment.Application;
using UserManagment.Exceptions;

namespace UserManagment
{
    public class UserManager : IUserManager
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IUserMailerService _userMailerService;
        
        private readonly object _userCreationLock = new object();
        private readonly object _userUpdateLock = new object();
        private readonly object _userSaveLock = new object();

        public UserManager(IGenericRepository<User> userRepository, IUserMailerService userMailerService)
        {
            _userRepository = userRepository;
            _userMailerService = userMailerService;
        }

        public User GetUserById(int userId)
        {
            Require.Positive(userId, nameof(userId));

            return _userRepository.GetById(userId);
        }

        public User GetUserByEmail(string email)
        {
            Require.NotEmpty(email, nameof(email));

            var foundedUsers = _userRepository.FindBy(user => user.Email.Equals(email));
            var users = foundedUsers as User[] ?? foundedUsers.ToArray();
            
            var numberOfFoundedUsers = users.Count();

            if (numberOfFoundedUsers > 1)
            {
                throw new UserVitalDomainException("Users with same emails founded");
            }

            return users.SingleOrDefault();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        public IEnumerable<User> GetUsersByPredicate(Expression<Func<User, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));

            return _userRepository.FindBy(predicate);
        }

        public void UpdateUser(User userToUpdate)
        {
            Require.NotNull(userToUpdate, nameof(userToUpdate));

            lock (_userUpdateLock)
            {
                MustHaveUniqueEmail(userToUpdate);
                MustContainConsistentProfile(userToUpdate);

                _userRepository.Update(userToUpdate);
            }
        }

        public int CreateUser(User userToCreate)
        {
            Require.NotNull(userToCreate, nameof(userToCreate));

            int userId;
            lock (_userCreationLock)
            {
                MustHaveUniqueEmail(userToCreate);
                MustContainConsistentProfile(userToCreate);

                userId = _userRepository.Create(userToCreate);
                _userMailerService.BeginPasswordSettingAndEmailVerification(userId).Wait();
            }
            
            return userId;
        }

        public User GetOrSaveUserWithoutCredentials(User userToSave)
        {
            Require.NotNull(userToSave, nameof(userToSave));

            lock (_userSaveLock)
            {
                var existingUser = GetUserByEmail(userToSave.Email);

                if (existingUser != null)
                {
                    return existingUser;
                }
            
                userToSave.Password = new Password
                (
                    //crutches.js
                    Guid.NewGuid().ToString("n").Substring(0, 10)
                );
                userToSave.Role = UserRole.User;
            
                var newCreatedUserId = CreateUser(userToSave);

                return _userRepository.GetById(newCreatedUserId);
            }
        }

        public void DeleteUser(int userToDeleteId)
        {
            Require.Positive(userToDeleteId, nameof(userToDeleteId));

            _userRepository.Delete(userToDeleteId);
        }

        private void MustHaveUniqueEmail(User userToCheck)
        {
            var sameEmailUser = GetUserByEmail(userToCheck.Email);

            if (sameEmailUser != null && sameEmailUser.Id != userToCheck.Id)
            {
                throw new DuplicateEmailException(userToCheck.Email);
            }
        }

        private void MustContainConsistentProfile(User userToCheck)
        {
            if (userToCheck.IsMiptAlumni && userToCheck.AlumniProfile == null)
            {
                throw new ProfileNotProvidedException();
            }

            if (!userToCheck.IsMiptAlumni && userToCheck.AlumniProfile != null)
            {
                throw new ProfileShouldNotBeProvidedException();
            }
        }
    }
}