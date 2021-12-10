namespace Pidp.Features.Parties;

using Microsoft.EntityFrameworkCore;

using Pidp.Data;

public class Index
{
    public class Query : IQuery<Response>
    {
    }

    public class Response
    {
        public List<Model> Results { get; set; } = new();

        public class Model
        {
            public string FullName { get; set; } = string.Empty;
        }
    }

    public class IndexQueryHandler : IQueryHandler<Query, Response>
    {
        private readonly PidpDbContext context;

        public IndexQueryHandler(PidpDbContext context) => this.context = context;

        public async Task<Response> HandleAsync(Query query)
        {
            return new Response
            {
                Results = await this.context.Parties
                    .Select(party => new Response.Model
                    {
                        FullName = $"{party.FirstName} {party.LastName}"
                    })
                    .ToListAsync()
            };
        }
    }
}
