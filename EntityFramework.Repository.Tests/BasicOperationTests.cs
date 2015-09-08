using System;
using Autofac;
using EntityFramework.Repository.Tests.Data;
using FluentAssertions;
using NUnit.Framework;

namespace EntityFramework.Repository.Tests
{
    [TestFixture]
    public class BasicOperationTests : TestBase 
    {
        private Transaction _transaction;
        private Account _account;

        [SetUp]
        public void SetUp()
        {
            var accountName = DateTime.Now.ToString("yyyyMMddTHH:mm:ss.fff");
            var transactionDescription = DateTime.Now.ToString("yyyyMMddTHH:mm:ss.fff");
            _account = new Account
            {
                Name = accountName,
                Balance = 10,
            };
            _transaction = new Transaction
            {
                Description = transactionDescription,
                Amount = 20
            };
            _account.Transactions.Add(_transaction);

        }

        [Test]
        public void Should_work_with_an_ioc_container()
        {
            // Arrange
            var accountRepository = GlobalSetup.Container.Resolve<IRepository<ITestDbContext, Account>>();
            var unitOfWork = GlobalSetup.Container.Resolve<IUnitOfWork<ITestDbContext>>();

            // Act
            accountRepository.Insert(_account);
            unitOfWork.SaveChanges();

            // Assert
            HasAccount(_account .Name).Should().BeTrue();
            HasTransaction(_account.Name, _transaction.Description).Should().BeTrue();
        }

        [Test]
        public void Should_work_without_an_ioc_container()
        {
            // Arrange
            // Act
            using (var context = new TestDbEntities())
            using (var contextAdapter = new ContextAdaptor<ITestDbContext>(context))
            using (var unitOfWork = new UnitOfWork<ITestDbContext>(contextAdapter))
            {
                var accountRepository = new Repository<ITestDbContext, Account>(contextAdapter);
                accountRepository.Insert(_account);

                unitOfWork.SaveChanges();
            }

            // Assert
            HasAccount(_account.Name).Should().BeTrue();
            HasTransaction(_account.Name, _transaction.Description).Should().BeTrue();
        }
    }
}
