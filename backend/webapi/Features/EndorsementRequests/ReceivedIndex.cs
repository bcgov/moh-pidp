namespace Pidp.Features.EndorsementRequests;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;

public class ReceivedIndex
{
    public class Query : IQuery<List<Model>>
    {
        public int PartyId { get; set; }
    }

    public class Model
    {
        public int Id { get; set; }
        public string PartyName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public bool? Approved { get; set; }
        public Instant? AdjudicatedOn { get; set; }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.PartyId).GreaterThan(0);
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
            return await this.context.EndorsementRequests
                .Where(request => request.EndorsingPartyId == query.PartyId)
                .ProjectTo<Model>(this.mapper.ConfigurationProvider)
                .OrderBy(model => model.AdjudicatedOn.HasValue) // show unadjudicated results first
                    .ThenByDescending(model => model.AdjudicatedOn)
                .ToListAsync();
        }
    }
}
