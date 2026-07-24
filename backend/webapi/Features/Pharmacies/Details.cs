namespace Pidp.Features.Pharmacies;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pidp.Data;
using Pidp.Models;

public class Details
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

    public class QueryHandler(PidpDbContext context, IMapper mapper) : IRequestHandler<Query, Model?>
    {
        public async Task<Model?> Handle(Query request, CancellationToken cancellationToken)
        {
            var canAccess = await context.PharmacyPartyRoles
                .AnyAsync(role => role.PartyId == request.PartyId && role.PharmacyId == request.PharmacyId, cancellationToken);

            if (!canAccess)
            {
                return null;
            }

            return await context.Pharmacies
                .Where(p => p.Id == request.PharmacyId)
                .ProjectTo<Model>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);
        }
    }

    public class ModelProjection : AutoMapper.Profile
    {
        public ModelProjection() => this.CreateMap<Pharmacy, Model>();
    }
}