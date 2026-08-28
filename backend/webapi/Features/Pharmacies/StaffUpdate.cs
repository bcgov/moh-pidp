#pragma warning disable CA1805

namespace Pidp.Features.Pharmacies;

using DomainResults.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Infrastructure.Services;
using Pidp.Models;
using Pidp.Models.Lookups;

public class StaffUpdate
{
    public class Command : ICommand<IDomainResult>
    {
        public int PharmacyId { get; set; } = 0;
        public int PartyId { get; set; } = 0;
        public required PharmacyRole Role { get; set; }
        public DateTime? EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public int RequestingPartyId { get; set; } = 0;
    }

    public class CommandHandler(IClock clock, PidpDbContext context, IRoleSynchronizationService roleSynchronizationService, IPlrClient plrClient) : ICommandHandler<Command, IDomainResult>
    {
        public async ValueTask<IDomainResult> Handle(Command request, CancellationToken cancellationToken)
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

            var staffRole = await context.PharmacyPartyRoles
                .Include(role => role.Party)
                .SingleOrDefaultAsync(role => role.PartyId == request.PartyId
                                           && role.PharmacyId == request.PharmacyId,
                                      cancellationToken);

            if (staffRole is null)
            {
                throw new InvalidOperationException("Staff record not found.");
            }

            if (staffRole.Role == PharmacyRole.Admin && request.Role != PharmacyRole.Admin)
            {
                throw new InvalidOperationException("Admin role cannot be removed.");
            }

            var pharmacyName = await context.Pharmacies
                .Where(p => p.Id == request.PharmacyId)
                .Select(p => p.Name)
                .SingleOrDefaultAsync(cancellationToken) ?? string.Empty;

            context.BusinessEvents.Add(PharmacyStaffChanged.Create(request.RequestingPartyId, pharmacyName, clock.GetCurrentInstant()));

            staffRole.Role = request.Role;
            staffRole.EffectiveStartDate = request.EffectiveStartDate?.ToUniversalTime();
            staffRole.EffectiveEndDate = request.EffectiveEndDate?.ToUniversalTime();
            await context.SaveChangesAsync(cancellationToken);

            await roleSynchronizationService.UpdatePharmStaffAttributes(request.PartyId, cancellationToken);

            return DomainResult.Success();
        }
    }
}
