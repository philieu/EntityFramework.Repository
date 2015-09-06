using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Linq.Expressions;
using System.Reflection;

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

        public void OpenConnection()
        {
            _context.Connection.Open();
        }

        public void CloseConnection()
        {
            _context.Connection.Close();
        }

        public DbTransaction BeginTransaction()
        {
            return _context.Connection.BeginTransaction(IsolationLevel.ReadCommitted);
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

        public ObjectContextOptions ContextOptions
        {
            get { return _context.ContextOptions; }
        }

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
                if (disposing && _context != null)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
