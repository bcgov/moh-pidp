namespace Pidp.Features.Pharmacies;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Pidp.Data;
using Pidp.Models;
using Pidp.Models.Lookups;

public class StaffDetails
{
    public class Query : IRequest<Model>
    {
        public int PharmacyId { get; set; }
        public int PartyId { get; set; }
    }

    public class Model
    {
        public int PartyId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public PharmacyRole Role { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
    }

    public class QueryHandler(PidpDbContext context) : IRequestHandler<Query, Model>
    {
        public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
        {
            return await context.PharmacyPartyRoles
                .Where(role => role.PharmacyId == request.PharmacyId && role.PartyId == request.PartyId)
                .Select(role => new Model
                {
                    PartyId = role.PartyId,
                    FullName = role.Party.FullName, // Assuming Party has FullName
                    Role = role.Role,
                    EffectiveStartDate = role.EffectiveStartDate ?? DateTime.MinValue,
                    EffectiveEndDate = role.EffectiveEndDate ?? DateTime.MinValue,
                })
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}