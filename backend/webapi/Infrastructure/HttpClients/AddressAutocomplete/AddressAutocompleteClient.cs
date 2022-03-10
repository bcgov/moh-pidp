namespace Pidp.Infrastructure.HttpClients.AddressAutocomplete;

public class AddressAutocompleteClient : BaseClient, IAddressAutocompleteClient
{
    private readonly string apiKey;

    public AddressAutocompleteClient(
        HttpClient httpClient,
        ILogger<AddressAutocompleteClient> logger,
        PidpConfiguration config)
        : base(httpClient, logger) => this.apiKey = config.AddressAutocompleteClient.ApiKey;

    public async Task<IEnumerable<AddressAutocompleteFindResponse>> Find(string searchTerm)
    {
        var result = await this.GetWithQueryParamsAsync<AddressAutocompleteApiResponse<AddressAutocompleteFindResponse>>("Find/v2.10/json3ex.ws", new
        {
            // See documentation for all available fields
            Key = this.apiKey,
            SearchTerm = searchTerm,
        });

        if (!result.IsSuccess)
        {
            // this.Logger.LogError($"Error when retrieving AddressAutocompleteFindResponse results for SearchTerm = {searchTerm}. {message}");
            return Enumerable.Empty<AddressAutocompleteFindResponse>();
        }

        return result.Value.Items ?? Enumerable.Empty<AddressAutocompleteFindResponse>();
    }

    public async Task<IEnumerable<AddressAutocompleteRetrieveResponse>> Retrieve(string id)
    {
        var result = await this.GetWithQueryParamsAsync<AddressAutocompleteApiResponse<AddressAutocompleteRetrieveResponse>>("Retrieve/v2.11/json3ex.ws", new
        {
            // See documentation for all available fields
            Key = this.apiKey,
            Id = id,
        });

        if (!result.IsSuccess)
        {
            // this.Logger.LogError($"Error when retrieving AddressAutocompleteRetrieveResponse results for Id = {id}. {message}");
            return Enumerable.Empty<AddressAutocompleteRetrieveResponse>();
        }

        return result.Value.Items ?? Enumerable.Empty<AddressAutocompleteRetrieveResponse>();
    }
}
