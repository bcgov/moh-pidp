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
    public async void HandleAsync_RosteringProviderRoleAuthorized_Incomplete(PlrStandingsDigest digest, PlrStandingsDigest aggregateDigest)
    {
        var party = this.TestDb.Has(AParty.WithLicenceDeclared());
        var client = A.Fake<IPlrClient>()
            .ReturningMultipleStatandingsDigests(digest, aggregateDigest);
        var handler = this.MockDependenciesFor<CommandHandler>(client);

        var profile = await handler.HandleAsync(new Command { Id = party.Id, User = AMock.BcscUser() });

        var rosteringSection = profile.Section<PrimaryCareRosteringSection>();
        rosteringSection.AssertNoAlerts();
        Assert.Equal(StatusCode.Incomplete, rosteringSection.StatusCode);
    }

    public static IEnumerable<object[]> PlrSuccessTestData()
    {
        yield return new object[] { AMock.StandingsDigest(true, providerRoleType: ProviderRoleType.MedicalDoctor), AMock.StandingsDigest(false) };
        yield return new object[] { AMock.StandingsDigest(true, providerRoleType: ProviderRoleType.RegisteredNursePractitioner), AMock.StandingsDigest(false) };
        yield return new object[] { AMock.StandingsDigest(true), AMock.StandingsDigest(true) };
        yield return new object[] { AMock.StandingsDigest(false), AMock.StandingsDigest(true) };
        yield return new object[] { PlrStandingsDigest.FromEmpty(), AMock.StandingsDigest(true) };
    }

    [Theory]
    [MemberData(nameof(PlrFailureTestData))]
    public async void HandleAsync_RosteringProviderRoleUnauthorized_Locked(PlrStandingsDigest digest, PlrStandingsDigest aggregateDigest)
    {
        var party = this.TestDb.Has(AParty.WithLicenceDeclared());
        var client = A.Fake<IPlrClient>()
            .ReturningMultipleStatandingsDigests(digest, aggregateDigest);
        var handler = this.MockDependenciesFor<CommandHandler>(client);

        var profile = await handler.HandleAsync(new Command { Id = party.Id, User = AMock.BcscUser() });

        var rosteringSection = profile.Section<PrimaryCareRosteringSection>();
        rosteringSection.AssertNoAlerts();
        Assert.Equal(StatusCode.Locked, rosteringSection.StatusCode);
    }

    public static IEnumerable<object[]> PlrFailureTestData()
    {
        yield return new object[] { AMock.StandingsDigest(true, providerRoleType: "UnknownProviderRole"), AMock.StandingsDigest(false) };
        yield return new object[] { AMock.StandingsDigest(true, providerRoleType: null), AMock.StandingsDigest(false) };
        yield return new object[] { AMock.StandingsDigest(false, providerRoleType: ProviderRoleType.MedicalDoctor), AMock.StandingsDigest(false) };
        yield return new object[] { AMock.StandingsDigest(false, providerRoleType: ProviderRoleType.RegisteredNursePractitioner), AMock.StandingsDigest(false) };
    }
}
