namespace Pidp.Features.Pharmacies;

using FluentValidation;
using MediatR;
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
        public string Address { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string PharmaCareCode { get; set; } = string.Empty;
        public int RequestingPartyId { get; set; } = 0;
    }

    public class CommandHandler(IClock clock, PidpDbContext context) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var partyIsAdmin = await context.PharmacyPartyRoles
                .AnyAsync(role => role.PartyId == request.RequestingPartyId
                               && role.PharmacyId == request.PharmacyId
                               && role.Role == PharmacyRole.Admin,
                          cancellationToken);

            if (!partyIsAdmin)
            {
                throw new AccessViolationException("User is not an admin of this pharmacy.");
            }

            var pharmacy = await context.Pharmacies.FindAsync(new object[] { request.PharmacyId }, cancellationToken);

            if (pharmacy is null)
            {
                throw new KeyNotFoundException();
            }

            context.Entry(pharmacy).CurrentValues.SetValues(request);
            context.BusinessEvents.Add(PharmacyUpdated.Create(request.RequestingPartyId, pharmacy.Name, clock.GetCurrentInstant()));

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}