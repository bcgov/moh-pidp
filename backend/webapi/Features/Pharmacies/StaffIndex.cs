namespace Pidp.Features.Pharmacies;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Pidp.Data;
using Pidp.Models;
using Pidp.Models.Lookups;

public class Staff
{
    public class Query : IRequest<List<Model>>
    {
        public int PharmacyId { get; set; }
        public int PartyId { get; set; } // Requesting party
    }

    public class Model
    {
        public int PartyId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public PharmacyRole Role { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
    }

    public class QueryHandler(PidpDbContext context) : IRequestHandler<Query, List<Model>>
    {
        public async Task<List<Model>> Handle(Query request, CancellationToken cancellationToken)
        {
            var partyIsAdmin = await context.PharmacyPartyRoles
                .AnyAsync(role => role.PartyId == request.PartyId
                               && role.PharmacyId == request.PharmacyId
                               && role.Role == PharmacyRole.Admin,
                          cancellationToken);

            if (!partyIsAdmin)
            {
                return new List<Model>(); // Or throw
            }

            return await context.PharmacyPartyRoles
                .Where(role => role.PharmacyId == request.PharmacyId)
                .Select(role => new Model
                {
                    PartyId = role.PartyId,
                    FullName = role.Party.FullName, // Assuming Party has FullName
                    Role = role.Role,
                    EffectiveStartDate = role.EffectiveStartDate ?? DateTime.MinValue,
                    EffectiveEndDate = role.EffectiveEndDate ?? DateTime.MinValue,
                })
                .ToListAsync(cancellationToken);
        }
    }
}