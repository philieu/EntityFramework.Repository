using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace EntityFramework.Repository
{
    public interface IObjectSetFactory<TContext> : IDisposable
        where TContext: IDbContext
    {
        IObjectSet<T> CreateObjectSet<T>() where T : class;
        void ChangeObjectState(object entity, EntityState state);
    }
}
