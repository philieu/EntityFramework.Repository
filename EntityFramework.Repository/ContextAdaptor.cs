using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq.Expressions;

namespace EntityFramework.Repository
{
    public class ContextAdaptor<TContext> : IObjectSetFactory<TContext>, IObjectContext<TContext>
        where TContext: IDbContext
    {
        private readonly IDbContext _dbContext;
        private readonly ObjectContext _context;

        public ContextAdaptor(TContext dbContext)
        {
            _dbContext = dbContext;
            _context = dbContext.GetObjectContext();
            _context.ContextOptions.LazyLoadingEnabled = true;
        }

        public virtual void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void ExecuteStoreCommand(string commandText)
        {
            _context.ExecuteStoreCommand(commandText);
        }

        public DbContextTransaction BeginTransaction()
        {
            return _dbContext.Database.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public TEntity Reload<TEntity>(TEntity entity)
        {
            _context.Refresh(RefreshMode.StoreWins, entity);
            return entity;
        }

        public void MarkPropertyAsModified<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertySelector)
            where TEntity : class
        {
            var entry = _dbContext.Entry(entity);
            entry.Property(propertySelector).IsModified = true;
        }

        public ObjectContextOptions ContextOptions => _context.ContextOptions;

        public IObjectSet<T> CreateObjectSet<T>() where T : class
        {
            return _context.CreateObjectSet<T>();
        }

        public void ChangeObjectState(object entity, EntityState state)
        {
            _context.ObjectStateManager.ChangeObjectState(entity, state);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context?.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
