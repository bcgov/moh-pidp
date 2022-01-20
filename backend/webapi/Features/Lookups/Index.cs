namespace Pidp.Features.Lookups;

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using Pidp.Data;
using Pidp.Models.Lookups;

public class Index
{
    public class Query : IQuery<Model>
    {
    }

    public class Model
    {
        public List<College> Colleges { get; set; } = new();
        public List<Country> Countries { get; set; } = new();
        public List<Province> Provinces { get; set; } = new();
    }

    public class QueryHandler : IQueryHandler<Query, Model>
    {
        private readonly PidpDbContext context;

        public QueryHandler(PidpDbContext context) => this.context = context;

        public async Task<Model> HandleAsync(Query query)
        {
            return new Model
            {
                Colleges = await this.context.Set<College>()
                    .ToListAsync(),
                Countries = await this.context.Set<Country>()
                    .ToListAsync(),
                Provinces = await this.context.Set<Province>()
                    .ToListAsync()
            };
        }
    }
}
