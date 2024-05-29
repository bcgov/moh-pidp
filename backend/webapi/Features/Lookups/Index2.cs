namespace Pidp.Features.Lookups;

using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Models.Lookups;

public class Index2
{
    public class Query : IQuery<Model> { }

    public class Model
    {
        public List<AccessType> AccessTypes { get; set; } = new();
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
            var sw = System.Diagnostics.Stopwatch.StartNew();


            var model = new Model
            {
                AccessTypes = await this.context.Set<AccessType>()
                    .AsNoTracking()
                    .ToListAsync(),
                Colleges = await this.context.Set<College>()
                    .AsNoTracking()
                    .ToListAsync(),
                Countries = await this.context.Set<Country>()
                    .AsNoTracking()
                    .ToListAsync(),
                Provinces = await this.context.Set<Province>()
                    .AsNoTracking()
                    .ToListAsync()
            };
            sw.Stop();
            Console.WriteLine($"Index2.QueryHandler.HandleAsync: {sw.ElapsedMilliseconds}ms");

            return model;
        }
    }
}
