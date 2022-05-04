namespace Pidp.Features.AccessRequests;

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Models;

public class Index
{
    public class Query : IQuery<List<Model>>
    {
        public int PartyId { get; set; }
    }

    public class Model
    {
        public int PartyId { get; set; }
        public AccessType AccessType { get; set; }
        public Instant RequestedOn { get; set; }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.PartyId).GreaterThan(0);
    }

    public class QueryHandler : IQueryHandler<Query, List<Model>>
    {
        private readonly PidpDbContext context;

        public QueryHandler(PidpDbContext context) => this.context = context;

        public async Task<List<Model>> HandleAsync(Query query)
        {
            return await this.context.AccessRequests
                .Where(access => access.PartyId == query.PartyId)
                .OrderByDescending(access => access.RequestedOn)
                .Select(access => new Model
                {
                    PartyId = access.PartyId,
                    AccessType = access.AccessType,
                    RequestedOn = access.RequestedOn,
                })
                .ToListAsync();
        }
    }
}
