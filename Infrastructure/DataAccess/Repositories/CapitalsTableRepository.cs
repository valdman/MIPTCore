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
using NavigationHelper;

namespace DataAccess.Repositories
{
    public class CapitalsTableRepository : ICapitalsTableEntryRepository
    {
        private readonly WithImageContext _context;
        private readonly DbSet<CapitalsTableEntry> _db;
        
        public CapitalsTableRepository(WithImageContext context)
        {
            _context = context;
            _db = context.CapitalsTableEntries;
        }
        
        public CapitalsTableEntry GetById(int id)
        {
            return _db.Find(id);
        }

        public IEnumerable<CapitalsTableEntry> GetAll()
        {
            var enriesToReturn = _db.ToList();
            return enriesToReturn;
        }

        public IEnumerable<CapitalsTableEntry> FindBy(Expression<Func<CapitalsTableEntry, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));
            
            var enriesToReturn = _db.Where(predicate).ToList();
            return enriesToReturn;
        }

        public int Create(CapitalsTableEntry @object)
        {
            _db.Add(@object);

            _context.SaveChanges();

            return @object.CapitalId;
        }

        public void Delete(int objectId)
        {
            var entryToDelete = GetById(objectId);
            if(entryToDelete != null)
                _db.Remove(entryToDelete);
            _context.SaveChanges();
        }

        public void Update(CapitalsTableEntry @object)
        {
            _db.Update(@object);

            _context.SaveChanges();
        }

        public void DeleteAllCapitalTableEntries()
        {
            _db.RemoveRange(_db);
            _context.SaveChanges();
        }
    }
}