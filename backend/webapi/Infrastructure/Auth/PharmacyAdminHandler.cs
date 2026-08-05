namespace Pidp.Infrastructure.Auth;

using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Pidp.Data;
using Pidp.Extensions;
using Pidp.Models.Lookups;

public class PharmacyAdminHandler : AuthorizationHandler<PharmacyAdminRequirement>
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly PidpDbContext context;

    public PharmacyAdminHandler(IHttpContextAccessor httpContextAccessor, PidpDbContext context)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.context = context;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PharmacyAdminRequirement requirement)
    {
        var httpContext = this.httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return;
        }

        var partyId = httpContext.User.GetPartyId(this.context);
        if (partyId == 0)
        {
            return;
        }

        if (!int.TryParse(httpContext.GetRouteValue("pharmacyId")?.ToString(), out var pharmacyId))
        {
            return;
        }

        var today = DateTime.Today;
        var isPharmacyAdmin = await this.context.PharmacyPartyRoles
            .AnyAsync(r => r.PartyId == partyId
                        && r.PharmacyId == pharmacyId
                        && r.Role == PharmacyRole.Admin
                        && r.EffectiveStartDate.HasValue && r.EffectiveStartDate.Value <= today
                        && r.EffectiveEndDate.HasValue && r.EffectiveEndDate.Value >= today);

        if (isPharmacyAdmin)
        {
            context.Succeed(requirement);
        }
    }
}