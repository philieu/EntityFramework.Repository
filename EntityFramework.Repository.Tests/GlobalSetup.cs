using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using EntityFramework.Repository;
using NUnit.Framework;

// ReSharper disable once CheckNamespace
[SetUpFixture]
public class GlobalSetup
{
    public static IContainer Container;

    [SetUp]
    public static void SetUp()
    {
        var executingAssembly = new FileInfo(typeof(GlobalSetup).Assembly.Location);
        var directoryInfo = new DirectoryInfo(Path.Combine(executingAssembly.Directory.FullName, "Data"));
        AppDomain.CurrentDomain.SetData("DataDirectory", directoryInfo.FullName);

        ConfigureDependency();
    }

    public static void TearDown()
    {
        Container?.Dispose();
    }

    private static void ConfigureDependency()
    {
        var builder = new ContainerBuilder();

        builder.RegisterAssemblyTypes(typeof(IDbContext).Assembly)
            .AsImplementedInterfaces().InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(typeof(GlobalSetup).Assembly)
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

        Container = builder.Build();
    }
}
