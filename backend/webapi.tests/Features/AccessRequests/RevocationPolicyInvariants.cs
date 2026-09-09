namespace PidpTests.Features.AccessRequests;

using FakeItEasy;
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

/// <summary>
/// Properties that must hold for EVERY revocation policy, not just the one it was written for.
///
/// The do-work bulk cleanup tool runs these exact policies against production data. Two things make that
/// safe, and neither is visible from any single policy:
///
/// 1. A policy's sole write path is IAccessRequestRevocationService, so substituting that service is enough
///    to get a dry run. A policy that started staging its own DbContext changes would make every future dry
///    run quietly lie.
/// 2. The RevocationDecision a policy returns is what the tool reports. A policy that returned the wrong
///    outcome, or one belonging to another card, would produce a report a reviewer then approves.
///
/// Both are pinned here rather than left as something a reviewer has to remember.
/// </summary>
public class RevocationPolicyInvariantTests : InMemoryDbTest
{
    public static TheoryData<AccessTypeCode> AllPolicies() =>
    [
        AccessTypeCode.HcimWebPcr,
        AccessTypeCode.ImmsBCEforms,
        AccessTypeCode.InfantRsvEforms,
        AccessTypeCode.NpdpEforms,
        AccessTypeCode.SAEforms,
    ];

