using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Contexts;
using UserManagment;
using UserManagment.Infrastructure;

namespace DataAccess.Repositories
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(TicketContext context) : base(context)
        {
        }

        public async Task UpdateManyTicketsAsync(IEnumerable<Ticket> tickets)
        {
            Db.UpdateRange(tickets);
            
            await Save();
        }
    }
}