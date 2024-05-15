namespace PidpTests.Infrastructure.HttpClients;

using FakeItEasy;
using Microsoft.Extensions.Logging;
using System.Net;
using Xunit;

using Pidp.Infrastructure.HttpClients;
using Pidp.Models;
using Pidp.Models.Lookups;
using PidpTests.TestingExtensions;

public class BaseClientTests : BaseClient
{
    private const string TestUrl = "http://www.example.com";
    private HttpMessageHandler MockedMessageHandler { get; }

    public BaseClientTests() : base(HttpMessageHandlerInjector.CreateClient(), A.Fake<ILogger<BaseClient>>()) => this.MockedMessageHandler = HttpMessageHandlerInjector.Handler;

    private static class HttpMessageHandlerInjector
    {
        public static HttpMessageHandler Handler { get; private set; } = A.Fake<HttpClientHandler>();

        public static HttpClient CreateClient()
        {
            Handler = A.Fake<HttpMessageHandler>();
            return new HttpClient(Handler);
        }
    }

    [Theory]
    [MemberData(nameof(HttpMethodTestData))]
    public async void SendCoreAsync_Success_SuccessResult(HttpMethod method)
    {
        var content = new StringContent("2");
        A.CallTo(this.MockedMessageHandler)
            .InvokingSendAsyncWith(method, TestUrl, content)
            .ReturnsAMessageWith(HttpStatusCode.OK);

        var result = await this.SendCoreAsync(method, TestUrl, content, default);

        A.CallTo(this.MockedMessageHandler)
            .InvokingSendAsyncWith(method, TestUrl, content)
            .MustHaveHappenedOnceExactly();
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [MemberData(nameof(HttpMethodTestData))]
    public async void SendCoreAsync_FailResponseCode_FailsWithNoExceptions(HttpMethod method)
    {
        A.CallTo(this.MockedMessageHandler)
            .InvokingSendAsyncWithAnything()
            .ReturnsAMessageWith(HttpStatusCode.InternalServerError);

        var result = await this.SendCoreAsync(method, TestUrl, null, default);

        Assert.False(result.IsSuccess);
    }

    [Theory]
    [MemberData(nameof(HttpMethodTestData))]
    public async void SendCoreAsync_InternalError_FailsWithNoExceptions(HttpMethod method)
    {
        A.CallTo(this.MockedMessageHandler)
            .InvokingSendAsyncWithAnything()
            .Throws<HttpRequestException>();

        var result = await this.SendCoreAsync(method, TestUrl, null, default);

        Assert.False(result.IsSuccess);
    }

    [Theory]
    [MemberData(nameof(HttpMethodTestData))]
    public async void SendCoreAsyncT_Success_SuccessResult(HttpMethod method)
    {
        var expectedCert = new PartyLicenceDeclaration
        {
            CollegeCode = CollegeCode.Pharmacists,
            LicenceNumber = "12345"
        };
        A.CallTo(this.MockedMessageHandler)
            .InvokingSendAsyncWith(method, TestUrl)
            .ReturnsAMessageWith(HttpStatusCode.OK, expectedCert);

        var result = await this.SendCoreAsync<PartyLicenceDeclaration>(method, TestUrl, null, default);

        Assert.True(result.IsSuccess);

        var cert = result.Value;
        Assert.Equal(expectedCert.CollegeCode, cert.CollegeCode);
        Assert.Equal(expectedCert.LicenceNumber, cert.LicenceNumber);
    }

    [Theory]
    [MemberData(nameof(HttpMethodTestData))]
    public async void SendCoreAsyncT_FailResponseCode_FailsWithNoExceptions(HttpMethod method)
    {
        A.CallTo(this.MockedMessageHandler)
            .InvokingSendAsyncWithAnything()
            .ReturnsAMessageWith(HttpStatusCode.InternalServerError);

        var result = await this.SendCoreAsync<object>(method, TestUrl, null, default);

        Assert.False(result.IsSuccess);
    }

    [Theory]
    [MemberData(nameof(HttpMethodTestData))]
    public async void SendCoreAsyncT_InternalError_FailsWithNoExceptions(HttpMethod method)
    {
        A.CallTo(this.MockedMessageHandler)
            .InvokingSendAsyncWithAnything()
            .Throws<HttpRequestException>();

        var result = await this.SendCoreAsync<object>(method, TestUrl, null, default);

        Assert.False(result.IsSuccess);
    }

    public static IEnumerable<object[]> HttpMethodTestData()
    {
        yield return new object[] { HttpMethod.Delete };
        yield return new object[] { HttpMethod.Get };
        yield return new object[] { HttpMethod.Head };
        yield return new object[] { HttpMethod.Options };
        yield return new object[] { HttpMethod.Patch };
        yield return new object[] { HttpMethod.Post };
        yield return new object[] { HttpMethod.Put };
        yield return new object[] { HttpMethod.Trace };
    }
}
