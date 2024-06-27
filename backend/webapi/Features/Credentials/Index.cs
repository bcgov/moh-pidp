namespace Pidp.Features.Credentials;

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
        public int Id { get; set; }
        public string? IdentityProvider { get; set; }
        public string? IdpId { get; set; }
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
            return await this.context.Credentials
                .Where(credential => credential.PartyId == query.PartyId)
                .Select(credential => new Model
                {
                    Id = credential.Id,
                    IdentityProvider = credential.IdentityProvider,
                    IdpId = credential.IdpId
                })
                .ToListAsync();
        }
    }
}
