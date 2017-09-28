using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using UserManagment;
using UserManagment.Infrastructure;

namespace DataAccess.Repositories
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(TicketContext context) : base(context)
        {
            Db = context.Tickets;
        }

        protected override DbSet<Ticket> Db { get; }
        public async Task UpdateManyTicketsAsync(IEnumerable<Ticket> tickets)
        {
            Db.UpdateRange(tickets);
            await Save();
        }
    }
}