using System.Data.Entity;

namespace EntityFramework.Repository.Tests.CodeFirst
{
    public class CodeFirstTestDbContext : DbContext, ICodeFirstTestDbContext
    {
        public CodeFirstTestDbContext() : base("name=CodeFirstTestDb")
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<CodeFirstTestDbContext>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}