using System;

namespace EntityFramework.Repository
{
    public interface IUnitOfWorkSession<TContext> : IDisposable
        where TContext : IDbContext
    {
        void Commit();
    }
}