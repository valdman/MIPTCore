using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using CapitalsTableHelper;
using Common;
using DataAccess.Contexts;
using Journalist;
using Journalist.Extensions;
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

        public IEnumerable<NavigationTableEntry> GetAll(PaginationAndFilteringParams filteringParams)
        {
            IOrderedQueryable<NavigationTableEntry> querry;
            
            if (filteringParams.Field.IsNotNullOrEmpty())
                return _db.OrderBy($"{filteringParams.Field} {filteringParams.Order}")   
                        .Skip(filteringParams.Skip)
                        .Take(filteringParams.Take)
                        .ToList();
            
            return _db.ToList();
        }

        public IEnumerable<NavigationTableEntry> FindBy(Expression<Func<NavigationTableEntry, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));
            
            var enriesToReturn = _db.Where(predicate).ToList();
            return enriesToReturn;
        }

        public IEnumerable<NavigationTableEntry> FindBy(Expression<Func<NavigationTableEntry, bool>> predicate,
            PaginationAndFilteringParams filteringParams)
        {
            Require.NotNull(predicate, nameof(predicate));

            var querry = predicate != null
                ? _db.Where(predicate)
                : _db;

            if (!filteringParams.Field.IsNotNullOrEmpty())
                querry = querry.OrderBy($"{filteringParams.Field} {filteringParams.Order}");

            querry = querry
                .Skip(filteringParams.Skip)
                .Take(filteringParams.Take);

            return querry.AsEnumerable<NavigationTableEntry>();
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