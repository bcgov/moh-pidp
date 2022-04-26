namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Models;

public class Index
{
    public class Query : IQuery<IDomainResult<List<Model>>>
    {
        public int PartyId { get; set; }
    }

    public class Model
    {
        public int Id { get; set; }
        public int PartyId { get; set; }
        public AccessType AccessType { get; set; }
        public Instant RequestedOn { get; set; }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.PartyId).GreaterThan(0);
    }

    public class QueryHandler : IQueryHandler<Query, IDomainResult<List<Model>>>
    {
        private readonly PidpDbContext context;

        public QueryHandler(PidpDbContext context) => this.context = context;

        public async Task<IDomainResult<List<Model>>> HandleAsync(Query query)
        {
            var accessRequests = await this.context.AccessRequests
                .Where(access => access.PartyId == query.PartyId)
                .OrderByDescending(access => access.RequestedOn)
                .Select(access => new Model
                {
                    Id = access.Id,
                    PartyId = access.PartyId,
                    AccessType = access.AccessType,
                    RequestedOn = access.RequestedOn,
                })
                .ToListAsync();

            return DomainResult.Success(accessRequests);
        }
    }
}
