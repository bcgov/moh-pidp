namespace Pidp.Features.Lookups;

using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Models.Lookups;

public class DMFT
{
    public class Query : IQuery<List<Model>>
    {
        public string Hpdid { get; set; } = string.Empty;
    }

    public class Model
    {
        public string Hpdid { get; set; } = string.Empty;
        public List<LicenceInformation> ColegeLicences { get; set; } = new();

        public class LicenceInformation
        {
            public string? IdentifierType { get; set; }
            public string? StatusCode { get; set; }
            public string? StatusReasonCode { get; set; }
        }
    }

    public class QueryHandler : IQueryHandler<Query, List<Model>>
    {
        private readonly PidpDbContext context;

        public QueryHandler(PidpDbContext context) => this.context = context;

        public async Task<List<Model>> HandleAsync(Query query)
        {
            throw new NotImplementedException();
        }
    }
}
