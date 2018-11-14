using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using BannerHelper;
using Common.Entities.Entities.ReadModifiers;
using Common.ReadModifiers;
using DataAccess.Contexts;
using Journalist;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class BannerRepository : IBannerRepository
    {
        private readonly BannerContext _context;
        private readonly DbSet<BannerElement> _db;
        
        public BannerRepository(BannerContext context)
        {
            _context = context;
            _db = context.Banner;
        }
        
        public BannerElement GetById(int id)
        {
            return _db.Find(id);
        }

        public IEnumerable<BannerElement> GetAll()
        {
            var enriesToReturn = _db.ToList();
            return enriesToReturn;
        }

        public IQueryable<BannerElement> AsQueryable()
        {
            return _db.AsQueryable();
        }

        public (int, IEnumerable<BannerElement>) GetAllForPagination(PaginationParams paginationParams, OrderingParams orderingParams, IEnumerable<FilteringParams> filteringParams, Expression<Func<BannerElement, bool>> predicate = null)
        {
            Require.NotNull(predicate, nameof(predicate));
            
            var querry = predicate != null
                ? _db.Where(predicate)
                : _db;
            
            querry = filteringParams.Aggregate(querry, (current, filter) => !filter.IsEmpty()
                ? current.Where(filter.Linq(), filter.FilterField, filter.EqualTo, filter.From, filter.To)
                : current);
            var total = querry.Count();
            

            querry = querry
                .OrderBy($"{orderingParams.Field} {orderingParams.Order}, Id DESC")
                .Skip(paginationParams.PerPage * Math.Max(paginationParams.Page - 1, 0))
                .Take(paginationParams.PerPage);

            return (total, querry.ToList());
        }

        public IEnumerable<BannerElement> GetWithFiltersAndOrder(IEnumerable<FilteringParams> filteringParams, OrderingParams orderingParams)
        {
            var querry = _db.AsQueryable();
            
            querry = filteringParams.Aggregate(querry, (current, filter) => !filter.IsEmpty()
                ? current.Where(filter.Linq(), filter.FilterField, filter.EqualTo, filter.From, filter.To)
                : current);

            return querry.OrderBy($"{orderingParams.Field} {orderingParams.Order}, Id DESC");
        }

        public IEnumerable<BannerElement> FindBy(Expression<Func<BannerElement, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));
            
            var enriesToReturn = _db.Where(predicate).ToList();
            return enriesToReturn;
        }

        public int Create(BannerElement @object)
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

        public void Update(BannerElement @object)
        {
            _db.Update(@object);

            _context.SaveChanges();
        }

        public void DeleteAllBannerElements()
        {
            _db.RemoveRange(_db);
            _context.SaveChanges();
        }
    }
}