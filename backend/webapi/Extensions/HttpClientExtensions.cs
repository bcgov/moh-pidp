namespace Pidp.Extensions;

using Flurl;

public static class HttpClientExtensions
{
    /// <summary>
    /// Performs a GET to the supplied Uri with name/value pairs as query values parsed from a values object.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="requestUri"></param>
    /// <param name="queryValues">Typically an anonymous object, ie: new { x = 1, y = 2 }</param>
    /// <param name="nullValueHandling">Indicates how to handle null values. Defaults to Remove (any existing)</param>
    public static Task<HttpResponseMessage> GetWithQueryParamsAsync(this HttpClient client, string requestUri, object queryValues, NullValueHandling nullValueHandling = NullValueHandling.Remove)
        => client.GetAsync(requestUri.SetQueryParams(queryValues, nullValueHandling));
}
