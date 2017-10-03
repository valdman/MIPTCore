using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common.Infrastructure;

namespace UserManagment.Infrastructure
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        Task UpdateManyTicketsAsync(IEnumerable<Ticket> tickets);
    }
}