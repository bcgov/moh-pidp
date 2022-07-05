namespace Pidp.Features.EndorsementRequests;

using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;

public class Index
{
    public class Query : IQuery<List<Model>>
    {
        public int PartyId { get; set; }
    }

    public class Model
    {
        public string RecipientEmail { get; set; } = string.Empty;
        public bool? Approved { get; set; }
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
            return await this.context.EndorsementRequests
                .Where(request => request.RequestingPartyId == query.PartyId)
                .Select(request => new Model
                {
                    RecipientEmail = request.RecipientEmail,
                    Approved = request.Approved
                })
                .ToListAsync();
        }
    }
}
