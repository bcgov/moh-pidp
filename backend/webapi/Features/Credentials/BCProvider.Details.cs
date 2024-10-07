namespace Pidp.Features.Credentials;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Infrastructure.Auth;

public class BCProviderDetails
{
    public class Query : IQuery<IDomainResult<Model>>
    {
        public int PartyId { get; set; }
    }

    public class Model
    {
        public string BCProviderId { get; set; } = string.Empty;
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.PartyId).GreaterThan(0);
    }

    public class QueryHandler(PidpDbContext context) : IQueryHandler<Query, IDomainResult<Model>>
    {
        private readonly PidpDbContext context = context;

        public async Task<IDomainResult<Model>> HandleAsync(Query query)
        {
            var bcProvider = await this.context.Credentials
                .Where(credential => credential.PartyId == query.PartyId
                    && credential.IdentityProvider == IdentityProviders.BCProvider)
                .Select(credential => new Model { BCProviderId = credential.IdpId! })
                .SingleOrDefaultAsync();

            if (bcProvider == null)
            {
                return DomainResult.NotFound<Model>();
            }
            else
            {
                return DomainResult.Success(bcProvider);
            }
        }
    }
}
