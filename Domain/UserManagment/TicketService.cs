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

        public async Task<IEnumerable<Ticket>> GetTicketsByEmailAndType(string email, TicketType ticketType)
        {
            return (await _ticketRepository.FindByAsync(
                ticket => 
                    ticket.TicketType == ticketType
                    && ticket.EmailToSend == email)
            ).ToList();
        }

        public async Task<string> GetUserEmailByPasswordRecoveyToken(string token)
        {
            Require.NotEmpty(token, nameof(token));

            return (await _ticketRepository.FindByAsync(
                    ticket =>
                        ticket.TicketType == TicketType.PasswordRecovery
                        && ticket.Token == token)
                ).SingleOrDefault()?.EmailToSend;
        }

        public async Task<string> GetUserEmailByEmailConfirmationToken(string token)
        {
            
            Require.NotEmpty(token, nameof(token));

            return (await _ticketRepository.FindByAsync(
                    ticket => 
                        ticket.TicketType == TicketType.EmailConfirmation
                        && ticket.Token == token)
                ).SingleOrDefault()?.EmailToSend;
        }

        public async Task<string> GetUserEmailByCombinatedTicket(string token)
        {
            Require.NotEmpty(token, nameof(token));

            return (await _ticketRepository.FindByAsync(
                ticket => 
                    ticket.TicketType == TicketType.CombinatedTicket
                    && ticket.Token == token)
            ).SingleOrDefault()?.EmailToSend;
        }

        public async Task CompleteAllTicketsByEmailAndType(string email, TicketType ticketType)
        {
            var allTicketsForThatTask = (await GetTicketsByEmailAndType(email, ticketType)).ToList();

            foreach (var ticket in allTicketsForThatTask)
            {
                ticket.IsCompleted = true;
            }

            await _ticketRepository.UpdateManyTicketsAsync(allTicketsForThatTask);
        }
    }
}