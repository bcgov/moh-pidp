namespace Pidp.Features.Pharmacies;

using Mapster;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Pidp.Data;
using Pidp.Models;

public class PharmacyDetails
{
    public class Query : IRequest<Model?>
    {
        public int PharmacyId { get; set; }
        public int PartyId { get; set; }
    }

    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string PharmaCareCode { get; set; } = string.Empty;
    }

    public class QueryHandler(PidpDbContext context) : IRequestHandler<Query, Model?>
    {
        public async ValueTask<Model?> Handle(Query request, CancellationToken cancellationToken)
        {
            var canAccess = await context.PharmacyPartyRoles
                .AnyAsync(role => role.PartyId == request.PartyId && role.PharmacyId == request.PharmacyId, cancellationToken);

            if (!canAccess)
            {
                return null;
            }

            return await context.Pharmacies
                .Where(p => p.Id == request.PharmacyId)
                .ProjectToType<Model>()
                .SingleOrDefaultAsync(cancellationToken);
        }
    }

}
