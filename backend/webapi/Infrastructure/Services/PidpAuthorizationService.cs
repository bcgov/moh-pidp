namespace Pidp.Infrastructure.Services;

using DomainResults.Common;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

using Pidp.Data;
using Pidp.Extensions;

public class PidpAuthorizationService : IPidpAuthorizationService
{
    private readonly PidpDbContext context;

    public PidpAuthorizationService(PidpDbContext context) => this.context = context;

    public async Task<IDomainResult> CheckPartyAccessibility(int partyId, ClaimsPrincipal user)
    {
        var partyUserIds = await this.context.Credentials
            .AsNoTracking()
            .Where(credential => credential.PartyId == partyId)
            .Select(credential => credential.UserId)
            .ToListAsync();

        if (!partyUserIds.Any())
        {
            return DomainResult.NotFound();
        }

        var userId = user.GetUserId();
        if (userId != Guid.Empty
            && partyUserIds.Contains(userId))
        {
            return DomainResult.Success();
        }

        return DomainResult.Unauthorized();
    }
}
