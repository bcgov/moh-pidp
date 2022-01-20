namespace PidpTests.UnitTests;

using NodaTime;
using Xunit;

using Pidp.Models;

public class AuditTests : InMemoryDbTest
{
    [Fact]
    public async void TestAudits_Creation()
    {
        var party = new Party();
        TestDb.Parties.Add(party);
        Assert.Equal(default(Instant), party.Created);
        Assert.Equal(default(Instant), party.Modified);

        await TestDb.SaveChangesAsync();

        Assert.NotEqual(default(Instant), party.Created);
        Assert.NotEqual(default(Instant), party.Modified);
    }

    [Fact]
    public async void TestAudits_Update()
    {
        var party = TestDb.Has(new Party());
        var initialModified = party.Modified;

        party.FirstName = "Name";
        await TestDb.SaveChangesAsync();

        Assert.True(party.Modified > initialModified);
    }

    [Fact]
    public async void TestAudits_ImmutableCreated()
    {
        var party = TestDb.Has(new Party());
        var initialCreated = party.Created;

        party.Created = party.Created.PlusTicks(1000);
        await TestDb.SaveChangesAsync();

        Assert.Equal(initialCreated, party.Created);
    }
}
