namespace PidpTests.Features.Parties;

using FakeItEasy;
using MassTransit;
using Xunit;

using static Pidp.Features.Parties.Demographics;
using Pidp.Models.DomainEvents;
using PidpTests.TestingExtensions;

public class DemographicsTests : InMemoryDbTest
{
    [Fact]
    public async void UpdatePartyDemographics_NewParty_NoMessagePublished()
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
        var bus = A.Fake<IBus>();

        var handler = this.MockDependenciesFor<CommandHandler>(bus);

        await handler.HandleAsync(command);

        var updatedParty = this.TestDb.Parties.Single(p => p.Id == party.Id);
        Assert.Equal(command.Email, updatedParty.Email);
        Assert.Equal(command.Phone, updatedParty.Phone);
        Assert.Equal(command.PreferredFirstName, updatedParty.PreferredFirstName);
        Assert.Equal(command.PreferredMiddleName, updatedParty.PreferredMiddleName);
        Assert.Equal(command.PreferredLastName, updatedParty.PreferredLastName);

        A.CallTo(() => bus.Publish(new PartyEmailUpdated(party.Id, party.PrimaryUserId, command.Email!), CancellationToken.None)).MustNotHaveHappened();
    }

    [Fact]
    public async void UpdatePartyDemographics_ExistingParty_MessagePublished()
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
        var bus = A.Fake<IBus>();

        var handler = this.MockDependenciesFor<CommandHandler>(bus);

        await handler.HandleAsync(command);

        var updatedParty = this.TestDb.Parties.Single(p => p.Id == party.Id);
        Assert.Equal(command.Email, updatedParty.Email);
        Assert.Equal(command.Phone, updatedParty.Phone);
        Assert.Equal(command.PreferredFirstName, updatedParty.PreferredFirstName);
        Assert.Equal(command.PreferredMiddleName, updatedParty.PreferredMiddleName);
        Assert.Equal(command.PreferredLastName, updatedParty.PreferredLastName);

        A.CallTo(() => bus.Publish(new PartyEmailUpdated(party.Id, party.PrimaryUserId, command.Email), CancellationToken.None)).MustHaveHappened();
    }

    [Fact]
    public async void UpdatePartyDemographics_ExistingPartySameEmail_NoMessagePublished()
    {
        var party = this.TestDb.HasAParty(party => party.Email = "existing@email.com");
        var command = new Command
        {
            Id = party.Id,
            Email = party.Email
        };
        var bus = A.Fake<IBus>();

        var handler = this.MockDependenciesFor<CommandHandler>(bus);

        await handler.HandleAsync(command);

        A.CallTo(() => bus.Publish(new PartyEmailUpdated(party.Id, party.PrimaryUserId, command.Email!), CancellationToken.None)).MustNotHaveHappened();
    }
}
