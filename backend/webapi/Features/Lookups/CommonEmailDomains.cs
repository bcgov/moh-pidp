namespace Pidp.Features.Lookups;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;

public class CommonEmailDomains
{
    public class Query : IQuery<IDomainResult>
    {
        public string Email { get; set; } = string.Empty;
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.Email).NotEmpty();
    }

    public class QueryHandler(PidpDbContext context) : IQueryHandler<Query, IDomainResult>
    {
        private readonly PidpDbContext context = context;

        public async Task<IDomainResult> HandleAsync(Query query)
        {
            // If the input Email is just the domain, this should use the entire input.
            var domain = query.Email[(query.Email.IndexOf("@", StringComparison.OrdinalIgnoreCase) + 1)..].ToLowerInvariant();

#pragma warning disable CA1304 // ToLower() is Locale Dependant
            var count = await this.context.Parties
                .Where(party => party.Email!.Substring(party.Email.IndexOf("@") + 1).ToLower() == domain)
                .CountAsync();
#pragma warning restore CA1304

            if (count > 1)
            {
                return DomainResult.Success();
            }
            else
            {
                return DomainResult.NotFound();
            }
        }
    }
}
