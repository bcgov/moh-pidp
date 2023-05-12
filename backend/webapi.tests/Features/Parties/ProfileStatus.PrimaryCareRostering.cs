namespace PidpTests.Features.Parties;

using FakeItEasy;
using Xunit;

using static Pidp.Features.Parties.ProfileStatus;
using static Pidp.Features.Parties.ProfileStatus.Model;
using Pidp.Infrastructure.HttpClients.Plr;
using PidpTests.TestingExtensions;

public class ProfileStatusPrimaryCareRosteringTests : ProfileStatusTest
{
    [Theory]
    [MemberData(nameof(PlrSuccessTestData))]
    public async void HandleAsync_RosteringProviderRoleAuthorized_Incomplete(PlrStandingsDigest digest)
    {
        var party = this.TestDb.Has(AParty.WithLicenceDeclared());
        var client = A.Fake<IPlrClient>()
            .ReturningAStatandingsDigest(digest);

        var handler = this.MockDependenciesFor<CommandHandler>(client);
        var profile = await handler.HandleAsync(new Command { Id = party.Id, User = AMock.BcscUser() });

        var rosteringSection = profile.Section<PrimaryCareRosteringSection>();
        rosteringSection.AssertNoAlerts();
        Assert.Equal(StatusCode.Incomplete, rosteringSection.StatusCode);
    }

    public static IEnumerable<object[]> PlrSuccessTestData()
    {
        yield return new object[] { AMock.StandingsDigest(true, IdentifierType.PhysiciansAndSurgeons, ProviderRoleType.MedicalDoctor) };
        yield return new object[] { AMock.StandingsDigest(true, IdentifierType.PhysiciansAndSurgeons, ProviderRoleType.RegisteredNursePractitioner) };
    }

    [Theory]
    [MemberData(nameof(PlrFailureTestData))]
    public async void HandleAsync_RosteringProviderRoleUnauthorized_Locked(PlrStandingsDigest digest)
    {
        var party = this.TestDb.Has(AParty.WithLicenceDeclared());
        var client = A.Fake<IPlrClient>()
            .ReturningAStatandingsDigest(digest);

        var handler = this.MockDependenciesFor<CommandHandler>(client);
        var profile = await handler.HandleAsync(new Command { Id = party.Id, User = AMock.BcscUser() });

        var rosteringSection = profile.Section<PrimaryCareRosteringSection>();
        rosteringSection.AssertNoAlerts();
        Assert.Equal(StatusCode.Locked, rosteringSection.StatusCode);
    }

    public static IEnumerable<object[]> PlrFailureTestData()
    {
        yield return new object[] { AMock.StandingsDigest(true, IdentifierType.PhysiciansAndSurgeons, "UnknowProviderRole") };
        yield return new object[] { AMock.StandingsDigest(true, IdentifierType.PhysiciansAndSurgeons, null) };
        yield return new object[] { AMock.StandingsDigest(false, IdentifierType.PhysiciansAndSurgeons, ProviderRoleType.MedicalDoctor) };
        yield return new object[] { AMock.StandingsDigest(false, IdentifierType.PhysiciansAndSurgeons, ProviderRoleType.RegisteredNursePractitioner) };
    }
}
