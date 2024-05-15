namespace Pidp.Infrastructure.HttpClients;

using DomainResults.Common;
using Flurl;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using Pidp.Extensions;

public enum PropertySerialization
{
    CamelCase
}

public class BaseClient
{
    private readonly HttpClient client;
    private readonly JsonSerializerOptions serializationOptions;

    protected ILogger<BaseClient> Logger { get; }

    /// <summary>
    /// An HttpClient with default serialization options (camelCase)
    /// </summary>
    /// <param name="client"></param>
    /// <param name="logger"></param>
    public BaseClient(HttpClient client, ILogger<BaseClient> logger) : this(client, logger, PropertySerialization.CamelCase) { }

    public BaseClient(HttpClient client, ILogger<BaseClient> logger, PropertySerialization option)
    {
        client.ThrowIfNull(nameof(client));
        this.client = client;
        this.Logger = logger;

        this.serializationOptions = option switch
        {
            PropertySerialization.CamelCase => new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase },
            _ => throw new NotImplementedException($"{option}")
        };
    }

    /// <summary>
    /// Creates JSON StringContent based on the serialization settings set in the constructor
    /// </summary>
    /// <param name="data"></param>
    protected StringContent CreateStringContent(object data) => new(JsonSerializer.Serialize(data, this.serializationOptions), Encoding.UTF8, "application/json");

    protected async Task<IDomainResult> DeleteAsync(string url, object? data = null) => await this.SendCoreAsync(HttpMethod.Delete, url, data == null ? null : this.CreateStringContent(data), default);

    protected async Task<IDomainResult<T>> GetAsync<T>(string url) => await this.SendCoreAsync<T>(HttpMethod.Get, url, null, default);

    /// <summary>
    /// Performs a GET to the supplied Url with name/value pairs as query values parsed from a values object.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="queryValues">Typically an anonymous object, ie: new { x = 1, y = 2 }</param>
    /// <param name="nullValueHandling">Indicates how to handle null values. Defaults to Remove (any existing)</param>
    protected async Task<IDomainResult<T>> GetWithQueryParamsAsync<T>(string url, object queryValues, NullValueHandling nullValueHandling = NullValueHandling.Remove) => await this.SendCoreAsync<T>(HttpMethod.Get, url.SetQueryParams(queryValues, nullValueHandling), null, default);

    /// <summary>
    /// Performs a POST to the supplied Url with an optional JSON StringContent body as per the serialization settings set in the constructor
    /// </summary>
    /// <param name="url"></param>
    /// <param name="data"></param>
    protected async Task<IDomainResult> PostAsync(string url, object? data = null) => await this.SendCoreAsync(HttpMethod.Post, url, data == null ? null : this.CreateStringContent(data), default);

    /// <summary>
    /// Performs a POST to the supplied Url with an optional JSON StringContent body as per the serialization settings set in the constructor.
    /// Produces a Success result with a (non-null) value of the indicated type, or a Failure result in the case of errors, non-success status codes, or a missing/null response value.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="data"></param>
    protected async Task<IDomainResult<T>> PostAsync<T>(string url, object? data = null) => await this.SendCoreAsync<T>(HttpMethod.Post, url, data == null ? null : this.CreateStringContent(data), default);

    /// <summary>
    /// Performs a POST to the supplied Url with an optional JSON StringContent body as per the serialization settings set in the constructor.
    /// Produces a DomainResult and the value of the Location response header.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="data"></param>
    protected async Task<(IDomainResult Status, Uri? Location)> PostWithLocationAsync(string url, object? data = null)
    {
        var (status, headers) = await this.SendCoreWithHeadersAsync(HttpMethod.Post, url, data == null ? null : this.CreateStringContent(data), default);
        return (status, headers?.Location);
    }

    /// <summary>
    /// Performs a PUT to the supplied Url with an optional JSON StringContent body as per the serialization settings set in the constructor
    /// </summary>
    /// <param name="url"></param>
    /// <param name="data"></param>
    protected async Task<IDomainResult> PutAsync(string url, object? data = null) => await this.SendCoreAsync(HttpMethod.Put, url, data == null ? null : this.CreateStringContent(data), default);

    /// <summary>
    /// Sends an HTTP message to the API; returning:
    ///  a) a Success result, or
    ///  b) a Failure result in the case of errors or a non-success status code
    /// </summary>
    /// <param name="method"></param>
    /// <param name="url"></param>
    /// <param name="content"></param>
    /// <param name="cancellationToken"></param>
    protected async Task<IDomainResult> SendCoreAsync(HttpMethod method, string url, HttpContent? content, CancellationToken cancellationToken)
    {
        var (status, _) = await this.SendCoreInternalAsync<object>(method, url, content, true, cancellationToken);
        status.Deconstruct(out _, out var details);
        return details;
    }

    /// <summary>
    /// Send an HTTP message to the API; returning:
    ///  a) a Success result with a (non-null) value of the indicated type, or
    ///  b) a Failure result in the case of errors, non-success status codes, or a missing/null response value.
    /// </summary>
    /// <typeparam name="T">Type of the API's Response Content.</typeparam>
    /// <param name="method"></param>
    /// <param name="url"></param>
    /// <param name="content"></param>
    /// <param name="cancellationToken"></param>
    protected async Task<IDomainResult<T>> SendCoreAsync<T>(HttpMethod method, string url, HttpContent? content, CancellationToken cancellationToken) => (await this.SendCoreInternalAsync<T>(method, url, content, false, cancellationToken)).Status;

    /// <summary>
    /// Sends an HTTP message to the API; returning the response headers as well as:
    ///  a) a Success result, or
    ///  b) a Failure result in the case of errors or a non-success status code
    /// </summary>
    /// <param name="method"></param>
    /// <param name="url"></param>
    /// <param name="content"></param>
    /// <param name="cancellationToken"></param>
    protected async Task<(IDomainResult Status, HttpResponseHeaders? Headers)> SendCoreWithHeadersAsync(HttpMethod method, string url, HttpContent? content, CancellationToken cancellationToken)
    {
        var (status, headers) = await this.SendCoreInternalAsync<object>(method, url, content, true, cancellationToken);
        status.Deconstruct(out _, out var details);
        return (details, headers);
    }

    private async Task<(IDomainResult<T> Status, HttpResponseHeaders? Headers)> SendCoreInternalAsync<T>(HttpMethod method, string url, HttpContent? content, bool ignoreResponseContent, CancellationToken cancellationToken)
    {
        try
        {
            using var request = new HttpRequestMessage(method, url)
            {
                Content = content
            };
            using var response = await this.client.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                // TODO: retryable status codes?
                // if (RetryableStatusCodes.Contains(response.StatusCode))
                // {
                //     ???
                // }

                var responseMessage = response.Content != null
                    ? await response.Content.ReadAsStringAsync(cancellationToken)
                    : "";

                this.Logger.LogNonSuccessStatusCode(response.StatusCode, responseMessage);
                return (DomainResult.Failed<T>(response.StatusCode == HttpStatusCode.NotFound
                    ? $"The URL {url} was not found"
                    : "Did not receive a successful status code"), response.Headers);
            }

            if (ignoreResponseContent)
            {
                return (DomainResult.Success<T>(default!), response.Headers);
            }

            if (response.Content == null)
            {
                this.Logger.LogNullResponseContent();
                return (DomainResult.Failed<T>("Response content was null"), response.Headers);
            }

            var deserializationResult = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
            if (deserializationResult == null)
            {
                this.Logger.LogNullResponseContent();
                return (DomainResult.Failed<T>("Response content was null"), response.Headers);
            }

            return (DomainResult.Success(deserializationResult), response.Headers);
        }
        catch (HttpRequestException exception)
        {
            this.Logger.LogBaseClientException(exception);
            return (DomainResult.Failed<T>("HttpRequestException during call to API"), null);
        }
        catch (TimeoutException exception)
        {
            this.Logger.LogBaseClientException(exception);
            return (DomainResult.Failed<T>("TimeoutException during call to API"), null);
        }
        catch (OperationCanceledException exception)
        {
            this.Logger.LogBaseClientException(exception);
            return (DomainResult.Failed<T>("Task was canceled during call to API"), null);
        }
        catch (JsonException exception)
        {
            this.Logger.LogBaseClientException(exception);
            return (DomainResult.Failed<T>("Could not deserialize API response"), null);
        }
        catch (Exception exception)
        {
            this.Logger.LogBaseClientException(exception);
            return (DomainResult.Failed<T>("Unhandled exception when calling the API"), null);
        }
    }
}

public static partial class BaseClientLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Received non-success status code {statusCode} with message: {responseMessage}.")]
    public static partial void LogNonSuccessStatusCode(this ILogger<BaseClient> logger, HttpStatusCode statusCode, string responseMessage);

    [LoggerMessage(2, LogLevel.Error, "Response content was null.")]
    public static partial void LogNullResponseContent(this ILogger<BaseClient> logger);

    [LoggerMessage(3, LogLevel.Error, "Unhandled exception when calling the API.")]
    public static partial void LogBaseClientException(this ILogger<BaseClient> logger, Exception e);
}
