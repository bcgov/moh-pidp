namespace Pidp.Features.Pharmacies;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Models;
using Pidp.Models.Lookups;

public class Profile
{
    public class Query : IRequest<Model>
    {
        public int PartyId { get; set; }
    }

    public class Model
    {
        public bool IsPharmacyAdmin { get; set; }
        public List<AssociationModel> Associations { get; set; } = new();

        public class AssociationModel
        {
            public int PharmacyId { get; set; }
            public string PharmacyName { get; set; } = string.Empty;
            public PharmacyRole Role { get; set; }
        }
    }

    public class QueryHandler : IRequestHandler<Query, Model>
    {
        private readonly PidpDbContext context;
        private readonly IMapper mapper;

        public QueryHandler(PidpDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
        {
            var associations = await this.context.PharmacyPartyRoles
                .Where(role => role.PartyId == request.PartyId)
                .ProjectTo<Model.AssociationModel>(this.mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new Model
            {
                Associations = associations,
                IsPharmacyAdmin = associations.Any(a => a.Role == PharmacyRole.Admin)
            };
        }
    }

    public class ModelProjection : AutoMapper.Profile
    {
        public ModelProjection() => this.CreateMap<PharmacyPartyRole, Model.AssociationModel>();
    }
}