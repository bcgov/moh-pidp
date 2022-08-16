namespace PidpTests.TestingExtensions;

using FakeItEasy;
using FakeItEasy.Configuration;
using System.Net;
using System.Text.Json;

using Pidp.Infrastructure.HttpClients.Plr;

public static class FakeItEasyExtensions
{
    public static IReturnValueConfiguration<Task<HttpResponseMessage>> InvokingSendAsyncWith(this IWhereConfiguration<IAnyCallConfigurationWithNoReturnTypeSpecified> configuration, HttpMethod method, string url, HttpContent? content = null)
    {
        return configuration
            .Where(x => x.Method.Name == "SendAsync")
            .WithReturnType<Task<HttpResponseMessage>>()
            .WhenArgumentsMatch((HttpRequestMessage message, CancellationToken token) => message.Method == method
                && message.RequestUri == new Uri(url)
                && (content == null
                    || content.Equals(message.Content)));
    }

    public static IReturnValueConfiguration<Task<HttpResponseMessage>> InvokingSendAsyncWithAnything(this IWhereConfiguration<IAnyCallConfigurationWithNoReturnTypeSpecified> configuration)
    {
        return configuration
            .Where(x => x.Method.Name == "SendAsync")
            .WithReturnType<Task<HttpResponseMessage>>();
    }

    public static IAfterCallConfiguredWithOutAndRefParametersConfiguration<IReturnValueConfiguration<Task<HttpResponseMessage>>> ReturnsAMessageWith(this IReturnValueConfiguration<Task<HttpResponseMessage>> configuration, HttpStatusCode code, object? asStringContent = null)
    {
        var content = asStringContent == null
            ? null
            : new StringContent(JsonSerializer.Serialize(asStringContent), System.Text.Encoding.UTF8, "application/json");
        return configuration
            .Returns(Task.FromResult(new HttpResponseMessage(code) { Content = content }));
    }

    public static IPlrClient ReturningAStatandingsDigest(this IPlrClient client, bool goodStanding, string? identifierType = null)
        => client.ReturningAStatandingsDigest(AMock.StandingsDigest(goodStanding, identifierType));

    public static IPlrClient ReturningAStatandingsDigest(this IPlrClient client, PlrStandingsDigest digest)
    {
        A.CallTo(() => client.GetStandingAsync(A<string?>._)).Returns(digest.HasGoodStanding);
        A.CallTo(() => client.GetStandingsDigestAsync(A<string?>._)).Returns(digest);

        return client;
    }
}
