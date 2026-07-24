namespace Pidp.Features.Pharmacies;

using MediatR;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Pidp.Data;
using Pidp.Models;
using Pidp.Models.Lookups;

public class DeleteStaff
{
    public class Command : IRequest
    {
        public int PharmacyId { get; set; }
        public int PartyId { get; set; }
        public int RequestingPartyId { get; set; }
    }

    public class CommandHandler(PidpDbContext context, IClock clock) : IRequestHandler<Command>
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
                // staffRole.EffectiveEndDate = clock.GetCurrentInstant().ToDateTimeUtc();
                context.PharmacyPartyRoles.Remove(staffRole); // Or soft delete
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}