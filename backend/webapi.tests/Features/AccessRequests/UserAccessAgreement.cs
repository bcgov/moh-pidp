namespace PidpTests.Features.AccessRequests;

using Xunit;

using static Pidp.Features.AccessRequests.UserAccessAgreement;
using PidpTests.TestingExtensions;
using Pidp.Models.Lookups;
using Pidp.Models;
using NodaTime;

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
            PartyId = party.Id,
        };
        var handler = this.MockDependenciesFor<CommandHandler>();

        var result = await handler.HandleAsync(command);

        Assert.True(result.IsSuccess);
        Assert.Single(this.TestDb.AccessRequests
            .Where(request => request.PartyId == party.Id
                && request.AccessTypeCode == AccessTypeCode.UserAccessAgreement));
    }

    [Fact]
    public async void AcceptUserAccessAgreement_HasAlreadyAccepted()
    {
        var party = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "partyfirst";
            party.LastName = "partylast";
        });

        this.TestDb.Has(new AccessRequest
        {
            AccessTypeCode = AccessTypeCode.UserAccessAgreement,
            PartyId = party.Id,
            RequestedOn = Instant.FromDateTimeOffset(DateTimeOffset.Now)
        });

        var command = new Command
        {
            PartyId = party.Id,
        };
        var handler = this.MockDependenciesFor<CommandHandler>();

        var result = await handler.HandleAsync(command);

        Assert.False(result.IsSuccess);
        var accessRequest = this.TestDb.AccessRequests
            .SingleOrDefault(p => p.PartyId == party.Id
                && p.AccessTypeCode == AccessTypeCode.UserAccessAgreement);
        Assert.NotNull(accessRequest);
    }
}
