namespace Pidp.Infrastructure.Services;

using DomainResults.Common;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Security.Claims;
using System.Text.Json;

using Pidp.Data;
using Pidp.Extensions;

public class PidpAuthorizationService : IPidpAuthorizationService
{
    private readonly IClock clock;
    private readonly PidpDbContext context;

    public PidpAuthorizationService(IClock clock, PidpDbContext context)
    {
        this.clock = clock;
        this.context = context;
    }

    public async Task<IDomainResult> CheckPartyAccessibilityAsync(int partyId, ClaimsPrincipal user)
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

    public async Task<string> SignTokenAsync(object claims)
    {
        // TODO: add expiry, signature
        return JsonSerializer.Serialize(claims);
    }

    public async Task<T?> VerifyTokenAsync<T>(string? token) where T : class
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        // TODO: Token validation + parsing

        try
        {
            return JsonSerializer.Deserialize<T>(token);
        }
        catch
        {
            return null;
        }
    }
}
