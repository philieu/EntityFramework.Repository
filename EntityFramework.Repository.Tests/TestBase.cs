using System.Linq;
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

        [SetUp]
        public void SetUpBase()
        {
            using (var context = new TestDbEntities())
            {
                context.Transactions.RemoveRange(context.Transactions.ToList());
                context.Accounts.RemoveRange(context.Accounts.ToList());
                context.SaveChanges();
            }
        }
    }
}
