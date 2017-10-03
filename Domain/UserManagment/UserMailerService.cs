using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Infrastructure;
using Journalist;
using UserManagment.Application;
using UserManagment.Exceptions;
using UserManagment.Infrastructure;

namespace UserManagment
{
    public class UserMailerService : IUserMailerService
    {
        private readonly IUserManager _userManager;
        private readonly ITicketRepository _ticketRepository;
        private readonly ITicketSender _ticketSender;

        public UserMailerService(IUserManager userManager, ITicketSender ticketSender,
            ITicketRepository ticketRepository)
        {
            _userManager = userManager;
            _ticketSender = ticketSender;
            _ticketRepository = ticketRepository;
        }

        public async Task BeginEmailConfirmation(int userId)
        {
            Require.Positive(userId, nameof(userId));

            var userToConfirm = await _userManager.GetUserByIdAsync(userId);

            if (userToConfirm == null)
                throw new OperationOnUserThatNotExistsException("start email confirmation");

            if (userToConfirm.IsEmailConfirmed)
                throw new EmailAlreadyConfirmedException();

            var confirmationTicket = new Ticket
            {
                TicketType = TicketType.EmailConfirmation,
                EmailToSend = userToConfirm.Email,
                Token = GenerateToken(),
                IsCompleted = false
            };

            await _ticketRepository.CreateAsync(confirmationTicket);
            await _ticketSender.SendTicketAsync(confirmationTicket.EmailToSend, confirmationTicket);
        }

        public async Task BeginPasswordRecovery(int userId)
        {
            Require.Positive(userId, nameof(userId));

            var userToChangePassword = await _userManager.GetUserByIdAsync(userId);

            if (userToChangePassword == null)
                throw new OperationOnUserThatNotExistsException("start password changing");

            var passwordRecoveyTicket = new Ticket
            {
                TicketType = TicketType.PasswordRecovery,
                EmailToSend = userToChangePassword.Email,
                Token = GenerateToken(),
                IsCompleted = false
            };

            await _ticketRepository.CreateAsync(passwordRecoveyTicket);
            await _ticketSender.SendTicketAsync(userToChangePassword.Email, passwordRecoveyTicket);
        }

        private string GenerateToken()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}