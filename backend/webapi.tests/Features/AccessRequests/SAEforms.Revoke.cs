namespace PidpTests.Features.AccessRequests;

using FakeItEasy;
using Microsoft.Extensions.Logging;
using NodaTime;
using Xunit;

using Pidp.Features.AccessRequests;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Infrastructure.Services;
using Pidp.Models;
using Pidp.Models.Lookups;
using PidpTests.TestingExtensions;

public class SAEformsRevocationTests : InMemoryDbTest
{
    [Fact]
    public async Task RevokeIfIneligible_NotEnroled_NoOp()
    {
        // The cheap guard: a Party who never held the card must cost nothing beyond one query.
        // Most PLR status changes are for these Parties.
        var party = this.TestDb.HasAParty(party =>
        {
            party.Cpn = "Cpn";
            party.Credentials = [new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCServicesCard }];
        });
        var (plr, keycloak, service) = this.SetupFor(AMock.StandingsDigest(false));

        await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        keycloak.AssertNoRolesRemoved();
        A.CallTo(() => plr.GetStandingsDigestAsync(A<string?>._)).MustNotHaveHappened();
        Assert.Empty(this.TestDb.BusinessEvents);
    }

    [Fact]
    public async Task RevokeIfIneligible_PlrError_DoesNotRevoke()
    {
        // An unreachable PLR yields a digest with no records, which is indistinguishable from
        // "not in good standing". Revoking on that would strip every holder during an outage.
        var party = this.HasAnEnroledParty();
        var digest = PlrStandingsDigest.FromError();
        Assert.False(digest.HasGoodStanding);
        var (_, keycloak, service) = this.SetupFor(digest);

        await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        keycloak.AssertNoRolesRemoved();
        Assert.Contains(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        Assert.Empty(this.TestDb.BusinessEvents);
    }

    [Fact]
    public async Task RevokeIfIneligible_StillInGoodStanding_NoOp()
    {
        var party = this.HasAnEnroledParty();
        var (_, keycloak, service) = this.SetupFor(AMock.StandingsDigest(true, IdentifierType.PhysiciansAndSurgeons));

        await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        keycloak.AssertNoRolesRemoved();
        Assert.Contains(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        Assert.Empty(this.TestDb.BusinessEvents);
    }

    [Fact]
    public async Task RevokeIfIneligible_CpsPostgrad_NoOp()
    {
        // Residents are PENDING/NONPRAC - never "good standing" - and hold the card via
        // IsCpsPostgrad. Revoking on a bare good-standing check would strip them all.
        var party = this.HasAnEnroledParty();
        var digest = PlrStandingsDigest.FromRecords([
            new PlrRecord
            {
                IdentifierType = IdentifierType.PhysiciansAndSurgeons,
                StatusCode = PlrStatusCode.Pending,
                StatusReasonCode = PlrStatusReasonCode.NonPracticing
            }
        ]);
        Assert.False(digest.HasGoodStanding);
        var (_, keycloak, service) = this.SetupFor(digest);

        await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        keycloak.AssertNoRolesRemoved();
        Assert.Contains(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
    }

    [Fact]
    public async Task RevokeIfIneligible_NoCpn_Revokes()
    {
        // Special Authority is the one eForm card with no endorsement path, so there is nothing for a
        // Party without a CPN to qualify on. An empty digest reports Error == false, so this is a
        // known ineligible state and must not be mistaken for the PLR-outage case above.
        var party = this.HasAnEnroledParty(party => party.Cpn = null);
        var (plr, keycloak, service) = this.SetupFor(PlrStandingsDigest.FromEmpty());

        var decision = await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        A.CallTo(() => keycloak.RemoveAccessRoles(A<Guid>._, MohKeycloakEnrolment.SAEforms)).MustHaveHappened();
        Assert.DoesNotContain(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        // No endorsement path means the endorsement digest is never consulted for this card.
        A.CallTo(() => plr.GetAggregateStandingsDigestAsync(A<IEnumerable<string?>>._)).MustNotHaveHappened();
        // The reason has to name the missing CPN. Reporting these as a lapsed licence would send a
        // reviewer looking at PLR for an answer that is not there: this population is data drift.
        Assert.Equal(RevocationOutcome.Revoked, decision.Outcome);
        Assert.Contains("no CPN", decision.Reason, StringComparison.Ordinal);
        Assert.DoesNotContain("good standing", decision.Reason, StringComparison.Ordinal);
    }

    [Fact]
    public async Task RevokeIfIneligible_NoLongerEligible_RemovesRoleAndAccessRequest()
    {
        var party = this.HasAnEnroledParty();
        var (_, keycloak, service) = this.SetupFor(AMock.StandingsDigest(false, IdentifierType.PhysiciansAndSurgeons));

        await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        A.CallTo(() => keycloak.RemoveAccessRoles(party.Credentials.Single().UserId, MohKeycloakEnrolment.SAEforms)).MustHaveHappened();
        Assert.DoesNotContain(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        Assert.Contains(this.TestDb.BusinessEvents, businessEvent => businessEvent is AccessRequestRevoked
            && businessEvent.Severity == LogLevel.Information);
    }

    [Fact]
    public async Task RevokeIfIneligible_KeycloakFails_LeavesAccessRequestToBeRetried()
    {
        // Deleting the row while the role is still live would strand access with no record of it,
        // so nothing would ever retry.
        var party = this.HasAnEnroledParty();
        var plr = A.Fake<IPlrClient>().ReturningAStandingsDigest(AMock.StandingsDigest(false));
        var keycloak = A.Fake<IKeycloakAdministrationClient>();
        A.CallTo(() => keycloak.RemoveAccessRoles(A<Guid>._, A<MohKeycloakEnrolment>._)).Returns(false);
        var revocationService = this.MockDependenciesFor<AccessRequestRevocationService>(keycloak);
        var service = this.MockDependenciesFor<SAEformsRevocationPolicy>(plr, keycloak, revocationService);

        await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        Assert.Contains(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        Assert.Contains(this.TestDb.BusinessEvents, businessEvent => businessEvent is AccessRequestRevoked
            && businessEvent.Severity == LogLevel.Error);
    }

    [Theory]
    [MemberData(nameof(IdentifierTypeTestData))]
    public async Task RevokeIfIneligible_VaryingLicence_RevokesOnlyExcludedTypes(IdentifierType identifierType, bool stillEligible)
    {
        // SAEforms is the inverse of the other cards: it excludes PharmacyTech rather than allowing a
        // named list, so every other college keeps the card while in good standing.
        var party = this.HasAnEnroledParty();
        var (_, keycloak, service) = this.SetupFor(AMock.StandingsDigest(true, identifierType));

        await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        if (stillEligible)
        {
            keycloak.AssertNoRolesRemoved();
            Assert.Contains(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        }
        else
        {
            A.CallTo(() => keycloak.RemoveAccessRoles(A<Guid>._, MohKeycloakEnrolment.SAEforms)).MustHaveHappened();
            Assert.DoesNotContain(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        }
    }

    public static TheoryData<IdentifierType, bool> IdentifierTypeTestData()
    {
        var testData = new TheoryData<IdentifierType, bool>();

        foreach (var identifierType in TestData.AllIdentifierTypes)
        {
            testData.Add(identifierType, !SAEforms.ExcludedIdentifierTypes.Contains(identifierType));
        }

        return testData;
    }

    [Fact]
    public async Task RevokeIfIneligible_CalledTwice_IsIdempotent()
    {
        var party = this.HasAnEnroledParty();
        var (_, keycloak, service) = this.SetupFor(AMock.StandingsDigest(false));

        await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();
        await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        A.CallTo(() => keycloak.RemoveAccessRoles(A<Guid>._, MohKeycloakEnrolment.SAEforms)).MustHaveHappenedOnceExactly();
        Assert.Single(this.TestDb.BusinessEvents, businessEvent => businessEvent is AccessRequestRevoked);
    }

    private Party HasAnEnroledParty(Action<Party>? config = null) => this.TestDb.HasAParty(party =>
    {
        party.Cpn = "Cpn";
        party.Credentials = [new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCServicesCard }];
        party.AccessRequests = [new AccessRequest { AccessTypeCode = AccessTypeCode.SAEforms, RequestedOn = Instant.FromUtc(2026, 1, 1, 0, 0) }];
        config?.Invoke(party);
    });

    /// <summary>
    /// Builds the policy over a REAL AccessRequestRevocationService, so these tests continue to
    /// exercise the eligibility rules and the removal mechanics together. Faking the service would
    /// leave every assertion about deleted Access Requests and Business Events vacuous.
    /// </summary>
    private (IPlrClient Plr, IKeycloakAdministrationClient Keycloak, SAEformsRevocationPolicy Policy) SetupFor(PlrStandingsDigest digest)
    {
        var plr = A.Fake<IPlrClient>().ReturningAStandingsDigest(digest);
        var keycloak = A.Fake<IKeycloakAdministrationClient>().ReturningTrueWhenRemovingClientRoles();
        var revocationService = this.MockDependenciesFor<AccessRequestRevocationService>(keycloak);

        return (plr, keycloak, this.MockDependenciesFor<SAEformsRevocationPolicy>(plr, keycloak, revocationService));
    }
}
