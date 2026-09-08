namespace Pidp.Features.Pharmacies;

using Mapster;
using Mediator;
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

        public QueryHandler(PidpDbContext context)
        {
            this.context = context;
        }

        public async ValueTask<Model> Handle(Query request, CancellationToken cancellationToken)
        {
            var associations = await this.context.PharmacyPartyRoles
                .Where(role => role.PartyId == request.PartyId)
                .ProjectToType<Model.AssociationModel>()
                .ToListAsync(cancellationToken);

            return new Model
            {
                Associations = associations,
                IsPharmacyAdmin = associations.Any(a => a.Role == PharmacyRole.Admin || a.Role == PharmacyRole.Lead)
            };
        }
    }

}
