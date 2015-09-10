EntityFramework.Repository
==========================

This project implements the Repository pattern for Entity Framework.

Nuget package: https://www.nuget.org/packages/EntityFramework.Repository

# How to use

Please have a look at the test project for an actual working implementation on how to use the library.

Below is a brief quick start guide.

## Database first
 
Follow the steps below to use the repository in a database first scenario:
- Create the EDMX model as you normally would using Visual Studio
- Create a partial class of the DB context in the step above and implements `IDbContext`. For example:

```C#
    public partial class TestDbEntities : IDbContext
    {
    }  
```

- The repository is now ready to use.

Here is a very basic way to use it without dependency injection:

```C#
    using (var context = new TestDbEntities())
    using (var contextAdapter = new ContextAdaptor<IDbContext>(context))
    using (var unitOfWork = new UnitOfWork<IDbContext>(contextAdapter))
    {
        var accountRepository = new Repository<IDbContext, Account>(contextAdapter);
        accountRepository.Insert(account);
    
        unitOfWork.SaveChanges();
    } 
```

## Code first
In case of code first, here are the steps:
- Create your context, make it inherits `DbContext` and implements `IDbContext`
- Create your domain classes as you normally would with EntityFramework.
- The repository is now ready to use. Use it the same way as with the database first scenario above.
 
## How to manage transaction
Normally, every `UnitOfWork.SaveChanges()` would be executed in a transaction. If you would like to manage the transaction manually, use `IUnitOfWorkSession`.

```C#
    using (var session = unitOfWork.StartSession())
    {
        transactionRepository.Insert(transaction);
        unitOfWork.SaveChanges();
    
        accountRepository.Insert(account);
        unitOfWork.SaveChanges();
    
        session.Commit();
    }
```
 
## Using with an IoC container (e.g. Autofac for example)
Below is an example on how to register and use with Autofac as the IoC container.
- Registration

```C#
    var builder = new ContainerBuilder();
    builder.RegisterAssemblyTypes(typeof(IDbContext).Assembly)
        .AsImplementedInterfaces().InstancePerLifetimeScope();
    
    builder.RegisterGeneric(typeof(ContextAdaptor<>))
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope();
    
    builder.RegisterGeneric(typeof(UnitOfWork<>))
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope();
    
    builder.RegisterGeneric(typeof(Repository<,>))
        .As(typeof(IRepository<,>))
        .InstancePerLifetimeScope();
    
    var container = builder.Build();
```

- How to use

```C#
    var accountRepository = container.Resolve<IRepository<IDbContext, Account>>();
    var unitOfWork = container.Resolve<IUnitOfWork<IDbContext>>();
```