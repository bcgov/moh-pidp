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

public class ImmsBCEformsRevocationTests : InMemoryDbTest
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
        A.CallTo(() => plr.GetAggregateStandingsDigestAsync(A<IEnumerable<string?>>._)).MustNotHaveHappened();
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
    public async Task RevokeIfIneligible_CpsPostgrad_Revokes()
    {
        // Pins today's accepted behaviour, which differs from every other eForm card: ImmsBCEforms
        // .IsEligible has no IsCpsPostgrad clause, so a resident in PENDING/NONPRAC loses this card.
        // If the eligibility table is ever reconciled, this test must be edited deliberately rather
        // than silently drifting.
        var party = this.HasAnEnroledParty();
        var digest = PlrStandingsDigest.FromRecords([
            new PlrRecord
            {
                IdentifierType = IdentifierType.PhysiciansAndSurgeons,
                StatusCode = PlrStatusCode.Pending,
                StatusReasonCode = PlrStatusReasonCode.NonPracticing
            }
        ]);
        Assert.True(digest.IsCpsPostgrad, "This test is meaningless unless the digest is a CPS postgrad.");
        var (_, keycloak, service) = this.SetupFor(digest);

        await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        A.CallTo(() => keycloak.RemoveAccessRoles(A<Guid>._, MohKeycloakEnrolment.ImmsBCEforms)).MustHaveHappened();
        Assert.DoesNotContain(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
    }

    [Fact]
    public async Task RevokeIfIneligible_NoLongerEligible_RemovesRoleAndAccessRequest()
    {
        var party = this.HasAnEnroledParty();
        var (_, keycloak, service) = this.SetupFor(AMock.StandingsDigest(false, IdentifierType.PhysiciansAndSurgeons));

        await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        A.CallTo(() => keycloak.RemoveAccessRoles(party.Credentials.Single().UserId, MohKeycloakEnrolment.ImmsBCEforms)).MustHaveHappened();
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
        var service = this.MockDependenciesFor<ImmsBCEformsRevocationPolicy>(plr, keycloak, revocationService);

        await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        Assert.Contains(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        Assert.Contains(this.TestDb.BusinessEvents, businessEvent => businessEvent is AccessRequestRevoked
            && businessEvent.Severity == LogLevel.Error);
    }

    [Theory]
    [MemberData(nameof(IdentifierTypeTestData))]
    public async Task RevokeIfIneligible_VaryingLicence_RevokesWhenNotAllowed(IdentifierType identifierType, bool stillEligible)
    {
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
            A.CallTo(() => keycloak.RemoveAccessRoles(A<Guid>._, MohKeycloakEnrolment.ImmsBCEforms)).MustHaveHappened();
            Assert.DoesNotContain(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        }
    }

    public static TheoryData<IdentifierType, bool> IdentifierTypeTestData()
    {
        var testData = new TheoryData<IdentifierType, bool>();

        foreach (var identifierType in TestData.AllIdentifierTypes)
        {
            testData.Add(identifierType, ImmsBCEforms.AllowedIdentifierTypes.Contains(identifierType));
        }

        return testData;
    }

    [Theory]
    [MemberData(nameof(EndorsementStandingTestData))]
    public async Task RevokeIfIneligible_NoCpn_UsesEndorsementStanding(PlrStandingsDigest endorsementDigest, bool stillEligible)
    {
        // An MOA holds the card on the strength of their endorsements, so losing the last
        // endorser in good standing must take it away again.
        var party = this.HasAnEnroledParty(party => party.Cpn = null);
        var (_, keycloak, service) = this.SetupFor(endorsementDigest);

        await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        if (stillEligible)
        {
            keycloak.AssertNoRolesRemoved();
            Assert.Contains(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        }
        else
        {
            A.CallTo(() => keycloak.RemoveAccessRoles(A<Guid>._, MohKeycloakEnrolment.ImmsBCEforms)).MustHaveHappened();
            Assert.DoesNotContain(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        }
    }

    public static TheoryData<PlrStandingsDigest, bool> EndorsementStandingTestData() => new()
    {
        { AMock.StandingsDigest(true, providerRoleType: ProviderRoleType.MedicalDoctor), true },
        { AMock.StandingsDigest(true, IdentifierType.Nurse), true },
        // Narrower than RSV and NPDP: a Midwife may hold Imms themselves but cannot confer it on an MOA.
        { AMock.StandingsDigest(true, IdentifierType.Midwife), false },
        { AMock.StandingsDigest(true, IdentifierType.Pharmacist), false },
        { AMock.StandingsDigest(false, providerRoleType: ProviderRoleType.MedicalDoctor), false },
        { PlrStandingsDigest.FromEmpty(), false },
    };

    [Fact]
    public async Task RevokeIfIneligible_CalledTwice_IsIdempotent()
    {
        var party = this.HasAnEnroledParty();
        var (_, keycloak, service) = this.SetupFor(AMock.StandingsDigest(false));

        await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();
        await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        A.CallTo(() => keycloak.RemoveAccessRoles(A<Guid>._, MohKeycloakEnrolment.ImmsBCEforms)).MustHaveHappenedOnceExactly();
        Assert.Single(this.TestDb.BusinessEvents, businessEvent => businessEvent is AccessRequestRevoked);
    }

    private Party HasAnEnroledParty(Action<Party>? config = null) => this.TestDb.HasAParty(party =>
    {
        party.Cpn = "Cpn";
        party.Credentials = [new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCServicesCard }];
        party.AccessRequests = [new AccessRequest { AccessTypeCode = AccessTypeCode.ImmsBCEforms, RequestedOn = Instant.FromUtc(2026, 1, 1, 0, 0) }];
        config?.Invoke(party);
    });

    /// <summary>
    /// Builds the policy over a REAL AccessRequestRevocationService, so these tests continue to
    /// exercise the eligibility rules and the removal mechanics together. Faking the service would
    /// leave every assertion about deleted Access Requests and Business Events vacuous.
    /// </summary>
    private (IPlrClient Plr, IKeycloakAdministrationClient Keycloak, ImmsBCEformsRevocationPolicy Policy) SetupFor(PlrStandingsDigest digest)
    {
        var plr = A.Fake<IPlrClient>().ReturningAStandingsDigest(digest);
        var keycloak = A.Fake<IKeycloakAdministrationClient>().ReturningTrueWhenRemovingClientRoles();
        var revocationService = this.MockDependenciesFor<AccessRequestRevocationService>(keycloak);

        return (plr, keycloak, this.MockDependenciesFor<ImmsBCEformsRevocationPolicy>(plr, keycloak, revocationService));
    }
}
