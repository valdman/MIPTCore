using System;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class DbSessionProvider<T> where T : PersistentEntity
    {
        private readonly DbContextOptions _dbContextOptions;

        public DbSessionProvider(DbContextOptions dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

        public EntityContext<T> GetEntityContext()
        {
            return new EntityContext<T>(_dbContextOptions);
        }

        public class EntityContext<TR> : DbContext, IDisposable where TR : PersistentEntity
        {
            public EntityContext(DbContextOptions connetcionOptions) : base(connetcionOptions)
            {
                
            }
            
            public DbSet<TR> DbSet { get; private set; }


            public new void Dispose()
            {
                base.Dispose();
            }
        }
    }
}