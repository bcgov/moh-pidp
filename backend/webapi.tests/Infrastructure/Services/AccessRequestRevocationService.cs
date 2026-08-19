namespace PidpTests.Infrastructure.Services;

using FakeItEasy;
using Microsoft.Extensions.Logging;
using NodaTime;
using Xunit;

using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.Services;
using Pidp.Models;
using Pidp.Models.Lookups;
using PidpTests.TestingExtensions;

/// <summary>
/// Covers the card-agnostic mechanics directly. The per-card policies are tested through their own
/// suites; these are the branches no single card exercises.
/// </summary>
public class AccessRequestRevocationServiceTests : InMemoryDbTest
{
    [Fact]
    public async Task RevokeAsync_NotEnroled_NoOp()
    {
        var party = this.HasAPartyEnroledIn(null);
        var (keycloak, service) = this.SetupService();

        var revoked = await service.RevokeAsync(party.Id, AccessTypeCode.InfantRsvEforms, "a reason");
        await this.TestDb.SaveChangesAsync();

        Assert.False(revoked);
        keycloak.AssertNoRolesRemoved();
        Assert.Empty(this.TestDb.BusinessEvents);
    }

    [Fact]
    public async Task RevokeAsync_NoAssociatedKeycloakRole_StillDeletesAndLogs()
    {
        // MSTeamsClinicMember grants no Keycloak role, so FromAssociatedAccessRequest returns null.
        // The Access Request must still be removed and recorded rather than silently skipped.
        Assert.Null(MohKeycloakEnrolment.FromAssociatedAccessRequest(AccessTypeCode.MSTeamsClinicMember));
        var party = this.HasAPartyEnroledIn(AccessTypeCode.MSTeamsClinicMember);
        var (keycloak, service) = this.SetupService();

        var revoked = await service.RevokeAsync(party.Id, AccessTypeCode.MSTeamsClinicMember, "a reason");
        await this.TestDb.SaveChangesAsync();

        Assert.True(revoked);
        keycloak.AssertNoRolesRemoved();
        Assert.DoesNotContain(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        Assert.Contains(this.TestDb.BusinessEvents, businessEvent => businessEvent is AccessRequestRevoked
            && businessEvent.Severity == LogLevel.Information);
    }

    [Fact]
    public async Task RevokeAsync_RemovesFromEveryCredential()
    {
        // Assign narrowly, remove broadly - a grant may have targeted only some Identity Providers,
        // including under an earlier version of the rule.
        var bcsc = new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCServicesCard };
        var bcProvider = new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCProvider };
        var party = this.HasAPartyEnroledIn(AccessTypeCode.InfantRsvEforms, party => party.Credentials = [bcsc, bcProvider]);
        var (keycloak, service) = this.SetupService();

        await service.RevokeAsync(party.Id, AccessTypeCode.InfantRsvEforms, "a reason");
        await this.TestDb.SaveChangesAsync();

        A.CallTo(() => keycloak.RemoveAccessRoles(bcsc.UserId, MohKeycloakEnrolment.InfantRsvEforms)).MustHaveHappened();
        A.CallTo(() => keycloak.RemoveAccessRoles(bcProvider.UserId, MohKeycloakEnrolment.InfantRsvEforms)).MustHaveHappened();
    }

    [Fact]
    public async Task RevokeAsync_KeycloakFails_KeepsAccessRequestAndRecordsFailure()
    {
        // Deleting the row while the role is still live would strand access with nothing to retry.
        var party = this.HasAPartyEnroledIn(AccessTypeCode.InfantRsvEforms);
        var keycloak = A.Fake<IKeycloakAdministrationClient>();
        A.CallTo(() => keycloak.RemoveAccessRoles(A<Guid>._, A<MohKeycloakEnrolment>._)).Returns(false);
        var service = this.MockDependenciesFor<AccessRequestRevocationService>(keycloak);

        var revoked = await service.RevokeAsync(party.Id, AccessTypeCode.InfantRsvEforms, "a reason");
        await this.TestDb.SaveChangesAsync();

        Assert.False(revoked);
        Assert.Contains(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        Assert.Contains(this.TestDb.BusinessEvents, businessEvent => businessEvent is AccessRequestRevoked
            && businessEvent.Severity == LogLevel.Error);
    }

    [Fact]
    public async Task RevokeAsync_CalledTwice_IsIdempotent()
    {
        var party = this.HasAPartyEnroledIn(AccessTypeCode.InfantRsvEforms);
        var (keycloak, service) = this.SetupService();

        Assert.True(await service.RevokeAsync(party.Id, AccessTypeCode.InfantRsvEforms, "a reason"));
        await this.TestDb.SaveChangesAsync();
        Assert.False(await service.RevokeAsync(party.Id, AccessTypeCode.InfantRsvEforms, "a reason"));
        await this.TestDb.SaveChangesAsync();

        A.CallTo(() => keycloak.RemoveAccessRoles(A<Guid>._, MohKeycloakEnrolment.InfantRsvEforms)).MustHaveHappenedOnceExactly();
        Assert.Single(this.TestDb.BusinessEvents, businessEvent => businessEvent is AccessRequestRevoked);
    }

    [Fact]
    public async Task RevokeAsync_OnlyTouchesTheGivenAccessType()
    {
        // The two eForms cards share a Keycloak Client, so a revocation must not disturb the other.
        var party = this.HasAPartyEnroledIn(AccessTypeCode.InfantRsvEforms, party => party.AccessRequests.Add(
            new AccessRequest { AccessTypeCode = AccessTypeCode.NpdpEforms, RequestedOn = Instant.FromUtc(2026, 1, 1, 0, 0) }));
        var (keycloak, service) = this.SetupService();

        await service.RevokeAsync(party.Id, AccessTypeCode.InfantRsvEforms, "a reason");
        await this.TestDb.SaveChangesAsync();

        A.CallTo(() => keycloak.RemoveAccessRoles(A<Guid>._, MohKeycloakEnrolment.NpdpEforms)).MustNotHaveHappened();
        Assert.Contains(this.TestDb.AccessRequests, request => request.PartyId == party.Id
            && request.AccessTypeCode == AccessTypeCode.NpdpEforms);
    }

    private Party HasAPartyEnroledIn(AccessTypeCode? accessTypeCode, Action<Party>? config = null) => this.TestDb.HasAParty(party =>
    {
        party.Cpn = "Cpn";
        party.Credentials = [new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCServicesCard }];
        if (accessTypeCode != null)
        {
            party.AccessRequests = [new AccessRequest { AccessTypeCode = accessTypeCode.Value, RequestedOn = Instant.FromUtc(2026, 1, 1, 0, 0) }];
        }

        config?.Invoke(party);
    });

    private (IKeycloakAdministrationClient Keycloak, AccessRequestRevocationService Service) SetupService()
    {
        var keycloak = A.Fake<IKeycloakAdministrationClient>().ReturningTrueWhenRemovingClientRoles();

        return (keycloak, this.MockDependenciesFor<AccessRequestRevocationService>(keycloak));
    }
}
