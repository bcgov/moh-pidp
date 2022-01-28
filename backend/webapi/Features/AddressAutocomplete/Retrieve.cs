namespace Pidp.Features.AddressAutocomplete;

using FluentValidation;

using Pidp.Infrastructure.HttpClients.AddressAutocomplete;

public class Retrieve
{
    public class Query : IQuery<List<AddressAutocompleteRetrieveResponse>>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.Id).NotEmpty();
    }

    public class QueryHandler : IQueryHandler<Query, List<AddressAutocompleteRetrieveResponse>>
    {
        private readonly IAddressAutocompleteClient client;

        public QueryHandler(IAddressAutocompleteClient client) => this.client = client;

        // TODO error response for when CanadaPost API is not available or times out
        public async Task<List<AddressAutocompleteRetrieveResponse>> HandleAsync(Query query) => (await this.client.Retrieve(query.Id)).ToList();
    }
}
