namespace Pidp.Features.Lookups;

using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Models.Lookups;

public class Index
{
    public class Query : IQuery<Model> { }

    public class Model
    {
        public List<AccessType> AccessTypes { get; set; } = [];
        public List<College> Colleges { get; set; } = [];
        public List<Country> Countries { get; set; } = [];
        public List<Province> Provinces { get; set; } = [];
        public string Version { get; } = "2024-10-31:1";
    }

    public class QueryHandler(PidpDbContext context) : IQueryHandler<Query, Model>
    {
        private readonly PidpDbContext context = context;

        public async Task<Model> HandleAsync(Query query)
        {
            return new Model
            {
                AccessTypes = await this.context.Set<AccessType>()
                    .ToListAsync(),
                Colleges = await this.context.Set<College>()
                    .ToListAsync(),
                Countries = await this.context.Set<Country>()
                    .ToListAsync(),
                Provinces = await this.context.Set<Province>()
                    .ToListAsync(),
            };
        }
    }
}
