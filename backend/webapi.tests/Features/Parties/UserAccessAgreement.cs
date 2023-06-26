namespace PidpTests.Features.Parties;

using Xunit;

using static Pidp.Features.Parties.UserAccessAgreement;
using PidpTests.TestingExtensions;

public class UserAccessAgreementTests : InMemoryDbTest
{
    [Fact]
    public async void AcceptUserAccessAgreement_Accepted()
    {
        var party = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "partyfirst";
            party.LastName = "partylast";
        });
        var command = new Command
        {
            Id = party.Id,
        };
        var handler = this.MockDependenciesFor<CommandHandler>();

        await handler.HandleAsync(command);

        var updatedParty = this.TestDb.Parties.Single(p => p.Id == party.Id);
        Assert.NotNull(updatedParty.UserAccessAgreementDate);
    }

    [Fact]
    public async void AcceptUserAccessAgreement_HasAlreadyAccepted()
    {
        var party = this.TestDb.Has(AParty.WithUserAccessAgreementAccepted());
        var command = new Command
        {
            Id = party.Id,
        };
        var handler = this.MockDependenciesFor<CommandHandler>();

        await handler.HandleAsync(command);

        var updatedParty = this.TestDb.Parties.Single(p => p.Id == party.Id);
        Assert.NotNull(updatedParty.UserAccessAgreementDate);
    }
}
