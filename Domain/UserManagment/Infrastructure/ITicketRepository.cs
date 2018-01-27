using System.Collections.Generic;
using Common.Infrastructure;

namespace UserManagment.Infrastructure
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        void UpdateManyTickets(IEnumerable<Ticket> tickets);
    }
}