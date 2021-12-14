namespace Pidp.Features.Parties;

using Microsoft.EntityFrameworkCore;

using Pidp.Data;

public class Index
{
    public class Query : IQuery<List<Model>>
    {
    }

    public class Model
    {
        public string FullName { get; set; } = string.Empty;
    }


    public class QueryHandler : IQueryHandler<Query, List<Model>>
    {
        private readonly PidpDbContext context;

        public QueryHandler(PidpDbContext context) => this.context = context;

        public async Task<List<Model>> HandleAsync(Query query)
        {
            return await this.context.Parties
                .Select(party => new Model
                {
                    FullName = $"{party.FirstName} {party.LastName}"
                })
                .ToListAsync();
        }
    }
}
