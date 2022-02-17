namespace PidpTests;

using AutoMapper;
using FakeItEasy.Sdk;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Reflection;

using Pidp.Data;

public class InMemoryDbTest : IDisposable
{
    protected PidpDbContext TestDb { get; }

    protected InMemoryDbTest()
    {
        var options = new DbContextOptionsBuilder<PidpDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        this.TestDb = new PidpDbContext(options, SystemClock.Instance);
        this.TestDb.Database.EnsureCreated();
    }

    public void Dispose()
    {
        this.TestDb.Database.EnsureDeleted();
        this.TestDb.Dispose();
        GC.SuppressFinalize(this);
    }

    public static IMapper DefaultMapper() => new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetAssembly(typeof(PidpDbContext)))).CreateMapper();

    /// <summary>
    /// Creates an instance of the given Type, injecting the TestDb, Automapper Configuration, and empty mocks for all dependencies (as applicable).
    /// Any supplied implementations or mocks of Interfaces will be used instead of empty mocks.
    /// Requires exactly one public constructor on the Type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dependencyOverrides"></param>
    /// <returns></returns>
    public T MockDependenciesFor<T>(params object[] dependencyOverrides)
    {
        var ctor = typeof(T).GetConstructors().Single();

        var parameters = new List<object>();
        foreach (var parameterType in ctor.GetParameters().Select(p => p.ParameterType))
        {
            object defaultParameter;
            if (parameterType == typeof(PidpDbContext))
            {
                defaultParameter = this.TestDb;
            }
            else if (parameterType == typeof(IMapper))
            {
                defaultParameter = DefaultMapper();
            }
            else
            {
                defaultParameter = Create.Fake(parameterType);
            }

            parameters.Add(
                dependencyOverrides.SingleOrDefault(x => x.GetType().GetInterfaces().Contains(parameterType))
                    ?? defaultParameter
            );
        }

        return (T)ctor.Invoke(parameters.ToArray());
    }
}
