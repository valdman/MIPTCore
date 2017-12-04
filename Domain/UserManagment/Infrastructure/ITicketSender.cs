using System.Threading.Tasks;

namespace UserManagment.Infrastructure
{
    public interface ITicketSender
    {
        Task SendTicket(string email, Ticket ticketToSend);
    }
}