namespace PidpTests.Features.Parties;

using FakeItEasy;
using NodaTime;
using Pidp.Features.Parties;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Models;
using PidpTests.TestingExtensions;
using Xunit;

public class DemographicsTests : InMemoryDbTest
{
    [Fact]
    public async void UpdatePartyDemographics_UpdateEmail_Success()
    {
        var party = this.TestDb.HasAParty(party =>
        {
            party.Id = 1;
            party.FirstName = "partyfirst";
            party.LastName = "partylast";
            party.Birthdate = LocalDate.FromDateTime(DateTime.Today);
            party.Email = "partyemail@testemail.com";
            party.Phone = "5555545555";
            party.Cpn = "aCpnn";
            party.Credentials.Add(new Credential
            {
                IdentityProvider = IdentityProviders.BCProvider,
                IdpId = "AnIdpId1123123",
                UserId = Guid.NewGuid()
            });
        });

        var command = new Demographics.Command
        {
            Email = "pidp.test@goh.com",
            Id = 1,
        };

        var bcProviderClient = A.Fake<IBCProviderClient>();

        var handler = this.MockDependenciesFor<Demographics.CommandHandler>(bcProviderClient);

        await handler.HandleAsync(command);

        var partyUpdated = this.TestDb.Parties.SingleOrDefault(request => request.Id == command.Id);
        Assert.NotNull(partyUpdated);
        Assert.Equal("pidp.test@goh.com", partyUpdated.Email);

        A.CallTo(() => bcProviderClient
            .UpdateAttributes(
                "AnIdpId1123123",
                A<Dictionary<string, object>>._
            )
        ).MustHaveHappenedOnceExactly();
    }
}
