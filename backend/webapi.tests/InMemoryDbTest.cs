using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;

namespace PidpTests
{
    public class InMemoryDbTest : IDisposable
    {
        protected PidpDbContext TestDb;

        protected InMemoryDbTest()
        {
            var options = new DbContextOptionsBuilder<PidpDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            TestDb = new PidpDbContext(options, SystemClock.Instance);
            TestDb.Database.EnsureCreated();
        }

        public void Dispose()
        {
            TestDb.Database.EnsureDeleted();
            TestDb.Dispose();
        }
    }
}
