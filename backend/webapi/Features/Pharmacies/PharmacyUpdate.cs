namespace Pidp.Features.Pharmacies;

using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pidp.Data;
using Pidp.Models;
using Pidp.Models.Lookups;

public class PharmacyUpdate
{
    public class Command : IRequest
    {
        public int? PharmacyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string PharmaCareCode { get; set; } = string.Empty;
        public int? RequestingPartyId { get; set; }
    }

    public class CommandHandler(PidpDbContext context) : IRequestHandler<Command>
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

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}