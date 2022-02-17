namespace PidpTests.Data;

using Xunit;

using Pidp.Models;
using PidpTests.TestingExtensions;

public class PidpDbContextAuditTests : InMemoryDbTest
{
    [Fact]
    public async void SaveChangesAsync_Creation_SetsAudits()
    {
        var party = new Party();
        this.TestDb.Parties.Add(party);
        Assert.Equal(default, party.Created);
        Assert.Equal(default, party.Modified);

        await this.TestDb.SaveChangesAsync();

        Assert.NotEqual(default, party.Created);
        Assert.NotEqual(default, party.Modified);
    }

    [Fact]
    public async void SaveChangesAsync_Update_UpdatesModified()
    {
        var party = this.TestDb.Has(new Party());
        var initialCreated = party.Created;
        var initialModified = party.Modified;

        party.FirstName = "Name";
        await this.TestDb.SaveChangesAsync();

        Assert.Equal(initialCreated, party.Created);
        Assert.True(party.Modified > initialModified);
    }

    [Fact]
    public async void SaveChangesAsync_ManualAuditUpdate_ImmutableCreated()
    {
        var party = this.TestDb.Has(new Party());
        var initialCreated = party.Created;

        party.Created = party.Created.PlusTicks(1000);
        await this.TestDb.SaveChangesAsync();

        Assert.Equal(initialCreated, party.Created);
    }
}
