namespace PidpTests.Extensions;

using Microsoft.AspNetCore.Http;
using Xunit;

using Pidp.Extensions;

public class HttpResponseExtensionsTests
{
    public const string HeaderKey = "Some-Header";
    public const string HeaderValue = "SomeValue";

    [Fact]
    public void SafeAddHeaders_HappyPath_HeaderIsAdded()
    {
        var response = new DefaultHttpContext().Response;

        var success = response.SafeAddHeader(HeaderKey, HeaderValue);

        Assert.True(success);
        AssertSingleHeader(HeaderKey, HeaderValue, response.Headers);
    }

    [Fact]
    public void SafeAddHeaders_KeyNull_NoHeaderIsAdded()
    {
        var response = new DefaultHttpContext().Response;

        var success = response.SafeAddHeader(null, HeaderValue);

        Assert.False(success);
        Assert.Empty(response.Headers);
    }

    [Fact]
    public void SafeAddHeaders_ValueNull_NoHeaderIsAdded()
    {
        var response = new DefaultHttpContext().Response;

        var success = response.SafeAddHeader(HeaderKey, null);

        Assert.False(success);
        Assert.Empty(response.Headers);
    }

    [Fact]
    public void SafeAddHeaders_KeyExists_HeaderIsUnchanged()
    {
        var response = new DefaultHttpContext().Response;
        response.Headers.Add(HeaderKey, HeaderValue);

        var success = response.SafeAddHeader(HeaderKey, "A NEW VALUE");

        Assert.False(success);
        AssertSingleHeader(HeaderKey, HeaderValue, response.Headers);
    }

    private static void AssertSingleHeader(string expectedHeaderKey, string expectedHeadervalue, IHeaderDictionary headers)
    {
        Assert.Single(headers);

        var header = headers.Single();

        Assert.Equal(expectedHeaderKey, header.Key);
        Assert.Equal(expectedHeadervalue, header.Value);
    }
}
