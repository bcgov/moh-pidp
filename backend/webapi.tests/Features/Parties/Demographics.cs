namespace PidpTests.Features.Parties;

using Xunit;

using static Pidp.Features.Parties.Demographics;
using Pidp.Models.DomainEvents;
using PidpTests.TestingExtensions;

public class DemographicsTests : InMemoryDbTest
{
    [Fact]
    public async void UpdatePartyDemographics_NewParty_PropertiesUpdatedNoDomainEvent()
    {
        var party = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "partyfirst";
            party.LastName = "partylast";
        });
        var command = new Command
        {
            Id = party.Id,
            Email = "pidp.test@goh.com",
            Phone = "5555555555",
            PreferredFirstName = "PFirst",
            PreferredMiddleName = "PMid",
            PreferredLastName = "PLast"
        };
        var handler = this.MockDependenciesFor<CommandHandler>();

        await handler.HandleAsync(command);

        var updatedParty = this.TestDb.Parties.Single(p => p.Id == party.Id);
        Assert.Equal(command.Email, updatedParty.Email);
        Assert.Equal(command.Phone, updatedParty.Phone);
        Assert.Equal(command.PreferredFirstName, updatedParty.PreferredFirstName);
        Assert.Equal(command.PreferredMiddleName, updatedParty.PreferredMiddleName);
        Assert.Equal(command.PreferredLastName, updatedParty.PreferredLastName);

        Assert.Empty(this.PublishedEvents.OfType<PartyEmailUpdated>());
    }

    [Fact]
    public async void UpdatePartyDemographics_ExistingParty_PropertiesUpdatedDomainEvent()
    {
        var party = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "partyfirst";
            party.LastName = "partylast";
            party.Email = "existing@email.com";
            party.Phone = "5555555555";
            party.PreferredFirstName = "ExistingFirst";
            party.PreferredMiddleName = "ExistingMid";
            party.PreferredLastName = "ExistingLast";
        });
        var command = new Command
        {
            Id = party.Id,
            Email = "new@emzlez.com",
            Phone = "4444444444",
            PreferredFirstName = "PFirst",
            PreferredMiddleName = "PMid",
            PreferredLastName = "PLast"
        };
        var handler = this.MockDependenciesFor<CommandHandler>();

        await handler.HandleAsync(command);

        var updatedParty = this.TestDb.Parties.Single(p => p.Id == party.Id);
        Assert.Equal(command.Email, updatedParty.Email);
        Assert.Equal(command.Phone, updatedParty.Phone);
        Assert.Equal(command.PreferredFirstName, updatedParty.PreferredFirstName);
        Assert.Equal(command.PreferredMiddleName, updatedParty.PreferredMiddleName);
        Assert.Equal(command.PreferredLastName, updatedParty.PreferredLastName);

        Assert.Single(this.PublishedEvents.OfType<PartyEmailUpdated>());
        var emailEvent = this.PublishedEvents.OfType<PartyEmailUpdated>().Single();
        Assert.Equal(party.Id, emailEvent.PartyId);
        Assert.Equal(command.Email, emailEvent.NewEmail);
    }

    [Fact]
    public async void UpdatePartyDemographics_ExistingParty_TrimPreferredNames()
    {
        var party = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "John";
            party.LastName = "doe";
            party.Email = "john.doe@email.com";
            party.Phone = "5555555555";
            party.PreferredFirstName = "Jo";
            party.PreferredMiddleName = "jr.";
            party.PreferredLastName = "Downey";
        });
        var command = new Command
        {
            Id = party.Id,
            PreferredFirstName = " Jean Charles  ",
            PreferredMiddleName = " jr.  ",
            PreferredLastName = " Downey "
        };
        var handler = this.MockDependenciesFor<CommandHandler>();

        await handler.HandleAsync(command);

        var updatedParty = this.TestDb.Parties.Single(p => p.Id == party.Id);
        Assert.Equal("Jean Charles", updatedParty.PreferredFirstName);
        Assert.Equal("jr.", updatedParty.PreferredMiddleName);
        Assert.Equal("Downey", updatedParty.PreferredLastName);

        Assert.Single(this.PublishedEvents.OfType<PartyEmailUpdated>());
        var emailEvent = this.PublishedEvents.OfType<PartyEmailUpdated>().Single();
        Assert.Equal(party.Id, emailEvent.PartyId);
        Assert.Equal(command.Email, emailEvent.NewEmail);
    }

    [Fact]
    public async void UpdatePartyDemographics_ExistingPartySameEmail_NoDomainEvent()
    {
        var party = this.TestDb.HasAParty(party => party.Email = "existing@email.com");
        var command = new Command
        {
            Id = party.Id,
            Email = party.Email
        };
        var handler = this.MockDependenciesFor<CommandHandler>();

        await handler.HandleAsync(command);

        Assert.Empty(this.PublishedEvents.OfType<PartyEmailUpdated>());
    }
}