    [Theory]
    [MemberData(nameof(AllPolicies))]
    public async Task RevokeIfIneligible_WithSubstitutedRevocationService_StagesNothingItself(AccessTypeCode accessTypeCode)
    {
        // An ineligible holder: the case where the policy does the most work and is most likely to write.
        var party = this.HasAnEnroledParty(accessTypeCode);
        var plr = A.Fake<IPlrClient>().ReturningAStandingsDigest(AMock.StandingsDigest(false, IdentifierType.PhysiciansAndSurgeons));
        var keycloak = A.Fake<IKeycloakAdministrationClient>().ReturningTrueWhenRemovingClientRoles();
        var recorder = new RecordingRevocationService();
        var policy = this.PolicyFor(accessTypeCode, plr, keycloak, recorder);

        var decision = await policy.RevokeIfIneligibleAsync(party.Id);

        // The substituted service saw the decision...
        Assert.Contains(recorder.Recorded, recorded => recorded.PartyId == party.Id && recorded.AccessTypeCode == accessTypeCode);
        // ...the policy reported it back to the caller...
        Assert.Equal(RevocationOutcome.Revoked, decision.Outcome);
        Assert.True(decision.WasIneligible);
        Assert.False(string.IsNullOrWhiteSpace(decision.Reason), "A revocation must say why; the reason is what a reviewer approves.");
        // ...and the policy itself wrote nothing at all.
        Assert.False(this.TestDb.ChangeTracker.HasChanges(), $"{policy.GetType().Name} staged its own DbContext changes, which would break the bulk tool's dry run.");
        keycloak.AssertNoRolesRemoved();
        Assert.Contains(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        Assert.Empty(this.TestDb.BusinessEvents);
    }

    [Theory]
    [MemberData(nameof(AllPolicies))]
    public async Task RevokeIfIneligible_EligibleHolder_ReportsNoDecision(AccessTypeCode accessTypeCode)
    {
        // The counterpart: a policy that leaves someone alone must not record a decision either, or a dry
        // run would report a revocation that was never going to happen.
        var party = this.HasAnEnroledParty(accessTypeCode);
        var plr = A.Fake<IPlrClient>().ReturningAStandingsDigest(AMock.StandingsDigest(true, IdentifierType.PhysiciansAndSurgeons));
        var keycloak = A.Fake<IKeycloakAdministrationClient>().ReturningTrueWhenRemovingClientRoles();
        var recorder = new RecordingRevocationService();
        var policy = this.PolicyFor(accessTypeCode, plr, keycloak, recorder);

        var decision = await policy.RevokeIfIneligibleAsync(party.Id);

        Assert.Empty(recorder.Recorded);
        Assert.Equal(RevocationOutcome.Eligible, decision.Outcome);
        Assert.False(decision.WasIneligible);
        Assert.False(this.TestDb.ChangeTracker.HasChanges());
    }

    [Theory]
    [MemberData(nameof(AllPolicies))]
    public async Task RevokeIfIneligible_PlrStandingUnknown_LeavesAccessInPlace(AccessTypeCode accessTypeCode)
    {
        // Fail closed. An unreachable PLR yields a digest with no records, which is indistinguishable from
        // "not in good standing" unless the Error flag is honoured. Getting this wrong on any one card
        // would strip every holder of it during an outage, so it is asserted for all of them.
        var party = this.HasAnEnroledParty(accessTypeCode);
        var plr = A.Fake<IPlrClient>().ReturningAStandingsDigest(PlrStandingsDigest.FromError());
        var keycloak = A.Fake<IKeycloakAdministrationClient>().ReturningTrueWhenRemovingClientRoles();
        var recorder = new RecordingRevocationService();
        var policy = this.PolicyFor(accessTypeCode, plr, keycloak, recorder);

        var decision = await policy.RevokeIfIneligibleAsync(party.Id);

        Assert.Empty(recorder.Recorded);
        // Distinct from Eligible on purpose: the bulk tool reports it separately so a PLR gap during a run
        // is visible in the report rather than reading as a clean bill of health.
        Assert.Equal(RevocationOutcome.StandingUnknown, decision.Outcome);
        Assert.Contains(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        Assert.False(this.TestDb.ChangeTracker.HasChanges());
    }

    [Theory]
    [MemberData(nameof(AllPolicies))]
    public async Task RevokeIfIneligible_PartyDoesNotHoldTheCard_IsANoOp(AccessTypeCode accessTypeCode)
    {
        // Every webapi trigger runs all policies against one Party, so most calls are for a card the Party
        // never held. That has to be free of both writes and PLR traffic.
        var party = this.TestDb.HasAParty(party => party.Cpn = "Cpn");
        var plr = A.Fake<IPlrClient>().ReturningAStandingsDigest(AMock.StandingsDigest(false, IdentifierType.PhysiciansAndSurgeons));
        var recorder = new RecordingRevocationService();
        var policy = this.PolicyFor(accessTypeCode, plr, A.Fake<IKeycloakAdministrationClient>(), recorder);

        var decision = await policy.RevokeIfIneligibleAsync(party.Id);

        Assert.Equal(RevocationOutcome.NotHeld, decision.Outcome);
        Assert.Empty(recorder.Recorded);
        A.CallTo(() => plr.GetStandingsDigestAsync(A<string?>._)).MustNotHaveHappened();
        A.CallTo(() => plr.GetAggregateStandingsDigestAsync(A<IEnumerable<string?>>._)).MustNotHaveHappened();
        Assert.False(this.TestDb.ChangeTracker.HasChanges());
    }

    [Theory]
    [MemberData(nameof(AllPolicies))]
    public async Task Decision_AlwaysNamesTheCardThePolicyIsFor(AccessTypeCode accessTypeCode)
    {
        // The bulk tool selects a policy by AccessTypeCode and reports every decision under it. A mismatch
        // would mean running "cleanup for SA" and revoking something else entirely.
        var party = this.HasAnEnroledParty(accessTypeCode);
        var plr = A.Fake<IPlrClient>().ReturningAStandingsDigest(AMock.StandingsDigest(false, IdentifierType.PhysiciansAndSurgeons));
        var recorder = new RecordingRevocationService();
        var policy = this.PolicyFor(accessTypeCode, plr, A.Fake<IKeycloakAdministrationClient>(), recorder);

        Assert.Equal(accessTypeCode, policy.AccessTypeCode);

        var decision = await policy.RevokeIfIneligibleAsync(party.Id);

        Assert.Equal(accessTypeCode, decision.AccessTypeCode);
        Assert.All(recorder.Recorded, recorded => Assert.Equal(accessTypeCode, recorded.AccessTypeCode));
    }

    private Party HasAnEnroledParty(AccessTypeCode accessTypeCode) => this.TestDb.HasAParty(party =>
    {
        party.Cpn = "Cpn";
        party.Credentials = [new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCServicesCard }];
        party.AccessRequests = [new AccessRequest { AccessTypeCode = accessTypeCode, RequestedOn = Instant.FromUtc(2026, 1, 1, 0, 0) }];
    });

    private IAccessRequestRevocationPolicy PolicyFor(
        AccessTypeCode accessTypeCode,
        IPlrClient plr,
        IKeycloakAdministrationClient keycloak,
        IAccessRequestRevocationService revocationService) => accessTypeCode switch
        {
            AccessTypeCode.HcimWebPcr => this.MockDependenciesFor<HcimWebPcrRevocationPolicy>(plr, keycloak, revocationService),
            AccessTypeCode.ImmsBCEforms => this.MockDependenciesFor<ImmsBCEformsRevocationPolicy>(plr, keycloak, revocationService),
            AccessTypeCode.InfantRsvEforms => this.MockDependenciesFor<InfantRsvEformsRevocationPolicy>(plr, keycloak, revocationService),
            AccessTypeCode.NpdpEforms => this.MockDependenciesFor<NpdpEformsRevocationPolicy>(plr, keycloak, revocationService),
            AccessTypeCode.SAEforms => this.MockDependenciesFor<SAEformsRevocationPolicy>(plr, keycloak, revocationService),
            _ => throw new ArgumentOutOfRangeException(nameof(accessTypeCode), accessTypeCode, "No revocation policy is registered for this Access Type.")
        };

    /// <summary>
    /// Mirrors DoWork's RecordingRevocationService. Duplicated rather than referenced because webapi.tests
    /// does not depend on the tools project; the assertions above are what keep the two honest.
    /// </summary>
    private sealed class RecordingRevocationService : IAccessRequestRevocationService
    {
        private readonly List<(int PartyId, AccessTypeCode AccessTypeCode, string Reason)> recorded = [];

        public IReadOnlyList<(int PartyId, AccessTypeCode AccessTypeCode, string Reason)> Recorded => this.recorded;

        public Task<bool> RevokeAsync(int partyId, AccessTypeCode accessTypeCode, string reason, string? trigger = null, CancellationToken cancellationToken = default)
        {
            this.recorded.Add((partyId, accessTypeCode, reason));
            return Task.FromResult(true);
        }
    }
}
