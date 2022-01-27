namespace Pidp.Infrastructure.HttpClients.AddressAutocomplete;

using Pidp.Extensions;

public class AddressAutocompleteClient : BaseClient, IAddressAutocompleteClient
{
    private readonly string apiKey;

    public AddressAutocompleteClient(HttpClient httpClient, ILogger<AddressAutocompleteClient> logger, PidpConfiguration config) : base(httpClient, logger) => this.apiKey = config.AddressAutocompleteClient.ApiKey;

    public async Task<IEnumerable<AddressAutocompleteFindResponse>> Find(string searchTerm)
    {
        var response = await this.Client.GetWithQueryParamsAsync("Find/v2.10/json3ex.ws", new
        {
            // See documentation for all available fields
            Key = this.apiKey,
            SearchTerm = searchTerm,
        });

        if (!response.IsSuccessStatusCode)
        {
            // See documentation for error details
            this.Logger.LogError($"Error when retrieving AddressAutocompleteFindResponse results for SearchTerm = {searchTerm}.", await response.Content.ReadAsStringAsync());
            return Enumerable.Empty<AddressAutocompleteFindResponse>();
        }

        var body = await response.Content.ReadFromJsonAsync<AddressAutocompleteApiResponse<AddressAutocompleteFindResponse>>();
        return body?.Items ?? Enumerable.Empty<AddressAutocompleteFindResponse>();
    }

    public async Task<IEnumerable<AddressAutocompleteRetrieveResponse>> Retrieve(string id)
    {
        var response = await this.Client.GetWithQueryParamsAsync("Retrieve/v2.11/json3ex.ws", new
        {
            // See documentation for all available fields
            Key = this.apiKey,
            Id = id,
        });

        if (!response.IsSuccessStatusCode)
        {
            // See documentation for error details
            this.Logger.LogError($"Error when retrieving AddressAutocompleteRetrieveResponse results for Id = {id}.", await response.Content.ReadAsStringAsync());
            return Enumerable.Empty<AddressAutocompleteRetrieveResponse>();
        }

        var body = await response.Content.ReadFromJsonAsync<AddressAutocompleteApiResponse<AddressAutocompleteRetrieveResponse>>();
        return body?.Items ?? Enumerable.Empty<AddressAutocompleteRetrieveResponse>();
    }
}
