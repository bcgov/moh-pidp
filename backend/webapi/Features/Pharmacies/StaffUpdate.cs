namespace Pidp.Features.Pharmacies;

using MediatR;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Pidp.Data;
using Pidp.Infrastructure.Services;
using Pidp.Models;
using Pidp.Models.Lookups;

public class StaffUpdate
{
    public class Command : IRequest
    {
        public int PharmacyId { get; set; } = 0;
        public int PartyId { get; set; } = 0;
        public required PharmacyRole Role { get; set; }
        public DateTime? EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public int RequestingPartyId { get; set; } = 0;
    }

    public class CommandHandler(IClock clock, PidpDbContext context, IBCProviderService bcProviderService) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var requestingPartyIsAdmin = await context.PharmacyPartyRoles
                .AnyAsync(role => role.PartyId == request.RequestingPartyId
                               && role.PharmacyId == request.PharmacyId
                               && role.Role == PharmacyRole.Admin,
                          cancellationToken);

            if (!requestingPartyIsAdmin)
            {
                throw new AccessViolationException("User is not an admin of this pharmacy.");
            }

            var staffRole = await context.PharmacyPartyRoles
                .SingleOrDefaultAsync(role => role.PartyId == request.PartyId
                                           && role.PharmacyId == request.PharmacyId,
                                      cancellationToken);

            if (staffRole is null)
            {
                throw new KeyNotFoundException();
            }

            staffRole.Role = request.Role;
            staffRole.EffectiveStartDate = request.EffectiveStartDate?.ToUniversalTime();
            staffRole.EffectiveEndDate = request.EffectiveEndDate?.ToUniversalTime();

            var pharmacyName = await context.Pharmacies
                .Where(p => p.Id == request.PharmacyId)
                .Select(p => p.Name)
                .SingleOrDefaultAsync(cancellationToken) ?? string.Empty;

            context.BusinessEvents.Add(PharmacyStaffChanged.Create(request.RequestingPartyId, pharmacyName, clock.GetCurrentInstant()));

            await context.SaveChangesAsync(cancellationToken);

            await bcProviderService.UpdatePharmStaffAttributes(request.PartyId, cancellationToken);
        }
    }
}