namespace Pidp.Features.Lookups;

using Microsoft.EntityFrameworkCore;

using Pidp.Data;

public class CommonEmailDomains
{
    public class Query : IQuery<List<string>> { }

    public class QueryHandler : IQueryHandler<Query, List<string>>
    {
        private readonly PidpDbContext context;

        public QueryHandler(PidpDbContext context) => this.context = context;

        public async Task<List<string>> HandleAsync(Query query)
        {
#pragma warning disable CA1304 // ToLower() is Locale Dependant
            return await this.context.Parties
                .Where(party => party.Email != null)
                .Select(party => party.Email!.Substring(party.Email.IndexOf("@") + 1).ToLower())
                .GroupBy(email => email)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToListAsync();
#pragma warning restore CA1304
        }
    }
}
