namespace PidpTests.Features.Credentials;

using FakeItEasy;
using Microsoft.Graph.Models;
using NodaTime;
using Xunit;

using static Pidp.Features.Credentials.BCProviderCreate;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using PidpTests.TestingExtensions;

public class BcProviderCreateTests : InMemoryDbTest
{
    [Theory]
    [MemberData(nameof(LicenceTestCases))]
    public async void CreateBCProvider_VaryingLicenceStatus_ProviderCreatedMatchingFlags(PlrStandingsDigest plrStanding, bool expectedMd, bool expectedRnp)
    {
        var expectedPassword = "p4ssw@rD";
        var expectedHpdid = "AnHpdid1123123";
        var party = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "partyfirst";
            party.LastName = "partylast";
            party.Birthdate = LocalDate.FromDateTime(DateTime.Today);
            party.Email = "partyemail@testemail.com";
            party.Phone = "5555545555";
            party.Cpn = "aCpnn";
            party.Credentials.Add(new Credential
            {
                IdentityProvider = IdentityProviders.BCServicesCard,
                IdpId = expectedHpdid,
                UserId = Guid.NewGuid()
            });
        });
        NewUserRepresentation? capturedNewUser = null;
        var bcProviderClient = A.Fake<IBCProviderClient>();
        A.CallTo(() => bcProviderClient.CreateBCProviderAccount(A<NewUserRepresentation>._))
            .Invokes(i => capturedNewUser = i.GetArgument<NewUserRepresentation>(0))
            .Returns(new User());
        var plrClient = A.Fake<PlrClient>()
            .ReturningAStatandingsDigest(plrStanding);
        var handler = this.MockDependenciesFor<CommandHandler>(bcProviderClient, plrClient);

        var result = await handler.HandleAsync(new Command { PartyId = party.Id, Password = expectedPassword });

        Assert.True(result.IsSuccess);
        Assert.NotNull(capturedNewUser);
        Assert.Equal(party.Cpn, capturedNewUser.Cpn);
        Assert.Equal(party.FirstName, capturedNewUser.FirstName);
        Assert.Equal(party.LastName, capturedNewUser.LastName);
        Assert.Equal(expectedHpdid, capturedNewUser.Hpdid);
        Assert.Equal(expectedMd, capturedNewUser.IsMd);
        Assert.False(capturedNewUser.IsMoa);
        Assert.Equal(expectedRnp, capturedNewUser.IsRnp);
        Assert.Equal(party.Email, capturedNewUser.PidpEmail);
        Assert.Equal(expectedPassword, capturedNewUser.Password);
    }

    public static IEnumerable<object[]> LicenceTestCases()
    {
        yield return new object[] { PlrStandingsDigest.FromEmpty(), false, false };
        yield return new object[] { PlrStandingsDigest.FromError(), false, false };
        yield return new object[] { AMock.StandingsDigest(false, providerRoleType: ProviderRoleType.MedicalDoctor), false, false };
        yield return new object[] { AMock.StandingsDigest(false, providerRoleType: ProviderRoleType.RegisteredNursePractitioner), false, false };

        yield return new object[] { AMock.StandingsDigest(true, providerRoleType: ProviderRoleType.MedicalDoctor), true, false };
        yield return new object[] { AMock.StandingsDigest(true, providerRoleType: ProviderRoleType.RegisteredNursePractitioner), false, true };
        yield return new object[] { AMock.StandingsDigest((true, IdentifierType.PhysiciansAndSurgeons, ProviderRoleType.MedicalDoctor), (true, IdentifierType.Nurse, ProviderRoleType.RegisteredNursePractitioner)), true, true };
    }
}
