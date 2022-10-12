namespace Pidp.Features.Lookups;

using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Models.Lookups;

public class Index
{
    public class Query : IQuery<Model> { }

    public class Model
    {
        public List<AccessType> AccessTypes { get; set; } = new();
        public List<College> Colleges { get; set; } = new();
        public List<Country> Countries { get; set; } = new();
        public List<Province> Provinces { get; set; } = new();
        public List<Organization> Organizations { get; set; } = new();
        public List<HealthAuthority> HealthAuthorities { get; set; } = new();
    }

    public class QueryHandler : IQueryHandler<Query, Model>
    {
        private readonly PidpDbContext context;

        public QueryHandler(PidpDbContext context) => this.context = context;

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
                Organizations = await this.context.Set<Organization>()
                    .ToListAsync(),
                HealthAuthorities = await this.context.Set<HealthAuthority>()
                    .ToListAsync()
            };
        }
    }
}
