namespace Pidp.Features.Admin;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;

public class Index
{
    public class Query : IQuery<List<Model>> { }

    public class Model
    {
        public int Id { get; set; }
        public string? ProviderName { get; set; }
        public string? ProviderCollege { get; set; }
    }

    public class QueryHandler : IQueryHandler<Query, List<Model>>
    {
        private readonly PidpDbContext context;
        private readonly IMapper mapper;

        public QueryHandler(PidpDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<Model>> HandleAsync(Query query)
        {
            return await this.context.Parties
                .ProjectTo<Model>(this.mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
