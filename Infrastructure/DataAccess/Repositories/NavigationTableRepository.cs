using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CapitalsTableHelper;
using DataAccess.Contexts;
using Journalist;
using Microsoft.EntityFrameworkCore;
using NavigationHelper;

namespace DataAccess.Repositories
{
    public class NavigationTableRepository : INavigationTableRepository
    {
        private readonly NavigationTableContext _context;
        private readonly DbSet<NavigationTableEntry> _db;
        
        public NavigationTableRepository(NavigationTableContext context)
        {
            _context = context;
            _db = context.NavigationTable;
        }
        
        public Task<NavigationTableEntry> GetByIdAsync(int id)
        {
            return _db.FindAsync(id);
        }

        public Task<IEnumerable<NavigationTableEntry>> GetAll()
        {
            var enriesToReturn = _db.ToList();
            return Task.FromResult((IEnumerable<NavigationTableEntry>) enriesToReturn);
        }

        public Task<IEnumerable<NavigationTableEntry>> FindByAsync(Expression<Func<NavigationTableEntry, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));
            
            var enriesToReturn = _db.Where(predicate).ToList();
            return Task.FromResult((IEnumerable<NavigationTableEntry>) enriesToReturn);
        }

        public async Task<int> CreateAsync(NavigationTableEntry @object)
        {
            await _db.AddAsync(@object);
            await _context.SaveChangesAsync();

            return @object.Id;
        }

        public async Task DeleteAsync(int objectId)
        {
            var entryToDelete = await GetByIdAsync(objectId);
            if(entryToDelete != null)
                _db.Remove(entryToDelete);
            await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(NavigationTableEntry @object)
        {
            _db.Update(@object);

            return _context.SaveChangesAsync();
        }

        public Task DeleteAllNavigatioTableEntriesAsync()
        {
            _db.RemoveRange(_db);
            return _context.SaveChangesAsync();
        }
    }
}