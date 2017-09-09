using System;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace DataAccess
{
    public class DbSessionProvider<T> where T : PersistentEntity
    {
        private readonly DbContextOptions _connectionOptions;
        private InnerContext _currentInnerContext;

        public DbSessionProvider(DbContextOptions connectionOptions)
        {
            _connectionOptions = connectionOptions;
        }


        public DbSet<T> CurrentSession { get; private set; }

        public void OpenSession()
        {
            _currentInnerContext = new InnerContext(_connectionOptions);
            CurrentSession = _currentInnerContext.Objects;
        }

        public void CloseSession()
        {
            if (CurrentSession == null)
            {
                Debug.WriteLine("Attend to close closed session");
            }

            CurrentSession = null;
            _currentInnerContext.Dispose();
        }

        private class InnerContext : DbContext
        {
            public InnerContext(DbContextOptions connectionOptions) : base(connectionOptions)
            {
            }

            public DbSet<T> Objects { get; private set; }
        }
    }
}