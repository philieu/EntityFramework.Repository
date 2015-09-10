using System;
using Autofac;
using EntityFramework.Repository.Tests.DatabaseFirst;
using FluentAssertions;
using NUnit.Framework;

namespace EntityFramework.Repository.Tests.CodeFirst
{
    [TestFixture]
    public class BasicOperationTests : TestBase 
    {
        private Role _role;
        private User _user;

        [SetUp]
        public void SetUp()
        {
            _role = new Role
            {
                Name = "Citizen"
            };

            _user = new User
            {
                FirstName = "Jane",
                LastName = "Doe",
            };

            _role.Users.Add(_user);
        }

        [Test]
        public void Should_work_with_an_ioc_container()
        {
            // Arrange
            var roleRepository = GlobalSetup.Container.Resolve<IRepository<ICodeFirstTestDbContext, Role>>();
            var unitOfWork = GlobalSetup.Container.Resolve<IUnitOfWork<ICodeFirstTestDbContext>>();

            // Act
            roleRepository.Insert(_role);
            unitOfWork.SaveChanges();

            // Assert
            HasRole(_role.Name).Should().BeTrue();
            HasUser(_user.FirstName, _user.LastName).Should().BeTrue();
        }

        [Test]
        public void Should_work_without_an_ioc_container()
        {
            // Arrange
            // Act
            using (var context = new CodeFirstTestDbContext())
            using (var contextAdapter = new ContextAdaptor<ICodeFirstTestDbContext>(context))
            using (var unitOfWork = new UnitOfWork<ICodeFirstTestDbContext>(contextAdapter))
            {
                var roleRepository = new Repository<ICodeFirstTestDbContext, Role>(contextAdapter);
                roleRepository.Insert(_role);
                unitOfWork.SaveChanges();
            }

            // Assert
            HasRole(_role.Name).Should().BeTrue();
            HasUser(_user.FirstName, _user.LastName).Should().BeTrue();
        }

        [Test]
        public void Should_be_able_to_manage_transaction_manually()
        {
            // Arrange
            var roleRepository = GlobalSetup.Container.Resolve<IRepository<ICodeFirstTestDbContext, Role>>();
            var unitOfWork = GlobalSetup.Container.Resolve<IUnitOfWork<ICodeFirstTestDbContext>>();

            // Act
            using (var session = unitOfWork.StartSession())
            {
                roleRepository.Insert(_role);
                unitOfWork.SaveChanges();

                session.Commit();
            }

            // Assert
            HasRole(_role.Name).Should().BeTrue();
            HasUser(_user.FirstName, _user.LastName).Should().BeTrue();
        }

        [Test]
        public void Should_rollback_transaction_if_no_commit()
        {
            // Arrange
            var roleRepository = GlobalSetup.Container.Resolve<IRepository<ICodeFirstTestDbContext, Role>>();
            var unitOfWork = GlobalSetup.Container.Resolve<IUnitOfWork<ICodeFirstTestDbContext>>();

            // Act
            using (var session = unitOfWork.StartSession())
            {
                roleRepository.Insert(_role);
                unitOfWork.SaveChanges();
            }

            // Assert
            HasRole(_role.Name).Should().BeFalse();
            HasUser(_user.FirstName, _user.LastName).Should().BeFalse();
        }


    }
}
