using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CapitalsTableHelper;
using Common.Infrastructure;
using DataAccess.Contexts;
using Journalist;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class CapitalsTableRepository : ICapitalsTableEntryRepository
    {
        private readonly CapitalContext _context;
        private readonly DbSet<CapitalsTableEntry> _db;
        
        public CapitalsTableRepository(CapitalContext context)
        {
            _context = context;
            _db = context.CapitalsTableEntries;
        }
        
        public Task<CapitalsTableEntry> GetByIdAsync(int id)
        {
            return _db.FindAsync(id);
        }

        public Task<IEnumerable<CapitalsTableEntry>> GetAll()
        {
            var enriesToReturn = _db.ToList();
            return Task.FromResult((IEnumerable<CapitalsTableEntry>) enriesToReturn);
        }

        public Task<IEnumerable<CapitalsTableEntry>> FindByAsync(Expression<Func<CapitalsTableEntry, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));
            
            var enriesToReturn = _db.Where(predicate).ToList();
            return Task.FromResult((IEnumerable<CapitalsTableEntry>) enriesToReturn);
        }

        public async Task<int> CreateAsync(CapitalsTableEntry @object)
        {
            _db.Add(@object);

            await _context.SaveChangesAsync();

            return @object.CapitalId;
        }

        public async Task DeleteAsync(int objectId)
        {
            var entryToDelete = await GetByIdAsync(objectId);
            if(entryToDelete != null)
                _db.Remove(entryToDelete);
            await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(CapitalsTableEntry @object)
        {
            _db.Update(@object);

            return _context.SaveChangesAsync();
        }

        public Task DeleteAllCapitalTableEntriesAsync()
        {
            _db.RemoveRange(_db);
            return _context.SaveChangesAsync();
        }
    }
}