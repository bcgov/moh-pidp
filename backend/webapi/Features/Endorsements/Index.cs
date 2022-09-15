namespace Pidp.Features.Endorsements;

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;

public class Index
{
    public class Query : IQuery<List<Model>>
    {
        public int PartyId { get; set; }
    }

    public class Model
    {
        public int Id { get; set; }
        public string PartyName { get; set; } = string.Empty;
        public bool Active { get; set; }
        public Instant CreatedOn { get; set; }
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
            return await this.context.EndorsementRelationships
                .Where(relationship => relationship.PartyId == query.PartyId)
                .SelectMany(relationship => relationship.Endorsement!.EndorsementRelationships)
                .Where(relationship => relationship.PartyId != query.PartyId)
                .Select(relationship => new Model
                {
                    Id = relationship.EndorsementId,
                    PartyName = $"{relationship.Party!.FirstName} {relationship.Party.LastName}",
                    Active = relationship.Endorsement!.Active,
                    CreatedOn = relationship.Endorsement.CreatedOn
                })
                .ToListAsync();
        }
    }
}
