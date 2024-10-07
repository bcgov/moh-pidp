namespace Pidp.Features.AddressAutocomplete;

using FluentValidation;

using Pidp.Infrastructure.HttpClients.AddressAutocomplete;

public class Find
{
    public class Query : IQuery<List<AddressAutocompleteFindResponse>>
    {
        public string SearchTerm { get; set; } = string.Empty;
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.SearchTerm).NotEmpty();
    }

    public class QueryHandler(IAddressAutocompleteClient client) : IQueryHandler<Query, List<AddressAutocompleteFindResponse>>
    {
        private readonly IAddressAutocompleteClient client = client;

        // TODO error response for when CanadaPost API is not available or times out
        public async Task<List<AddressAutocompleteFindResponse>> HandleAsync(Query query) => (await this.client.Find(query.SearchTerm)).ToList();
    }
}
