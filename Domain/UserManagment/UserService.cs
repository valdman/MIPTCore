using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Journalist;
using UserManagment.Application;
using UserManagment.Exceptions;

namespace UserManagment
{
    public class UserService : IUserService
    {
        private readonly IUserManager _userManager;
        private readonly ITicketService _ticketService;

        public UserService(IUserManager userManager, ITicketService ticketService)
        {
            _userManager = userManager;
            _ticketService = ticketService;
        }

        public async Task ConfirmEmail(string emailToConfirm)
        {
            Require.NotEmpty(emailToConfirm, nameof(emailToConfirm));

            var userToConfirmEmail = await _userManager.GetUserByEmailAsync(emailToConfirm);

            if (userToConfirmEmail == null)
                throw new OperationOnUserThatNotExistsException("confirm email");
            
            userToConfirmEmail.IsEmailConfirmed = true;
            await _userManager.UpdateUserAsync(userToConfirmEmail);
            
            await _ticketService.CompleteAllTicketsByEmailAndType(emailToConfirm, TicketType.EmailConfirmation);
        }

        public async Task ChangePassword(int userIdToChangePassword, Password newPassword)
        {
            Require.Positive(userIdToChangePassword, nameof(userIdToChangePassword));
            Require.NotNull(newPassword, nameof(newPassword));

            var userToChangePassword = await _userManager.GetUserByIdAsync(userIdToChangePassword);

            if (userToChangePassword == null)
                throw new OperationOnUserThatNotExistsException("change password");

            userToChangePassword.Password = newPassword;
            await _userManager.UpdateUserAsync(userToChangePassword);

            await _ticketService.CompleteAllTicketsByEmailAndType(userToChangePassword.Email, TicketType.PasswordRecovery);
        }
    }
}