using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using EntityFramework.Repository.Tests.Data;
using NUnit.Framework;

namespace EntityFramework.Repository.Tests
{
    [TestFixture]
    public class BasicOperationTests
    {
        [Test]
        public void Test1()
        {
            var accountRepository = GlobalSetup.Container.Resolve<IRepository<ITestDbContext, Account>>();
            var unitOfWork = GlobalSetup.Container.Resolve<IUnitOfWork<ITestDbContext>>();
            accountRepository.Insert(new Account
            {
                Name = DateTime.Now.ToString(CultureInfo.CurrentCulture),
                Balance = 10,
            });

            unitOfWork.SaveChanges();
        }
    }
}
