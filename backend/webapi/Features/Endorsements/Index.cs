namespace Pidp.Features.Endorsements;
using Pidp.Models.Lookups;

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Extensions;

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
        public CollegeCode? CollegeCode { get; set; }
        public Instant CreatedOn { get; set; }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.PartyId).GreaterThan(0);
    }

    public class QueryHandler(PidpDbContext context) : IQueryHandler<Query, List<Model>>
    {
        private readonly PidpDbContext context = context;

        public async Task<List<Model>> HandleAsync(Query query)
        {
            return await this.context.ActiveEndorsementRelationships(query.PartyId)
                .Select(relationship => new Model
                {
                    Id = relationship.EndorsementId,
                    PartyName = relationship.Party!.FullName,
                    CollegeCode = relationship.Party.LicenceDeclaration!.CollegeCode,
                    CreatedOn = relationship.Endorsement!.CreatedOn
                })
                .ToListAsync();
        }
    }
}
