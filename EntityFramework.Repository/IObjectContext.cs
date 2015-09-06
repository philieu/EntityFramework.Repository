using System;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Linq.Expressions;

namespace EntityFramework.Repository
{
    public interface IObjectContext<TContext> : IDisposable
        where TContext: IDbContext
    {
        void SaveChanges();
        ObjectContextOptions ContextOptions { get; }
        TEntity Reload<TEntity>(TEntity entity);
        void MarkPropertyAsModified<TEntity, TProperty>(TEntity entity,
            Expression<Func<TEntity, TProperty>> propertySelector)
            where TEntity : class;

        void ExecuteStoreCommand(string commandText);
        void OpenConnection();
        void CloseConnection();
        DbTransaction BeginTransaction();
    }
}
