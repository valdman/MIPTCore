using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common.Entities;
using DataAccess.Contexts;
using Journalist;
using Microsoft.EntityFrameworkCore;
using NavigationHelper;
using UserManagment;

namespace DataAccess.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public override User GetById(int id)
        {
            Require.Positive(id, nameof(id));

            return Db
                .Include(u => u.AlumniProfile)
                .Include(u => u.Password)
                .SingleOrDefault(u => u.Id == id);
        }

        public override IEnumerable<User> GetAll()
        {
            return Db
                .Include(u => u.AlumniProfile)
                .ToList();
        }

        public override IEnumerable<User> FindBy(Expression<Func<User, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));

            return Db
                .Include(u => u.AlumniProfile)
                .Include(u => u.Password)                
                .Where(predicate).ToList();
        }

        public override void Update(User @object)
        {
            Require.NotNull(@object, nameof(@object));

            Save();
        }

        public UserRepository(UserContext context) : base(context)
        {
        }
    }
}