using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common.Infrastructure;
using Journalist;
using UserManagment.Application;
using UserManagment.Exceptions;

namespace UserManagment
{
    public class UserManager : IUserManager
    {
        private readonly IGenericRepository<User> _userRepository;

        public UserManager(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            Require.Positive(userId, nameof(userId));

            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            Require.NotEmpty(email, nameof(email));

            var foundedUsers = await _userRepository.FindByAsync(user => user.Email.Equals(email));
            var users = foundedUsers as User[] ?? foundedUsers.ToArray();
            
            var numberOfFoundedUsers = users.Count();

            if (numberOfFoundedUsers > 1)
            {
                throw new UserVitalDomainException("Users with same emails founded");
            }

            return users.SingleOrDefault();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAll();
        }

        public async Task<IEnumerable<User>> GetUsersByPredicateAsync(Expression<Func<User, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));

            return await _userRepository.FindByAsync(predicate);
        }

        public async Task UpdateUserAsync(User userToUpdate)
        {
            Require.NotNull(userToUpdate, nameof(userToUpdate));

            await MustHaveUniqueEmail(userToUpdate);
            MustContainConsistentProfile(userToUpdate);

            await _userRepository.UpdateAsync(userToUpdate);
        }

        public async Task<int> CreateUserAsync(User userToCreate)
        {
            Require.NotNull(userToCreate, nameof(userToCreate));

            await MustHaveUniqueEmail(userToCreate);
            MustContainConsistentProfile(userToCreate);

            return await _userRepository.CreateAsync(userToCreate);
        }

        public async Task DeleteUserAsync(int userToDeleteId)
        {
            Require.Positive(userToDeleteId, nameof(userToDeleteId));

            await _userRepository.DeleteAsync(userToDeleteId);
        }

        private async Task MustHaveUniqueEmail(User userToCheck)
        {
            var sameEmailUser = await GetUserByEmailAsync(userToCheck.Email);

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