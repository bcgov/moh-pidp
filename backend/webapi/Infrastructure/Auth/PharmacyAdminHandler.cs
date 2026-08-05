namespace Pidp.Infrastructure.Auth;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Models.Lookups;

public class PharmacyAdminHandler(IHttpContextAccessor httpContextAccessor, PidpDbContext context) : AuthorizationHandler<PharmacyAdminRequirement>
{
    private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
    private readonly PidpDbContext context = context;

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