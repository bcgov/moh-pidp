namespace Pidp.Features.Admin;

using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    }

    public class QueryHandler : IQueryHandler<Query, List<Model>>
    {
        private readonly IMapper mapper;
        private readonly PidpDbContext context;

        public QueryHandler(IMapper mapper, PidpDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<List<Model>> HandleAsync(Query query)
        {
            return await this.context.Parties
                .ProjectTo<Model>(this.mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
