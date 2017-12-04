using System;
using System.Threading.Tasks;
using Common;
using UserManagment.Application;
using UserManagment.Exceptions;

namespace UserManagment
{
    public class AuthentificationService : IAuthentificationService
    {
        private readonly IUserManager _userManager;

        public AuthentificationService(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> AuthentificateAsync(Credentials credentials)
        {
            var userToAuthentificate = _userManager.GetUserByEmail(credentials.Email);
            if(userToAuthentificate == null)
            {
                throw new OperationOnUserThatNotExistsException("login");
            }

            var intendedHash = new Password(credentials.Password).Hash;

            if(userToAuthentificate.Password.Hash != intendedHash)
            {
                throw new WrongPasswordException();
            }
            
            userToAuthentificate.AuthentificatedAt = DateTimeOffset.Now;

            _userManager.UpdateUser(userToAuthentificate);

            return userToAuthentificate;
        }

        public async Task DeauthentificateAsync(int userId)
        {
            var userToDeauthentificate = _userManager.GetUserById(userId);
            
            userToDeauthentificate.AuthentificatedAt = null;

            _userManager.UpdateUser(userToDeauthentificate);
            
        }
    }
}