namespace Pidp.Features.Credentials;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;

public class BCProviderInvitedEntraAccounts
{
    public class Query : IQuery<IDomainResult<List<Model>>>
    {
        public int PartyId { get; set; }
    }

    public class Model
    {
        public string InvitedUserPrincipalName { get; set; } = string.Empty;
        public string InvitedAt { get; set; } = string.Empty;
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.PartyId).GreaterThan(0);
    }

    public class QueryHandler(PidpDbContext context) : IQueryHandler<Query, IDomainResult<List<Model>>>
    {
        private readonly PidpDbContext context = context;

        public async Task<IDomainResult<List<Model>>> HandleAsync(Query query)
        {
            var invitedAccounts = await this.context.InvitedEntraAccounts
                .Where(account => account.PartyId == query.PartyId)
                .OrderByDescending(account => account.InvitedAt)
                .Select(account => new Model
                {
                    InvitedUserPrincipalName = account.InvitedUserPrincipalName,
                    InvitedAt = account.InvitedAt.ToString()
                })
                .ToListAsync();

            if (invitedAccounts == null || invitedAccounts.Count == 0)
            {
                return DomainResult.NotFound<List<Model>>();
            }
            else
            {
                return DomainResult.Success(invitedAccounts);
            }
        }
    }
}
