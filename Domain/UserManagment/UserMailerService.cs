using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Infrastructure;
using Journalist;
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

            var confirmationTicket = new Ticket(TicketType.EmailConfirmation)
            {
                EmailToSend = userToConfirm.Email,
                Token = GenerateToken(),
                IsCompleted = false
            };

            await _ticketRepository.CreateAsync(confirmationTicket);
            await _ticketSender.SendTicketAsync(confirmationTicket.EmailToSend,
                confirmationTicket);
        }

        public async Task BeginPasswordRecovery(int userId)
        {
            Require.Positive(userId, nameof(userId));

            var userToChangePassword = await _userManager.GetUserByIdAsync(userId);

            if (userToChangePassword == null)
                throw new OperationOnUserThatNotExistsException("start password changing");

            var passwordRecoveyTicket = new Ticket(TicketType.PasswordRecovery)
            {
                EmailToSend = userToChangePassword.Email,
                Token = GenerateToken(),
                IsCompleted = false
            };

            await _ticketRepository.CreateAsync(passwordRecoveyTicket);
            await _ticketSender.SendTicketAsync(userToChangePassword.Email, passwordRecoveyTicket);
        }

        public async Task ConfirmEmail(string emailToConfirm)
        {
            Require.NotEmpty(emailToConfirm, nameof(emailToConfirm));

            var userToConfirmEmail = await _userManager.GetUserByEmailAsync(emailToConfirm);

            if (userToConfirmEmail == null)
                throw new OperationOnUserThatNotExistsException("confirm email");

            var allTickets = (await _ticketRepository.FindByAsync(ticket
                => ticket.TicketType == TicketType.EmailConfirmation
                   && !ticket.IsCompleted
                   && ticket.EmailToSend == emailToConfirm
                   && emailToConfirm == userToConfirmEmail.Email)).ToList();

            foreach (var ticket in allTickets)
            {
                ticket.IsCompleted = true;
            }

            await _ticketRepository.UpdateManyTicketsAsync(allTickets);
            userToConfirmEmail.IsEmailConfirmed = true;
            await _userManager.UpdateUserAsync(userToConfirmEmail);
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
        }

        public async Task<string> GetUserEmailByPasswordRecoveyToken(string token)
        {
            Require.NotEmpty(token, nameof(token));

            return (await _ticketRepository.FindByAsync(ticket =>
                    ticket.TicketType == TicketType.PasswordRecovery
                    && !ticket.IsCompleted
                    && ticket.Token == token)).SingleOrDefault()
                ?.EmailToSend;
        }

        public async Task<string> GetUserEmailByEmailConfirmationToken(string token)
        {
            Require.NotEmpty(token, nameof(token));

            return (await _ticketRepository.FindByAsync(ticket => ticket.TicketType == TicketType.EmailConfirmation
                                                                  && ticket.Token == token)).SingleOrDefault()
                ?.EmailToSend;
        }

        private string GenerateToken()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}