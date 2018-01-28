using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Journalist;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic;
using Common.Abstractions;
using Common.Entities.Entities.ReadModifiers;
using Common.Infrastructure;

namespace DataAccess.Repositories
{
    public class GenericRepository<TEntity> :
        IGenericRepository<TEntity> where TEntity : PersistentEntity
    {
        protected readonly DbContext Context;
        protected readonly DbSet<TEntity> Db;

        protected GenericRepository(DbContext context)
        {
            Context = context;
            Db = Context.Set<TEntity>();
        }

        public virtual TEntity GetById(int id)
        {
            Require.Positive(id, nameof(id));
            
            var foundedObject = Db.Find(id);
            if (foundedObject == null || !foundedObject.IsDeleted)
            {
                return foundedObject;
            }
            return null;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Db
                .Where(@object => !@object.IsDeleted)
                .ToList();
        }

        public (int, IEnumerable<TEntity>) GetAllForPagination(PaginationParams paginationParams, OrderingParams orderingParams, IEnumerable<FilteringParams> filteringParams, Expression<Func<TEntity, bool>> predicate = null)
        {
            var aliveObjects = Db.Where(@object => !@object.IsDeleted);
            
            var querry = predicate != null
                ? aliveObjects.Where(predicate)
                : aliveObjects;

            querry = filteringParams.Aggregate(querry, (current, filter) => !filter.IsEmpty()
                ? current.Where(filter.Linq())
                : current);

            var total = querry.Count();

            var orderedResult = querry
                .OrderBy($"{orderingParams.Field} {orderingParams.Order}, CreatingTime DESC, Id DESC")
                .Skip(paginationParams.PerPage * Math.Max(paginationParams.Page - 1, 0))
                .Take(paginationParams.PerPage);

            return (total, orderedResult);
        }

        public IEnumerable<TEntity> GetWithFiltersAndOrder(IEnumerable<FilteringParams> filteringParams, OrderingParams orderingParams)
        {
            var aliveObjects = Db.Where(@object => !@object.IsDeleted);

            aliveObjects = filteringParams.Aggregate(aliveObjects, (current, filter) => !filter.IsEmpty()
                ? current.Where(filter.Linq())
                : current);

            var total = aliveObjects.Count();

            var orderedResult = aliveObjects
                .OrderBy($"{orderingParams.Field} {orderingParams.Order}, CreatingTime DESC, Id DESC");

            return orderedResult;
        }

        public virtual IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            Require.NotNull(predicate, nameof(predicate));
            
            return Db.Where(predicate)
                .Where(@object => !@object.IsDeleted)
                .ToList();
        }

        public virtual int Create(TEntity @object)
        {
            Require.NotNull(@object, nameof(@object));
            
            @object.CreatingTime = DateTimeOffset.Now;
            Db.Add(@object);
            
            Save();
            return @object.Id;
        }

        public virtual void Delete(int objectId)
        {
            Require.Positive(objectId, nameof(objectId));
            
            var objectToDelete = GetById(objectId);
            if(objectToDelete != null)
                objectToDelete.IsDeleted = true;
            
            Save();
        }

        public virtual void Update(TEntity @object)
        {
            Require.NotNull(@object, nameof(@object));

            Save();
        }

        protected void Save()
        {
            Context.SaveChanges();
        }
    }
}