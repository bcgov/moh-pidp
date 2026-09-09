namespace PidpTests.Features.Parties;

using FakeItEasy;
using NodaTime;
using Xunit;

using Pidp.Features.AccessRequests;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.Lookups;
using PidpTests.TestingExtensions;
using static Pidp.Features.Parties.ProfileStatus;
using static Pidp.Features.Parties.ProfileStatus.Model;

/// <summary>
/// The dashboard section and HcimWebPcr.IsEligible have to agree: the section is what tells a Party they
/// may request the card, and the command handler is what decides whether they actually get it. A section
/// that says AVAILABLE where the handler would fail leaves a button that only ever produces an error.
/// </summary>
public class ProfileStatusHcimWebPcrTests : ProfileStatusTest
{
    [Fact]
    public async Task HandleAsync_NoProfile_Locked()
    {
        var party = this.TestDb.Has(AParty.WithNoProfile(IdentityProviders.BCServicesCard));
        var handler = this.HandlerFor(PlrStandingsDigest.FromEmpty());

        var profile = await handler.HandleAsync(new Query { Id = party.Id, User = AMock.BcscUser() });

        var section = profile.Section<HcimWebPcrSection>();
        section.AssertNoAlerts();
        Assert.Equal(StatusCode.Locked, section.StatusCode);
    }

    [Theory]
    [MemberData(nameof(IdentifierTypeTestData))]
    public async Task HandleAsync_BcscLicenceDeclaredWithGoodStanding_IncompleteOnlyForAllowedColleges(IdentifierType identifierType, bool eligible)
    {
        // Good standing is not enough on its own: the card is limited to the College of Physicians and
        // Surgeons of BC and the BC College of Nurses and Midwives.
        var party = this.TestDb.Has(AParty.WithLicenceDeclared());
        var handler = this.HandlerFor(AMock.StandingsDigest(true, identifierType));

        var profile = await handler.HandleAsync(new Query { Id = party.Id, User = AMock.BcscUser() });

        var section = profile.Section<HcimWebPcrSection>();
        section.AssertNoAlerts();
        Assert.Equal(eligible ? StatusCode.Incomplete : StatusCode.Locked, section.StatusCode);
    }

    public static TheoryData<IdentifierType, bool> IdentifierTypeTestData()
    {
        var testData = new TheoryData<IdentifierType, bool>();

        foreach (var identifierType in TestData.AllIdentifierTypes)
        {
            testData.Add(identifierType, HcimWebPcr.AllowedIdentifierTypes.Contains(identifierType));
        }

        return testData;
    }

    [Fact]
    public async Task HandleAsync_MoaEndorsedByAllowedCollege_Incomplete()
    {
        // An MOA holds no licence of their own, so the section has to consult the endorsement digest or
        // they would never be offered the card at all.
        var party = this.TestDb.Has(AParty.WithLicenceDeclared(cpn: null));
        var handler = this.HandlerFor(
            PlrStandingsDigest.FromEmpty(),
            AMock.StandingsDigest(true, IdentifierType.PhysiciansAndSurgeons));

        var profile = await handler.HandleAsync(new Query { Id = party.Id, User = AMock.BcscUser() });

        var section = profile.Section<HcimWebPcrSection>();
        section.AssertNoAlerts();
        Assert.Equal(StatusCode.Incomplete, section.StatusCode);
    }

    [Fact]
    public async Task HandleAsync_MoaEndorsedByIneligibleCollege_Locked()
    {
        var party = this.TestDb.Has(AParty.WithLicenceDeclared(cpn: null));
        var handler = this.HandlerFor(
            PlrStandingsDigest.FromEmpty(),
            AMock.StandingsDigest(true, IdentifierType.Pharmacist));

        var profile = await handler.HandleAsync(new Query { Id = party.Id, User = AMock.BcscUser() });

        Assert.Equal(StatusCode.Locked, profile.Section<HcimWebPcrSection>().StatusCode);
    }

    [Fact]
    public async Task HandleAsync_BcscLicenceDeclaredNotInGoodStanding_Locked()
    {
        var party = this.TestDb.Has(AParty.WithLicenceDeclared());
        var digest = AMock.StandingsDigest(false, IdentifierType.PhysiciansAndSurgeons);
        Assert.False(HcimWebPcr.IsEligible(digest));
        var handler = this.HandlerFor(digest);

        var profile = await handler.HandleAsync(new Query { Id = party.Id, User = AMock.BcscUser() });

        var section = profile.Section<HcimWebPcrSection>();
        section.AssertNoAlerts();
        Assert.Equal(StatusCode.Locked, section.StatusCode);
    }

    [Fact]
    public async Task HandleAsync_EnroledWithBCProviderCredential_Complete()
    {
        var party = this.TestDb.Has(AnEnroledParty());
        var handler = this.HandlerFor(AMock.StandingsDigest(true, IdentifierType.PhysiciansAndSurgeons));

        var profile = await handler.HandleAsync(new Query { Id = party.Id, User = AMock.BcscUser() });

        var section = profile.Section<HcimWebPcrSection>();
        section.AssertNoAlerts();
        Assert.Equal(StatusCode.Complete, section.StatusCode);
    }

    [Fact]
    public async Task HandleAsync_EnroledButNoBCProviderCredential_NotComplete()
    {
        // The role only ever lands on a BCProvider account, so holding the Access Request without one
        // is not a finished enrolment. The card must not report Complete and send them to log in.
        var enroled = AnEnroledParty();
        enroled.Credentials = [new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCServicesCard, IdpId = "idpId" }];
        var party = this.TestDb.Has(enroled);
        var handler = this.HandlerFor(AMock.StandingsDigest(true, IdentifierType.PhysiciansAndSurgeons));

        var profile = await handler.HandleAsync(new Query { Id = party.Id, User = AMock.BcscUser() });

        var section = profile.Section<HcimWebPcrSection>();
        Assert.NotEqual(StatusCode.Complete, section.StatusCode);
    }

    /// <summary>
    /// A holder as the grant leaves them: a BC Services Card credential to qualify, and the BCProvider
    /// credential the role was actually assigned to.
    /// </summary>
    private static Party AnEnroledParty()
    {
        var party = AParty.WithLicenceDeclared();
        party.Credentials.Add(new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCProvider, IdpId = "bcProviderIdpId" });
        party.AccessRequests.Add(new AccessRequest
        {
            AccessTypeCode = AccessTypeCode.HcimWebPcr,
            RequestedOn = Instant.FromUtc(2026, 1, 1, 0, 0)
        });
        return party;
    }

    private QueryHandler HandlerFor(PlrStandingsDigest digest)
        => this.MockDependenciesFor<QueryHandler>(A.Fake<IPlrClient>().ReturningAStandingsDigest(digest));

    /// <summary>
    /// The two-digest form for MOA cases, where the Party's own standing and their endorsers' must differ.
    /// </summary>
    private QueryHandler HandlerFor(PlrStandingsDigest digest, PlrStandingsDigest aggregateDigest)
        => this.MockDependenciesFor<QueryHandler>(A.Fake<IPlrClient>().ReturningMultipleStandingsDigests(digest, aggregateDigest));
}
