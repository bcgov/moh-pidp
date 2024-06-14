namespace Pidp.Infrastructure.HttpClients.AddressAutocomplete;

public class AddressAutocompleteClient(
    HttpClient httpClient,
    ILogger<AddressAutocompleteClient> logger,
    PidpConfiguration config) : BaseClient(httpClient, logger), IAddressAutocompleteClient
{
    private readonly string apiKey = config.AddressAutocompleteClient.ApiKey;

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
            return [];
        }

        return result.Value.Items ?? [];
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
            return [];
        }

        return result.Value.Items ?? [];
    }
}
