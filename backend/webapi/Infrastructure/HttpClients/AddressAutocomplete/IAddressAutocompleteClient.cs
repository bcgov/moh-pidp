namespace Pidp.Infrastructure.HttpClients.AddressAutocomplete;

/// <summary>
/// Address autocomplete client for CanadaPost for more information on the API:
/// https://www.canadapost-postescanada.ca/ac/support/api/
/// </summary>
public interface IAddressAutocompleteClient
{
    /// <summary>
    /// Returns addresses matching the search term.
    /// https://www.canadapost-postescanada.ca/ac/support/api/addresscomplete-interactive-find/
    /// </summary>
    /// <param name="searchTerm"></param>
    Task<IEnumerable<AddressAutocompleteFindResponse>> Find(string searchTerm);

    /// <summary>
    /// Returns the full address details based on the Id.
    /// https://www.canadapost-postescanada.ca/ac/support/api/addresscomplete-interactive-retrieve/
    /// </summary>
    /// <param name="id"></param>
    Task<IEnumerable<AddressAutocompleteRetrieveResponse>> Retrieve(string id);
}
