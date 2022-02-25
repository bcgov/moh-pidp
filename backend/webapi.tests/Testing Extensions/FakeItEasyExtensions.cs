namespace PidpTests.TestingExtensions;

using FakeItEasy;
using FakeItEasy.Configuration;

public static class FakeItEasyExtensions
{
    public static IReturnValueConfiguration<Task<HttpResponseMessage>> InvokingSendAsyncWith(this IWhereConfiguration<IAnyCallConfigurationWithNoReturnTypeSpecified> configuration, HttpMethod method, string url, HttpContent? content)
    {
        return configuration
            .Where(x => x.Method.Name == "SendAsync")
            .WithReturnType<Task<HttpResponseMessage>>()
            .WhenArgumentsMatch((HttpRequestMessage message, CancellationToken token) => message.Method == method
                && message.RequestUri == new Uri(url)
                && message.Content == content);
    }
}
