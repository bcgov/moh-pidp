namespace Pidp.Features.Pharmacies;

using Mediator;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Pidp.Data;
using Pidp.Infrastructure.Services;
using Pidp.Models;
using Pidp.Models.Lookups;

public class StaffDelete
{
    public class Command : IRequest
    {
        public int PharmacyId { get; set; }
        public int PartyId { get; set; }
        public int RequestingPartyId { get; set; }
    }

    public class CommandHandler(PidpDbContext context, IClock clock, IBCProviderService bcProviderService) : IRequestHandler<Command>
    {
        public async ValueTask<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var requestingPartyIsAdmin = await context.PharmacyPartyRoles
                .AnyAsync(role => role.PartyId == request.RequestingPartyId
                               && role.PharmacyId == request.PharmacyId
                               && role.Role == PharmacyRole.Admin,
                          cancellationToken);

            if (!requestingPartyIsAdmin)
            {
                throw new InvalidOperationException("User is not an admin of this pharmacy.");
            }

            if (request.PartyId == request.RequestingPartyId)
            {
                throw new InvalidOperationException("An admin cannot remove themselves from a pharmacy.");
            }

            var staffRole = await context.PharmacyPartyRoles
                .SingleOrDefaultAsync(role => role.PartyId == request.PartyId
                                           && role.PharmacyId == request.PharmacyId,
                                      cancellationToken);

            if (staffRole is not null)
            {
                var pharmacyName = await context.Pharmacies
                    .Where(p => p.Id == request.PharmacyId)
                    .Select(p => p.Name)
                    .SingleOrDefaultAsync(cancellationToken) ?? string.Empty;

                context.BusinessEvents.Add(PharmacyStaffChanged.Create(request.RequestingPartyId, pharmacyName, clock.GetCurrentInstant()));

                context.PharmacyPartyRoles.Remove(staffRole);
                await context.SaveChangesAsync(cancellationToken);

                await bcProviderService.UpdatePharmStaffAttributes(request.PartyId, cancellationToken);
            }
            return Unit.Value;
        }
    }
}
