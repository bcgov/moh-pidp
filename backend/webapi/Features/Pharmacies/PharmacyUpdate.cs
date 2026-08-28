#pragma warning disable CA1805

namespace Pidp.Features.Pharmacies;

using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Pidp.Data;
using Pidp.Models;
using Pidp.Models.Lookups;
using NodaTime;

public class PharmacyUpdate
{
    public class Command : IRequest
    {
        public int PharmacyId { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string HealthAuthority { get; set; } = string.Empty;
        public string Address1 { get; set; } = string.Empty;
        public string? Address2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string PharmaCareCode { get; set; } = string.Empty;
        public int RequestingPartyId { get; set; } = 0;
    }

    public class CommandHandler(IClock clock, PidpDbContext context) : IRequestHandler<Command>
    {
        public async ValueTask<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var partyIsAdmin = await context.PharmacyPartyRoles
                .AnyAsync(role => role.PartyId == request.RequestingPartyId
                               && role.PharmacyId == request.PharmacyId
                               && role.Role == PharmacyRole.Admin,
                          cancellationToken);

            if (!partyIsAdmin)
            {
                throw new InvalidOperationException("User is not an admin of this pharmacy.");
            }

            var pharmacy = await context.Pharmacies.FindAsync(new object[] { request.PharmacyId }, cancellationToken);

            if (pharmacy is null)
            {
                throw new KeyNotFoundException();
            }

            if (pharmacy.Name != request.Name)
            {
                throw new InvalidOperationException("Pharmacy name cannot be modified.");
            }

            request.Address2 ??= string.Empty;
            context.Entry(pharmacy).CurrentValues.SetValues(request);
            context.BusinessEvents.Add(PharmacyUpdated.Create(request.RequestingPartyId, pharmacy.Name, clock.GetCurrentInstant()));

            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
