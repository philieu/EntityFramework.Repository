using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace EntityFramework.Repository
{
    public class Repository<TContext, TEntity> : IRepository<TContext, TEntity>
        where TEntity : class
        where TContext: IDbContext
    {
        private readonly IObjectSet<TEntity> _objectSet;
        private readonly IObjectSetFactory<TContext> _objectSetFactory;

        public Repository(IObjectSetFactory<TContext> objectSetFactory)
        {
            _objectSet = objectSetFactory.CreateObjectSet<TEntity>();
            _objectSetFactory = objectSetFactory;
        }

        public virtual IQueryable<TEntity> AsQueryable()
        {
            return _objectSet;
        }

        public virtual int CountAll(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return query.Count();
        }

        public virtual int Count(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return query.Count(where);
        }

        public virtual IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return query.ToList();
        }

        public virtual IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> where,
                                   params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return query.Where(where);
        }

        public virtual TEntity Single(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return query.Single(where);
        }

        public virtual TEntity First(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return query.First(where);
        }

        public virtual TEntity SingleOrDefault(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return query.SingleOrDefault(where);
        }

        public virtual void Delete(TEntity entity)
        {
            _objectSet.DeleteObject(entity);
        }

        public virtual void Insert(TEntity entity)
        {
            _objectSet.AddObject(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _objectSet.Attach(entity);
            _objectSetFactory.ChangeObjectState(entity, EntityState.Modified);
        }

        private static IQueryable<TEntity> PerformInclusions(IEnumerable<Expression<Func<TEntity, object>>> includeProperties,
                                                       IQueryable<TEntity> query)
        {
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return query.FirstOrDefault(where);
        }
    }
}
