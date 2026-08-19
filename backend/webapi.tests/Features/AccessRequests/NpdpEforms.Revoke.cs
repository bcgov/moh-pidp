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

public class NpdpEformsRevocationTests : InMemoryDbTest
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
    public async Task RevokeIfIneligible_NoLongerEligible_RemovesRoleAndAccessRequest()
    {
        var party = this.HasAnEnroledParty();
        var (_, keycloak, service) = this.SetupFor(AMock.StandingsDigest(false, IdentifierType.PhysiciansAndSurgeons));

        await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        A.CallTo(() => keycloak.RemoveAccessRoles(party.Credentials.Single().UserId, MohKeycloakEnrolment.NpdpEforms)).MustHaveHappened();
        Assert.DoesNotContain(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        Assert.Contains(this.TestDb.BusinessEvents, businessEvent => businessEvent is AccessRequestRevoked
            && businessEvent.Severity == LogLevel.Information);
    }

    [Fact]
    public async Task RevokeIfIneligible_NoLongerEligible_RemovesFromAllCredentials()
    {
        // Assign narrowly, remove broadly. The grant only targets BC Services Card credentials, but
        // removal deliberately covers every credential: Keycloak no-ops on a role the User does not
        // hold, and this cleans up grants made by an earlier version of the rule, which did include
        // BC Provider.
        var bcsc = new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCServicesCard };
        var bcProvider = new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCProvider };
        var party = this.HasAnEnroledParty(party => party.Credentials = [bcsc, bcProvider]);
        var (_, keycloak, service) = this.SetupFor(AMock.StandingsDigest(false));

        await service.RevokeIfIneligibleAsync(party.Id);
        await this.TestDb.SaveChangesAsync();

        A.CallTo(() => keycloak.RemoveAccessRoles(bcsc.UserId, MohKeycloakEnrolment.NpdpEforms)).MustHaveHappened();
        A.CallTo(() => keycloak.RemoveAccessRoles(bcProvider.UserId, MohKeycloakEnrolment.NpdpEforms)).MustHaveHappened();
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
        var service = this.MockDependenciesFor<NpdpEformsRevocationPolicy>(plr, keycloak, revocationService);

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
            A.CallTo(() => keycloak.RemoveAccessRoles(A<Guid>._, MohKeycloakEnrolment.NpdpEforms)).MustHaveHappened();
            Assert.DoesNotContain(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        }
    }

    public static TheoryData<IdentifierType, bool> IdentifierTypeTestData()
    {
        var testData = new TheoryData<IdentifierType, bool>();

        foreach (var identifierType in TestData.AllIdentifierTypes)
        {
            testData.Add(identifierType, NpdpEforms.AllowedIdentifierTypes.Contains(identifierType));
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
            A.CallTo(() => keycloak.RemoveAccessRoles(A<Guid>._, MohKeycloakEnrolment.NpdpEforms)).MustHaveHappened();
            Assert.DoesNotContain(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        }
    }

    public static TheoryData<PlrStandingsDigest, bool> EndorsementStandingTestData() => new()
    {
        { AMock.StandingsDigest(true, providerRoleType: ProviderRoleType.MedicalDoctor), true },
        { AMock.StandingsDigest(true, IdentifierType.Nurse), true },
        { AMock.StandingsDigest(true, IdentifierType.Midwife), true },
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

        A.CallTo(() => keycloak.RemoveAccessRoles(A<Guid>._, MohKeycloakEnrolment.NpdpEforms)).MustHaveHappenedOnceExactly();
        Assert.Single(this.TestDb.BusinessEvents, businessEvent => businessEvent is AccessRequestRevoked);
    }

    [Fact]
    public void AccessRequestRevoked_MultiRoleEnrolment_NamesEveryRole()
    {
        // ProviderReportingPortal carries two roles. The existing LicenceStatusRoleAssigned and
        // LicenceStatusRoleUnassigned events both call AccessRoles.Single() and throw on it; this
        // is the guard against inheriting that bug in the generic event.
        var enrolment = MohKeycloakEnrolment.ProviderReportingPortal;
        Assert.True(enrolment.AccessRoles.Count() > 1, "This test is meaningless unless the enrolment has multiple roles.");

        var businessEvent = AccessRequestRevoked.Create(1, AccessTypeCode.ProviderReportingPortal, enrolment, "Cpn", [Guid.NewGuid()], "a reason", "a trigger", Instant.FromUtc(2026, 1, 1, 0, 0));

        foreach (var role in enrolment.AccessRoles)
        {
            Assert.Contains(role, businessEvent.Description);
        }
    }

    [Fact]
    public void AccessRequestRevoked_NoAssociatedKeycloakRole_StillDescribesTheRevocation()
    {
        // Some Access Types (the MS Teams ones, UserAccessAgreement) grant no Keycloak role at all.
        var businessEvent = AccessRequestRevoked.Create(1, AccessTypeCode.MSTeamsClinicMember, null, null, [], "a reason", "a trigger", Instant.FromUtc(2026, 1, 1, 0, 0));

        Assert.Contains(nameof(AccessTypeCode.MSTeamsClinicMember), businessEvent.Description);
        Assert.Contains("a reason", businessEvent.Description);
    }

    private Party HasAnEnroledParty(Action<Party>? config = null) => this.TestDb.HasAParty(party =>
    {
        party.Cpn = "Cpn";
        party.Credentials = [new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCServicesCard }];
        party.AccessRequests = [new AccessRequest { AccessTypeCode = AccessTypeCode.NpdpEforms, RequestedOn = Instant.FromUtc(2026, 1, 1, 0, 0) }];
        config?.Invoke(party);
    });

    /// <summary>
    /// Builds the policy over a REAL AccessRequestRevocationService, so these tests continue to
    /// exercise the eligibility rules and the removal mechanics together. Faking the service would
    /// leave every assertion about deleted Access Requests and Business Events vacuous.
    /// </summary>
    private (IPlrClient Plr, IKeycloakAdministrationClient Keycloak, NpdpEformsRevocationPolicy Policy) SetupFor(PlrStandingsDigest digest)
    {
        var plr = A.Fake<IPlrClient>().ReturningAStandingsDigest(digest);
        var keycloak = A.Fake<IKeycloakAdministrationClient>().ReturningTrueWhenRemovingClientRoles();
        var revocationService = this.MockDependenciesFor<AccessRequestRevocationService>(keycloak);

        return (plr, keycloak, this.MockDependenciesFor<NpdpEformsRevocationPolicy>(plr, keycloak, revocationService));
    }
}
