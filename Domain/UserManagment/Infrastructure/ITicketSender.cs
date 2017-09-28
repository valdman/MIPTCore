using System.Threading.Tasks;

namespace UserManagment.Infrastructure
{
    public interface ITicketSender
    {
        Task SendTicketAsync(string email, Ticket ticketToSend);
    }
}