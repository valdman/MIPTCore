using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CapitalsTableHelper;
using Common;
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
        
        public NavigationTableEntry GetById(int id)
        {
            return _db.Find(id);
        }

        public IEnumerable<NavigationTableEntry> GetAll()
        {
            var enriesToReturn = _db.ToList();
            return enriesToReturn;
        }

        public (int, IEnumerable<NavigationTableEntry>) GetAllForPagination(PaginationAndFilteringParams filteringParams, Expression<Func<NavigationTableEntry, bool>> predicate = null)
        {
            Require.NotNull(predicate, nameof(predicate));
            
            var querry = predicate != null
                ? _db.Where(predicate)
                : _db;

            var total = querry.Count();

            querry = querry
                .OrderBy(n => n.Id)
                .Skip(filteringParams.PerPage * Math.Max(filteringParams.Page, 1))
                .Take(filteringParams.PerPage);

            return (total, querry.ToList());
        }

        public IEnumerable<NavigationTableEntry> FindBy(Expression<Func<NavigationTableEntry, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));
            
            var enriesToReturn = _db.Where(predicate).ToList();
            return enriesToReturn;
        }

        public int Create(NavigationTableEntry @object)
        {
            _db.AddAsync(@object);
            _context.SaveChangesAsync();

            return @object.Id;
        }

        public void Delete(int objectId)
        {
            var entryToDelete = GetById(objectId);
            if(entryToDelete != null)
                _db.Remove(entryToDelete);
            _context.SaveChangesAsync();
        }

        public void Update(NavigationTableEntry @object)
        {
            _db.Update(@object);

            _context.SaveChanges();
        }

        public void DeleteAllNavigatioTableEntries()
        {
            _db.RemoveRange(_db);
            _context.SaveChanges();
        }
    }
}