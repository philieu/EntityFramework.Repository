using System.Collections.Generic;

namespace EntityFramework.Repository
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>
        where TContext: IDbContext
    {
        private readonly IObjectContext<TContext> _objectContext;
        private readonly List<UnitOfWorkSession<TContext>> _sessions;

        public UnitOfWork(IObjectContext<TContext> objectContext)
        {
            _objectContext = objectContext;
            _sessions = new List<UnitOfWorkSession<TContext>>();
        }

        public void SaveChanges()
        {
            _objectContext.SaveChanges();
        }

        public bool LazyLoadingEnabled
        {
            set { _objectContext.ContextOptions.LazyLoadingEnabled = value; }
            get { return _objectContext.ContextOptions.LazyLoadingEnabled; }
        }

        public IUnitOfWorkSession<TContext> StartSession()
        {
            var session = new UnitOfWorkSession<TContext>(_objectContext);
            _sessions.Add(session);

            return session;
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing && _objectContext != null)
                {
                    _objectContext.Dispose();
                }

                _sessions.ForEach(session => session.Dispose());
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
