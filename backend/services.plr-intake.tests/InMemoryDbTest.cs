using Microsoft.EntityFrameworkCore;

using PlrIntake.Data;

namespace PidpTests
{
    public class InMemoryDbTest : IDisposable
    {
        protected PlrDbContext TestDb;

        protected InMemoryDbTest()
        {
            var options = new DbContextOptionsBuilder<PlrDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            TestDb = new PlrDbContext(options);
            TestDb.Database.EnsureCreated();
        }

        public void Dispose()
        {
            TestDb.Database.EnsureDeleted();
            TestDb.Dispose();
        }
    }
}
