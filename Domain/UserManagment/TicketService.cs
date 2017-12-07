using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Journalist;
using UserManagment.Application;
using UserManagment.Exceptions;
using UserManagment.Infrastructure;

namespace UserManagment
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public IEnumerable<Ticket> GetTicketsByEmailAndType(string email, TicketType ticketType)
        {
            return _ticketRepository.FindBy(
                ticket => 
                    ticket.TicketType == ticketType
                    && ticket.EmailToSend == email).ToList();
        }

        public string GetUserEmailByPasswordRecoveyToken(string token)
        {
            Require.NotEmpty(token, nameof(token));

            return _ticketRepository.FindBy(
                ticket =>
                    ticket.TicketType == TicketType.PasswordRecovery
                    && ticket.Token == token).SingleOrDefault()?.EmailToSend;
        }

        public string GetUserEmailByEmailConfirmationToken(string token)
        {
            
            Require.NotEmpty(token, nameof(token));

            return _ticketRepository.FindBy(
                ticket => 
                    ticket.TicketType == TicketType.EmailConfirmation
                    && ticket.Token == token).SingleOrDefault()?.EmailToSend;
        }

        public string GetUserEmailByCombinatedTicket(string token)
        {
            Require.NotEmpty(token, nameof(token));

            return _ticketRepository.FindBy(
                ticket => 
                    ticket.TicketType == TicketType.CombinatedTicket
                    && ticket.Token == token).SingleOrDefault()?.EmailToSend;
        }

        public void CompleteAllTicketsByEmailAndType(string email, TicketType ticketType)
        {
            var allTicketsForThatTask = GetTicketsByEmailAndType(email, ticketType).ToList();

            foreach (var ticket in allTicketsForThatTask)
            {
                ticket.IsCompleted = true;
            }

            _ticketRepository.UpdateManyTickets(allTicketsForThatTask);
        }
    }
}