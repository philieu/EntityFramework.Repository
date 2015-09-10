using System;
using System.Data.Common;
using System.Data.Entity;

namespace EntityFramework.Repository
{
    public class UnitOfWorkSession<TContext> : IUnitOfWorkSession<TContext> where TContext: IDbContext
    {
        private readonly DbContextTransaction _dbTransaction;

        public UnitOfWorkSession(IObjectContext<TContext> objectContext)
        {
            _dbTransaction = objectContext.BeginTransaction();
        }

        public void Commit()
        {
            _dbTransaction.Commit();
        }

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing) _dbTransaction.Dispose();
            }

            _disposed = true;
        }
        
    }
}
