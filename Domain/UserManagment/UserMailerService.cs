using System;
using System.Threading.Tasks;
using Common.Infrastructure;
using Journalist;
using UserManagment.Application;
using UserManagment.Exceptions;
using UserManagment.Infrastructure;

namespace UserManagment
{
    public class UserMailerService : IUserMailerService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly ITicketSender _ticketSender;

        public UserMailerService(ITicketSender ticketSender,
            ITicketRepository ticketRepository, IGenericRepository<User> userRepository)
        {
            _ticketSender = ticketSender;
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
        }

        public async Task BeginEmailConfirmation(int userId)
        {
            Require.Positive(userId, nameof(userId));

            var userToConfirm = _userRepository.GetById(userId);

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

            _ticketRepository.Create(confirmationTicket);
            await _ticketSender.SendTicket(confirmationTicket.EmailToSend, confirmationTicket);
        }

        public async Task BeginPasswordRecovery(int userId)
        {
            Require.Positive(userId, nameof(userId));

            var userToChangePassword = _userRepository.GetById(userId);

            if (userToChangePassword == null)
                throw new OperationOnUserThatNotExistsException("start password changing");

            var passwordRecoveyTicket = new Ticket
            {
                TicketType = TicketType.PasswordRecovery,
                EmailToSend = userToChangePassword.Email,
                Token = GenerateToken(),
                IsCompleted = false
            };

            _ticketRepository.Create(passwordRecoveyTicket);
            await _ticketSender.SendTicket(userToChangePassword.Email, passwordRecoveyTicket);
        }

        public async Task BeginPasswordSettingAndEmailVerification(int userId)
        {
            Require.Positive(userId, nameof(userId));

            var userToConfirmAndSetPassword = _userRepository.GetById(userId);

            if (userToConfirmAndSetPassword == null)
                throw new OperationOnUserThatNotExistsException("start email confirmation & setting password");

            if (userToConfirmAndSetPassword.IsEmailConfirmed)
                throw new EmailAlreadyConfirmedException();

            var comboTicket = new Ticket
            {
                TicketType = TicketType.CombinatedTicket,
                EmailToSend = userToConfirmAndSetPassword.Email,
                Token = GenerateToken(),
                IsCompleted = false
            };

            _ticketRepository.Create(comboTicket);
            await _ticketSender.SendTicket(comboTicket.EmailToSend, comboTicket);
        }

        private string GenerateToken()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}