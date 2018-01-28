using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using Common.Entities.Entities.ReadModifiers;
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

        public (int, IEnumerable<NavigationTableEntry>) GetAllForPagination(PaginationParams paginationParams, OrderingParams orderingParams, IEnumerable<FilteringParams> filteringParams, Expression<Func<NavigationTableEntry, bool>> predicate = null)
        {
            Require.NotNull(predicate, nameof(predicate));
            
            var querry = predicate != null
                ? _db.Where(predicate)
                : _db;
            
            querry = filteringParams.Aggregate(querry, (current, filter) => !filter.IsEmpty()
                ? current.Where(filter.Linq())
                : current);
            var total = querry.Count();
            

            querry = querry
                .OrderBy(n => n.Id)
                .Skip(paginationParams.PerPage * Math.Max(paginationParams.Page - 1, 0))
                .Take(paginationParams.PerPage);

            return (total, querry.ToList());
        }

        public IEnumerable<NavigationTableEntry> GetWithFiltersAndOrder(IEnumerable<FilteringParams> filteringParams, OrderingParams orderingParams)
        {
            var querry = _db.AsQueryable();
            
            querry = filteringParams.Aggregate(querry, (current, filter) => !filter.IsEmpty()
                ? current.Where(filter.Linq())
                : current);

            return querry.OrderBy($"{orderingParams.Field} {orderingParams.Order}, CreatingTime DESC, Id DESC");
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