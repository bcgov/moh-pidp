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

    public class QueryHandler : IQueryHandler<Query, IDomainResult>
    {
        private readonly PidpDbContext context;

        public QueryHandler(PidpDbContext context) => this.context = context;

#pragma warning disable CA1304 // ToLower() is Locale Dependant
        public async Task<IDomainResult> HandleAsync(Query query)
        {
            // If the input Email is just the domain, this should return the entire input.
            var domain = query.Email[(query.Email.IndexOf("@") + 1)..].ToLower();

            var count = await this.context.Parties
                .Where(party => party.Email!.Substring(party.Email.IndexOf("@") + 1).ToLower() == domain)
                .CountAsync();

            if (count > 1)
            {
                return DomainResult.Success();
            }
            else
            {
                return DomainResult.NotFound();
            }
        }
#pragma warning restore CA1304
    }
}

