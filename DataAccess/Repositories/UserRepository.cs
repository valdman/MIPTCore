using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataAccess.Contexts;
using Journalist;
using Microsoft.EntityFrameworkCore;
using UserManagment;

namespace DataAccess.Repositories
{
    public class UserRepository : GenericRepository<User>
    {   
        public UserRepository(UserContext context) : base(context)
        {
            Db = context.Users;
        }

        protected override DbSet<User> Db { get; }
    }
}