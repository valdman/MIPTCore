using Common.Entities;
using Journalist;
using UserManagment.Application;
using UserManagment.Exceptions;

namespace UserManagment
{
    public class UserService : IUserService
    {
        private readonly IUserManager _userManager;
        private readonly ITicketService _ticketService;
        private readonly IMailer _userMailerService;

        public UserService(IUserManager userManager, ITicketService ticketService, IMailer userMailerService)
        {
            _userManager = userManager;
            _ticketService = ticketService;
            _userMailerService = userMailerService;
        }

        public void ConfirmEmail(string emailToConfirm)
        {
            Require.NotEmpty(emailToConfirm, nameof(emailToConfirm));

            var userToConfirmEmail = _userManager.GetUserByEmail(emailToConfirm);

            if (userToConfirmEmail == null)
                throw new OperationOnUserThatNotExistsException("confirm email");
            
            userToConfirmEmail.IsEmailConfirmed = true;
            _userManager.UpdateUser(userToConfirmEmail);
            
            _ticketService.CompleteAllTicketsByEmailAndType(emailToConfirm, TicketType.EmailConfirmation);
        }

        public void ChangePassword(int userIdToChangePassword, Password newPassword)
        {
            Require.Positive(userIdToChangePassword, nameof(userIdToChangePassword));
            Require.NotNull(newPassword, nameof(newPassword));

            var userToChangePassword = _userManager.GetUserById(userIdToChangePassword);

            if (userToChangePassword == null)
                throw new OperationOnUserThatNotExistsException("change password");

            userToChangePassword.Password = newPassword;
            _userManager.UpdateUser(userToChangePassword);

            _ticketService.CompleteAllTicketsByEmailAndType(userToChangePassword.Email, TicketType.PasswordRecovery);
        }

        public void RequestEmailBill(int userId)
        {
            Require.Positive(userId, nameof(userId));

            var user = _userManager.GetUserById(userId);
            
            _userMailerService.RequestBill(user);
        }
    }
}