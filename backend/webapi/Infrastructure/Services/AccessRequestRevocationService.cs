namespace Pidp.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Models;
using Pidp.Models.Lookups;

public class AccessRequestRevocationService(
    IClock clock,
    IKeycloakAdministrationClient keycloakClient,
    ILogger<AccessRequestRevocationService> logger,
    PidpDbContext context) : IAccessRequestRevocationService
{
    private readonly IClock clock = clock;
    private readonly IKeycloakAdministrationClient keycloakClient = keycloakClient;
    private readonly ILogger<AccessRequestRevocationService> logger = logger;
    private readonly PidpDbContext context = context;

    public async Task<bool> RevokeAsync(int partyId, AccessTypeCode accessTypeCode, string reason, string? trigger = null, CancellationToken cancellationToken = default)
    {
        var dto = await this.context.Parties
            .Where(party => party.Id == partyId)
            .Select(party => new
            {
                AccessRequestId = party.AccessRequests
                    .Where(request => request.AccessTypeCode == accessTypeCode)
                    .Select(request => (int?)request.Id)
                    .FirstOrDefault(),
                // Assign narrowly, remove broadly. A grant may target only certain Identity
                // Providers, but removing from every Credential costs nothing (Keycloak returns 204
                // for a role the User does not hold) and cleans up grants made by an earlier
                // version of the rule.
                UserIds = party.Credentials
                    .Select(credential => credential.UserId)
                    .ToList(),
                party.Cpn,
            })
            .SingleOrDefaultAsync(cancellationToken);

        // Most re-evaluations are for Parties who never held the Access Request. Bailing out here
        // keeps those cheap and makes a repeated revocation a no-op.
        if (dto?.AccessRequestId == null)
        {
            return false;
        }

        // Null is legitimate: some Access Types (the MS Teams ones, the User Access Agreement)
        // grant no Keycloak role at all, and still need the Access Request removed.
        var enrolment = MohKeycloakEnrolment.FromAssociatedAccessRequest(accessTypeCode);

        if (enrolment != null)
        {
            foreach (var userId in dto.UserIds)
            {
                if (!await this.keycloakClient.RemoveAccessRoles(userId, enrolment))
                {
                    // Leave the Access Request in place so the next trigger retries; deleting it
                    // would strand a live role with no record that the Party ever held it.
                    this.logger.LogRoleRemovalFailed(accessTypeCode, partyId, userId);
                    this.context.BusinessEvents.Add(AccessRequestRevoked.CreateFailure(partyId, accessTypeCode, enrolment, dto.Cpn, dto.UserIds, reason, trigger, this.clock.GetCurrentInstant()));
                    return false;
                }
            }
        }

        // FindAsync returns the already-tracked instance when there is one, so this cannot collide
        // with an Access Request the caller has loaded.
        var accessRequest = await this.context.AccessRequests.FindAsync([dto.AccessRequestId.Value], cancellationToken);
        if (accessRequest != null)
        {
            this.context.AccessRequests.Remove(accessRequest);
        }

        this.context.BusinessEvents.Add(AccessRequestRevoked.Create(partyId, accessTypeCode, enrolment, dto.Cpn, dto.UserIds, reason, trigger, this.clock.GetCurrentInstant()));

        this.logger.LogAccessRevoked(accessTypeCode, partyId);

        return true;
    }
}

public static partial class AccessRequestRevocationLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Information, "{accessTypeCode} access for Party {partyId} was revoked; they no longer meet the eligibility criteria.")]
    public static partial void LogAccessRevoked(this ILogger<AccessRequestRevocationService> logger, AccessTypeCode accessTypeCode, int partyId);

    [LoggerMessage(2, LogLevel.Error, "Failed to remove the {accessTypeCode} role(s) from User {userId} of Party {partyId}; the Access Request was left in place to be retried.")]
    public static partial void LogRoleRemovalFailed(this ILogger<AccessRequestRevocationService> logger, AccessTypeCode accessTypeCode, int partyId, Guid userId);
}
