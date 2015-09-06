using System;
using System.Data.Common;

namespace EntityFramework.Repository
{
    public class UnitOfWorkSession<TContext> : IDisposable, IUnitOfWorkSession<TContext> where TContext: IDbContext
    {
        private readonly IObjectContext<TContext> _objectContext;
        private readonly DbTransaction _dbTransaction;
        private bool _hasCommitted;

        public UnitOfWorkSession(IObjectContext<TContext> objectContext)
        {
            _objectContext = objectContext;
            _objectContext.OpenConnection();
            _dbTransaction = _objectContext.BeginTransaction();
            _hasCommitted = false;
        }

        public void Commit()
        {
            _dbTransaction.Commit();
            _hasCommitted = true;
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
                if (disposing && _objectContext != null)
                {
                    if (!_hasCommitted) _dbTransaction.Rollback();
                    _objectContext.CloseConnection();
                }
            }
            _disposed = true;
        }
        
    }
}
