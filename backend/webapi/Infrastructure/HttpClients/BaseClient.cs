namespace Pidp.Infrastructure.HttpClients;

using System;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using Pidp.Extensions;

public enum PropertySerialization
{
    CamelCase
}

public class BaseClient
{
    private readonly JsonSerializerOptions serializationOptions;

    protected HttpClient Client { get; }

    public BaseClient(HttpClient client, PropertySerialization option)
    {
        client.ThrowIfNull(nameof(client));
        this.Client = client;

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
}
