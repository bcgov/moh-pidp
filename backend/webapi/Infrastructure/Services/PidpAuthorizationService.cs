namespace Pidp.Infrastructure.Services;

using DomainResults.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;

using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Models;

public class PidpAuthorizationService : IPidpAuthorizationService
{
    private readonly IAuthorizationService authService;
    private readonly PidpDbContext context;

    public PidpAuthorizationService(IAuthorizationService authService, PidpDbContext context)
    {
        this.authService = authService;
        this.context = context;
    }

    public async Task<IDomainResult> CheckPartyAccessibility(int partyId, ClaimsPrincipal user) => await this.CheckResourceAccessibility((Party party) => party.Id == partyId, user, Policies.UserOwnsResource);

    public async Task<IDomainResult> CheckResourceAccessibility<T>(Expression<Func<T, bool>> predicate, ClaimsPrincipal user, string policy) where T : class, IOwnedResource
    {
        var resourceStub = await this.context.Set<T>()
            .AsNoTracking()
            .Where(predicate)
            .Select(x => new OwnedResourceStub { UserId = x.UserId })
            .SingleOrDefaultAsync();

        if (resourceStub == null)
        {
            return DomainResult.NotFound();
        }

        var result = await this.authService.AuthorizeAsync(user, resourceStub, policy);

        return result.Succeeded
            ? DomainResult.Success()
            : DomainResult.Unauthorized();
    }

    private class OwnedResourceStub : IOwnedResource
    {
        public Guid UserId { get; set; }
    }
}
