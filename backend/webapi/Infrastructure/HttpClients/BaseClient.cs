namespace Pidp.Infrastructure.HttpClients;

using System;
using System.Text;
using System.Text.Json;
using System.Net.Http;

using Pidp.Extensions;
using Pidp.Features;
using DomainResults.Common;
using System.Net;

public enum PropertySerialization
{
    CamelCase
}

public class BaseClient
{
    private readonly JsonSerializerOptions serializationOptions;

    protected HttpClient Client { get; }

    protected ILogger Logger { get; }

    /// <summary>
    /// An HttpClient with default serialization options (camelCase)
    /// </summary>
    /// <param name="client"></param>
    /// <param name="logger"></param>
    public BaseClient(HttpClient client, ILogger logger) : this(client, logger, PropertySerialization.CamelCase) { }

    public BaseClient(HttpClient client, ILogger logger, PropertySerialization option)
    {
        client.ThrowIfNull(nameof(client));
        this.Client = client;
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
    public StringContent CreateStringContent(object data) => new(JsonSerializer.Serialize(data, this.serializationOptions), Encoding.UTF8, "application/json");

    /// <summary>
    /// Sends an HTTP message to the API; returning:
    ///  a) a Success result, or
    ///  b) a Failure result in the case of errors or a non-success status code
    /// </summary>
    /// <param name="method"></param>
    /// <param name="url"></param>
    /// <param name="cancellationToken"></param>
    protected async Task<IDomainResult> SendCoreAsync(HttpMethod method, string url, CancellationToken cancellationToken)
    {
        var result = await this.SendCoreInternalAsync(method, url, cancellationToken);
        result.Deconstruct(out _, out var details);
        return details;
    }

    /// <summary>
    /// Send an HTTTP message to the API; returning:
    ///  a) a Success result with a (non-null) value of the indicated type, or
    ///  b) a Failure result in the case of errors, non-success status codes, or a missing/null response value.
    /// </summary>
    /// <typeparam name="T">Type of the API's Response Content.</typeparam>
    /// <param name="method"></param>
    /// <param name="url"></param>
    /// <param name="cancellationToken"></param>
    protected async Task<IDomainResult<T>> SendCoreAsync<T>(HttpMethod method, string url, CancellationToken cancellationToken)
    {
        var result = await this.SendCoreInternalAsync(method, url, cancellationToken);
        if (!result.IsSuccess)
        {
            return result.To<T>();
        }

        var responseContent = result.Value;

        if (responseContent == null)
        {
            this.Logger.LogError("Response content was null");
            return DomainResult.Failed<T>("Response content was null");
        }

        try
        {
            var deserializationResult = await responseContent.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
            if (deserializationResult == null)
            {
                this.Logger.LogError("Response content was null");
                return DomainResult.Failed<T>("Response content was null");
            }

            return DomainResult.Success(deserializationResult);
        }
        catch (JsonException exception)
        {
            this.Logger.LogError(exception, "Error when deserializaing response body after calling the API");
            return DomainResult.Failed<T>("Could not deserialize API response");
        }
    }

    private async Task<IDomainResult<HttpContent>> SendCoreInternalAsync(HttpMethod method, string url, CancellationToken cancellationToken)
    {
        try
        {
            using var response = await this.Client.SendAsync(new HttpRequestMessage(method, url), cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                // TODO: retryable status codes?
                // if (RetryableStatusCodes.Contains(response.StatusCode))
                // {
                //     return DomainResult.Failed<T>($"Received retryable status code '{response.StatusCode}'");
                // }

                this.Logger.LogError($"Recieved non-success status code {response.StatusCode}.");
                return DomainResult.Failed<HttpContent>(response.StatusCode == HttpStatusCode.NotFound
                    ? $"The URL {url} was not found"
                    : "Did not receive a successful status code");
            }

            return DomainResult.Success(response.Content);
        }
        catch (HttpRequestException exception)
        {
            this.Logger.LogError(exception, "HttpRequestException when calling the API");
            return DomainResult.Failed<HttpContent>("HttpRequestException during call to API");
        }
        catch (TimeoutException exception)
        {
            this.Logger.LogError(exception, "TimeoutException during call to API");
            return DomainResult.Failed<HttpContent>("TimeoutException during call to API");
        }
        catch (OperationCanceledException exception)
        {
            this.Logger.LogError(exception, "Task was canceled during call to API");
            return DomainResult.Failed<HttpContent>("Task was canceled during call to API");
        }
        catch (Exception exception)
        {
            this.Logger.LogError(exception, "Unhandled exception when calling the API");
            return DomainResult.Failed<HttpContent>("Unhandled exception when calling the API");
        }
    }
}
