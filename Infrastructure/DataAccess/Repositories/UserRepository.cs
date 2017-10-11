using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common;
using DataAccess.Contexts;
using Journalist;
using Microsoft.EntityFrameworkCore;
using NavigationHelper;
using UserManagment;

namespace DataAccess.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public override Task<User> GetByIdAsync(int id)
        {
            Require.Positive(id, nameof(id));

            return Db
                .Include(u => u.AlumniProfile)
                .Include(u => u.Password)
                .Where(u => u.Id == id)
                .SingleOrDefaultAsync();
        }

        public override async Task<IEnumerable<User>> GetAll()
        {
            return await Db
                .Include(u => u.AlumniProfile)
                .ToListAsync();
        }

        public override async Task<IEnumerable<User>> FindByAsync(Expression<Func<User, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));

            return await Db
                .Include(u => u.AlumniProfile)
                .Include(u => u.Password)                
                .Where(predicate).ToListAsync();
        }

        public override async Task UpdateAsync(User @object)
        {
            Require.NotNull(@object, nameof(@object));

            await Save();
        }

        public UserRepository(UserContext context) : base(context)
        {
        }
    }
}