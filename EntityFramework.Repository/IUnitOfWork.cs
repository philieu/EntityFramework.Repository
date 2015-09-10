using System;

namespace EntityFramework.Repository
{
    public interface IUnitOfWork<TContext> : IDisposable
        where TContext: IDbContext
    {
        void SaveChanges();
        bool LazyLoadingEnabled { set; get; }
        IUnitOfWorkSession<TContext> StartSession();
    }
}
