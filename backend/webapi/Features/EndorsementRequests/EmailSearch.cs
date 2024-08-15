namespace Pidp.Features.EndorsementRequests;

using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;


public class EmailSearch
{
    public class Query : IQuery<Model>
    {
        public string RecipientEmail { get; set; } = string.Empty;
    }

    public class Model
    {
        public string? RecipientName { get; set; }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.RecipientEmail).NotEmpty().EmailAddress();
    }

    public class QueryHandler(PidpDbContext context) : IQueryHandler<Query, Model>
    {
        private readonly PidpDbContext context = context;

        public async Task<Model> HandleAsync(Query query)
        {
            var existingParties = await this.context.Parties
                .Where(party => party.Email.ToLower() == query.RecipientEmail.ToLower())
                .Select(party => party.DisplayFullName)
                .ToListAsync();

            if (existingParties.Count == 1)
            {
                return new Model { RecipientName = existingParties.Single() };
            }
            else
            {
                return new();
            }
        }
    }
}
