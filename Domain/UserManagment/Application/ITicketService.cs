using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserManagment.Application
{
    public interface ITicketService
    {
        IEnumerable<Ticket> GetTicketsByEmailAndType(string email, TicketType ticketType);
        string GetUserEmailByPasswordRecoveyToken(string token);
        string GetUserEmailByEmailConfirmationToken(string token);
        string GetUserEmailByCombinatedTicket(string token);
        
        void CompleteAllTicketsByEmailAndType(string email, TicketType ticketType);
    }
}