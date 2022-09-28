#pragma warning disable CA1304 // ToLower() is Locale Dependant
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

        public async Task<IDomainResult> HandleAsync(Query query)
        {
            return DomainResult.NotFound();
            // return await this.context.Parties
            //     .Where(party => party.Email != null)
            //     .Select(party => party.Email!.Substring(party.Email.IndexOf("@") + 1).ToLower())
            //     .GroupBy(email => email)
            //     .Where(group => group.Count() > 1)
            //     .Select(group => group.Key)
            //     .ToListAsync();
        }
    }
}

#pragma warning restore CA1304
