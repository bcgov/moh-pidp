namespace Pidp.Features.BCProvider;

using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class BCProviderDetails
{
    public class Query : IQuery<Model>
    {
        public int PartyId { get; set; }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.PartyId).GreaterThan(0);
    }

    public class Model
    {
        public string Username { get; set; } = string.Empty;
    }

    public class QueryHandler : IQueryHandler<Query, Model>
    {
        public Task<Model> HandleAsync(Query query)
        {
            var result = new Model
            {
                Username = "example@bcproviderlab.ca"
            };
            return Task.FromResult(result);
        }
    }
}
