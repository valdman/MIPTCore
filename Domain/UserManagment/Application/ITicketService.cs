using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserManagment.Application
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetTicketsByEmailAndType(string email, TicketType ticketType);
        Task<string> GetUserEmailByPasswordRecoveyToken(string token);
        Task<string> GetUserEmailByEmailConfirmationToken(string token);
        Task<string> GetUserEmailByCombinatedTicket(string token);
        
        Task CompleteAllTicketsByEmailAndType(string email, TicketType ticketType);
    }
}