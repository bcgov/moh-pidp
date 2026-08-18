namespace Pidp.Features.Admin;

using Mapster;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Models.Lookups;

public class PartyIndex
{
    public class Query : IQuery<List<Model>> { }

    public class Model
    {
        public int Id { get; set; }
        public string? ProviderName { get; set; }
        public CollegeCode? ProviderCollegeCode { get; set; }
        public bool SAEformsAccessRequest { get; set; }
        public List<CredentialModel> Credentials { get; set; } = [];

        public class CredentialModel
        {
            public int Id { get; set; }
            public string IdentityProvider { get; set; } = string.Empty;
            public string? IdpId { get; set; }
        }
    }

    public class QueryHandler(PidpDbContext context) : IQueryHandler<Query, List<Model>>
    {
        private readonly PidpDbContext context = context;

        public async Task<List<Model>> HandleAsync(Query query)
        {
            return await this.context.Parties
                .ProjectToType<Model>()
                .ToListAsync();
        }
    }
}
