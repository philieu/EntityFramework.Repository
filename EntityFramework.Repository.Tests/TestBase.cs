using System.Linq;
using EntityFramework.Repository.Tests.CodeFirst;
using EntityFramework.Repository.Tests.DatabaseFirst;
using NUnit.Framework;

namespace EntityFramework.Repository.Tests
{
    public abstract class TestBase
    {
        protected bool HasAccount(string accountName)
        {
            using (var context = new TestDbEntities())
            {
                return context.Accounts.Any(acc => acc.Name == accountName);
            }
        }

        protected bool HasTransaction(string accountName, string transactionDescription)
        {
            using (var context = new TestDbEntities())
            {
                return
                    context.Accounts.Any(
                        acc =>
                            acc.Name == accountName &&
                            acc.Transactions.Any(tr => tr.Description == transactionDescription));
            }
        }

        protected bool HasRole(string roleName)
        {
            using (var context = new CodeFirstTestDbContext())
            {
                return
                    context.Roles.Any(
                        role =>
                            role.Name == roleName);
            }
        }

        protected bool HasUser(string firstName, string lastName)
        {
            using (var context = new CodeFirstTestDbContext())
            {
                return
                    context.Users.Any(
                        user =>
                            user.FirstName == firstName && user.LastName == lastName);
            }
        }

        [SetUp]
        public void SetUpBase()
        {
            using (var context = new TestDbEntities())
            {
                context.Transactions.RemoveRange(context.Transactions.ToList());
                context.Accounts.RemoveRange(context.Accounts.ToList());
                context.SaveChanges();
            }

            using (var context = new CodeFirstTestDbContext())
            {
                context.Users.RemoveRange(context.Users.ToList());
                context.Roles.RemoveRange(context.Roles.ToList());
                context.SaveChanges();
            }
        }
    }
}
