namespace PlrIntakeTests;

using FakeItEasy.Sdk;
using Microsoft.EntityFrameworkCore;

using PlrIntake.Data;

public class InMemoryDbTest : IDisposable
{
    protected PlrDbContext TestDb { get; }

    protected InMemoryDbTest()
    {
        var options = new DbContextOptionsBuilder<PlrDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        this.TestDb = new PlrDbContext(options);
        this.TestDb.Database.EnsureCreated();
    }

    public void Dispose()
    {
        this.TestDb.Database.EnsureDeleted();
        this.TestDb.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Creates an instance of the given Type, injecting the TestDb and empty mocks for all dependencies (as applicable).
    /// Any supplied implementations or mocks of Interfaces will be used instead of empty mocks.
    /// Requires exactly one public constructor on the Type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dependencyOverrides"></param>
    public T MockDependenciesFor<T>(params object[] dependencyOverrides)
    {
        var ctor = typeof(T).GetConstructors().Single();

        var parameters = new List<object>();
        foreach (var parameterType in ctor.GetParameters().Select(p => p.ParameterType))
        {
            object defaultParameter;
            if (parameterType == typeof(PlrDbContext))
            {
                defaultParameter = this.TestDb;
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
