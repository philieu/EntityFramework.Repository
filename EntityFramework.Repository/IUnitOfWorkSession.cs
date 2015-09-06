namespace EntityFramework.Repository
{
    public interface IUnitOfWorkSession<TContext>
        where TContext : IDbContext
    {
        void Commit();
    }
}