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

public class HcimWebPcrRevocationTests : InMemoryDbTest
{
    [Fact]
    public async Task RevokeIfIneligible_NotEnroled_NoOp()
    {
        // The cheap guard: a Party who never held the card must cost nothing beyond one query.
        // Most PLR status changes are for these Parties.
        var party = this.TestDb.HasAParty(party =>
        {
            party.Cpn = "Cpn";
            party.Credentials = [new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCProvider }];
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

        var decision = await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        Assert.Equal(RevocationOutcome.StandingUnknown, decision.Outcome);
        keycloak.AssertNoRolesRemoved();
        Assert.Contains(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        Assert.Empty(this.TestDb.BusinessEvents);
    }

    [Fact]
    public async Task RevokeIfIneligible_StillInGoodStanding_NoOp()
    {
        var party = this.HasAnEnroledParty();
        var (_, keycloak, service) = this.SetupFor(AMock.StandingsDigest(true, IdentifierType.PhysiciansAndSurgeons));

        var decision = await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        Assert.Equal(RevocationOutcome.Eligible, decision.Outcome);
        keycloak.AssertNoRolesRemoved();
        Assert.Contains(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        Assert.Empty(this.TestDb.BusinessEvents);
    }

    [Fact]
    public async Task RevokeIfIneligible_MoaWithGoodEndorsement_NoOp()
    {
        // An MOA has no CPN of their own and is judged on their endorsers. Their own (empty) digest must
        // not be what decides it, or every MOA would be stripped on the first re-evaluation.
        var party = this.HasAnEnroledParty(party => party.Cpn = null);
        var (plr, keycloak, service) = this.SetupFor(
            PlrStandingsDigest.FromEmpty(),
            AMock.StandingsDigest(true, IdentifierType.PhysiciansAndSurgeons));

        var decision = await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        Assert.Equal(RevocationOutcome.Eligible, decision.Outcome);
        keycloak.AssertNoRolesRemoved();
        Assert.Contains(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        // The endorsement digest is the one that matters, and the Party's own is never consulted.
        A.CallTo(() => plr.GetAggregateStandingsDigestAsync(A<IEnumerable<string?>>._)).MustHaveHappened();
        A.CallTo(() => plr.GetStandingsDigestAsync(A<string?>._)).MustNotHaveHappened();
    }

    [Fact]
    public async Task RevokeIfIneligible_MoaWithNoGoodEndorsement_Revokes()
    {
        var party = this.HasAnEnroledParty(party => party.Cpn = null);
        var (_, keycloak, service) = this.SetupFor(
            PlrStandingsDigest.FromEmpty(),
            AMock.StandingsDigest(false, IdentifierType.PhysiciansAndSurgeons));

        var decision = await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        A.CallTo(() => keycloak.RemoveAccessRoles(A<Guid>._, MohKeycloakEnrolment.HcimWebPcr)).MustHaveHappened();
        Assert.DoesNotContain(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        // The reason has to name the endorsement. Reporting an MOA as a lapsed licence would send a
        // reviewer looking in PLR for a licence the Party never had.
        Assert.Equal(RevocationOutcome.Revoked, decision.Outcome);
        Assert.Contains("endorsement", decision.Reason, StringComparison.Ordinal);
    }

    [Fact]
    public async Task RevokeIfIneligible_MoaEndorsedByIneligibleCollege_Revokes()
    {
        // An endorser in good standing is not enough: they have to be registered with one of the colleges
        // the card allows. A pharmacist endorser does not carry an MOA onto the Client Registry.
        var party = this.HasAnEnroledParty(party => party.Cpn = null);
        var (_, keycloak, service) = this.SetupFor(
            PlrStandingsDigest.FromEmpty(),
            AMock.StandingsDigest(true, IdentifierType.Pharmacist));

        var decision = await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        Assert.Equal(RevocationOutcome.Revoked, decision.Outcome);
        A.CallTo(() => keycloak.RemoveAccessRoles(A<Guid>._, MohKeycloakEnrolment.HcimWebPcr)).MustHaveHappened();
    }

    [Fact]
    public async Task RevokeIfIneligible_MoaEndorsementPlrError_DoesNotRevoke()
    {
        // Fail closed on the endorsement digest too, not just the Party's own.
        var party = this.HasAnEnroledParty(party => party.Cpn = null);
        var (_, keycloak, service) = this.SetupFor(PlrStandingsDigest.FromEmpty(), PlrStandingsDigest.FromError());

        var decision = await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        Assert.Equal(RevocationOutcome.StandingUnknown, decision.Outcome);
        keycloak.AssertNoRolesRemoved();
        Assert.Contains(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
    }

    [Fact]
    public async Task RevokeIfIneligible_NoLongerEligible_RemovesRoleAndAccessRequest()
    {
        var party = this.HasAnEnroledParty();
        var (_, keycloak, service) = this.SetupFor(AMock.StandingsDigest(false, IdentifierType.PhysiciansAndSurgeons));

        var decision = await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        Assert.Equal(RevocationOutcome.Revoked, decision.Outcome);
        A.CallTo(() => keycloak.RemoveAccessRoles(party.Credentials.Single().UserId, MohKeycloakEnrolment.HcimWebPcr)).MustHaveHappened();
        Assert.DoesNotContain(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        Assert.Contains(this.TestDb.BusinessEvents, businessEvent => businessEvent is AccessRequestRevoked
            && businessEvent.Severity == LogLevel.Information);
    }

    [Fact]
    public async Task RevokeIfIneligible_RemovesFromEveryCredentialNotJustBCProvider()
    {
        // Assign narrowly, remove broadly. The grant only ever targets the BCProvider credential, but a
        // role can reach a BC Services Card account by other routes - the CredentialLinked handler
        // replays every held Access Type onto a newly linked credential. Removing from all of them is
        // what cleans that up, so it is pinned here rather than left to the shared service's own tests.
        var bcProviderUserId = Guid.NewGuid();
        var bcscUserId = Guid.NewGuid();
        var party = this.HasAnEnroledParty(party => party.Credentials =
        [
            new Credential { UserId = bcProviderUserId, IdentityProvider = IdentityProviders.BCProvider },
            new Credential { UserId = bcscUserId, IdentityProvider = IdentityProviders.BCServicesCard }
        ]);
        var (_, keycloak, service) = this.SetupFor(AMock.StandingsDigest(false, IdentifierType.PhysiciansAndSurgeons));

        await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        A.CallTo(() => keycloak.RemoveAccessRoles(bcProviderUserId, MohKeycloakEnrolment.HcimWebPcr)).MustHaveHappened();
        A.CallTo(() => keycloak.RemoveAccessRoles(bcscUserId, MohKeycloakEnrolment.HcimWebPcr)).MustHaveHappened();
        Assert.DoesNotContain(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
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
        var service = this.MockDependenciesFor<HcimWebPcrRevocationPolicy>(plr, keycloak, revocationService);

        var decision = await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        Assert.Equal(RevocationOutcome.RevokeFailed, decision.Outcome);
        Assert.Contains(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        Assert.Contains(this.TestDb.BusinessEvents, businessEvent => businessEvent is AccessRequestRevoked
            && businessEvent.Severity == LogLevel.Error);
    }

    [Fact]
    public async Task RevokeIfIneligible_CpsPostgrad_Revokes()
    {
        // Deliberately unlike SA and RSV eForms: the Provincial Client Registry has no IsCpsPostgrad
        // clause, so a resident in PENDING/NONPRAC loses it. Pinned so that stays a decision rather
        // than something a future copy-paste from another card quietly changes.
        var party = this.HasAnEnroledParty();
        var digest = PlrStandingsDigest.FromRecords([
            new PlrRecord
            {
                IdentifierType = IdentifierType.PhysiciansAndSurgeons,
                StatusCode = PlrStatusCode.Pending,
                StatusReasonCode = PlrStatusReasonCode.NonPracticing
            }
        ]);
        Assert.True(digest.IsCpsPostgrad);
        var (_, keycloak, service) = this.SetupFor(digest);

        await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        A.CallTo(() => keycloak.RemoveAccessRoles(A<Guid>._, MohKeycloakEnrolment.HcimWebPcr)).MustHaveHappened();
        Assert.DoesNotContain(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
    }

    [Theory]
    [MemberData(nameof(IdentifierTypeTestData))]
    public async Task RevokeIfIneligible_VaryingLicence_RevokesOutsideTheAllowedColleges(IdentifierType identifierType, bool stillEligible)
    {
        // Good standing alone no longer keeps the card: it has to be with one of the allowed colleges.
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
            A.CallTo(() => keycloak.RemoveAccessRoles(A<Guid>._, MohKeycloakEnrolment.HcimWebPcr)).MustHaveHappened();
            Assert.DoesNotContain(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        }
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
    public async Task RevokeIfIneligible_CalledTwice_IsIdempotent()
    {
        var party = this.HasAnEnroledParty();
        var (_, keycloak, service) = this.SetupFor(AMock.StandingsDigest(false));

        await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();
        await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        A.CallTo(() => keycloak.RemoveAccessRoles(A<Guid>._, MohKeycloakEnrolment.HcimWebPcr)).MustHaveHappenedOnceExactly();
        Assert.Single(this.TestDb.BusinessEvents, businessEvent => businessEvent is AccessRequestRevoked);
    }

    /// <summary>
    /// The grant targets the BCProvider credential, so that is what a holder looks like here.
    /// </summary>
    private Party HasAnEnroledParty(Action<Party>? config = null) => this.TestDb.HasAParty(party =>
    {
        party.Cpn = "Cpn";
        party.Credentials = [new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCProvider }];
        party.AccessRequests = [new AccessRequest { AccessTypeCode = AccessTypeCode.HcimWebPcr, RequestedOn = Instant.FromUtc(2026, 1, 1, 0, 0) }];
        config?.Invoke(party);
    });

    /// <summary>
    /// Builds the policy over a REAL AccessRequestRevocationService, so these tests continue to
    /// exercise the eligibility rules and the removal mechanics together. Faking the service would
    /// leave every assertion about deleted Access Requests and Business Events vacuous.
    /// </summary>
    private (IPlrClient Plr, IKeycloakAdministrationClient Keycloak, HcimWebPcrRevocationPolicy Policy) SetupFor(PlrStandingsDigest digest)
        => this.SetupFor(digest, digest);

    /// <summary>
    /// The two-digest form for MOA cases, where the Party's own standing and their endorsers' must differ.
    /// ReturningAStandingsDigest arms both PLR calls with the same digest, which cannot express that.
    /// </summary>
    private (IPlrClient Plr, IKeycloakAdministrationClient Keycloak, HcimWebPcrRevocationPolicy Policy) SetupFor(PlrStandingsDigest digest, PlrStandingsDigest aggregateDigest)
    {
        var plr = A.Fake<IPlrClient>().ReturningMultipleStandingsDigests(digest, aggregateDigest);
        var keycloak = A.Fake<IKeycloakAdministrationClient>().ReturningTrueWhenRemovingClientRoles();
        var revocationService = this.MockDependenciesFor<AccessRequestRevocationService>(keycloak);

        return (plr, keycloak, this.MockDependenciesFor<HcimWebPcrRevocationPolicy>(plr, keycloak, revocationService));
    }
}
