namespace PidpTests.Infrastructure.HttpClients;

using FakeItEasy;
using Microsoft.Extensions.Logging;
using System.Net;
using Xunit;

using Pidp.Infrastructure.HttpClients.Ldap;
using PidpTests.TestingExtensions;
using static Pidp.Infrastructure.HttpClients.Ldap.HcimAuthorizationStatus;

public class LdapClientTests
{
    private const string BaseUrl = "http://www.example.com/";

    [Fact]
    public async void HcimLoginAsync_SuccessfulHcimLogin_Authorized()
    {
        var expectedRole = "AN HCIM ROLE";
        var ldapClient = CreateLdapClientWithMockedResponse(new LdapLoginResponse
        {
            Authenticated = true,
            Hcmuserrole = expectedRole,
            Unlocked = true,
            UserName = "USERNAME"
        });

        var result = await ldapClient.HcimLoginAsync("username", "password");

        Assert.True(result.IsSuccess);

        var authStatus = result.Value;
        Assert.Equal(AuthorizationStatus.Authorized, authStatus.Status);
        Assert.Equal(expectedRole, authStatus.HcimUserRole);
        Assert.Null(authStatus.RemainingAttempts);
    }

    [Fact]
    public async void HcimLoginAsync_SuccessfulLoginNoRole_Unauthorized()
    {
        var ldapClient = CreateLdapClientWithMockedResponse(new LdapLoginResponse
        {
            Authenticated = true,
            Unlocked = true,
            UserName = "USERNAME"
        });

        var result = await ldapClient.HcimLoginAsync("username", "password");

        Assert.True(result.IsSuccess);

        var authStatus = result.Value;
        Assert.Equal(AuthorizationStatus.Unauthorized, authStatus.Status);
        Assert.Null(authStatus.HcimUserRole);
        Assert.Null(authStatus.RemainingAttempts);
    }

    [Fact]
    public async void HcimLoginAsync_UserDoesNotExist_AuthError()
    {
        var ldapClient = CreateLdapClientWithMockedResponse(new LdapLoginResponse());

        var result = await ldapClient.HcimLoginAsync("username", "password");

        Assert.True(result.IsSuccess);

        var authStatus = result.Value;
        Assert.Equal(AuthorizationStatus.AuthFailure, authStatus.Status);
        Assert.Null(authStatus.HcimUserRole);
        Assert.Null(authStatus.RemainingAttempts);
    }

    [Fact]
    public async void HcimLoginAsync_BadPassword_AuthErrorWithRemainingAttempts()
    {
        var expectedRemainingAttempts = 2;
        var ldapClient = CreateLdapClientWithMockedResponse(new LdapLoginResponse
        {
            Authenticated = false,
            Unlocked = true,
            RemainingAttempts = expectedRemainingAttempts,
            UserName = "USERNAME"
        });

        var result = await ldapClient.HcimLoginAsync("username", "password");

        Assert.True(result.IsSuccess);

        var authStatus = result.Value;
        Assert.Equal(AuthorizationStatus.AuthFailure, authStatus.Status);
        Assert.Null(authStatus.HcimUserRole);
        Assert.Equal(expectedRemainingAttempts, authStatus.RemainingAttempts);
    }

    [Fact]
    public async void HcimLoginAsync_UserLocked_Locked()
    {
        var ldapClient = CreateLdapClientWithMockedResponse(new LdapLoginResponse
        {
            Authenticated = false,
            Unlocked = false,
            RemainingAttempts = 3,
            UserName = "USERNAME"
        });

        var result = await ldapClient.HcimLoginAsync("username", "password");

        Assert.True(result.IsSuccess);

        var authStatus = result.Value;
        Assert.Equal(AuthorizationStatus.AccountLocked, authStatus.Status);
        Assert.Null(authStatus.HcimUserRole);
        Assert.Null(authStatus.RemainingAttempts);
    }

    [Fact]
    public async void HcimLoginAsync_ClientError_FailDomainResult()
    {
        var messageHandler = A.Fake<HttpMessageHandler>();
        A.CallTo(messageHandler)
            .InvokingSendAsyncWithAnything()
            .Throws<HttpRequestException>();
        var ldapClient = new LdapClient(new HttpClient(messageHandler) { BaseAddress = new Uri(BaseUrl) }, A.Fake<ILogger<LdapClient>>());

        var result = await ldapClient.HcimLoginAsync("username", "password");

        Assert.False(result.IsSuccess);
    }

    public static LdapClient CreateLdapClientWithMockedResponse(LdapLoginResponse response)
    {
        var messageHandler = A.Fake<HttpMessageHandler>();
        A.CallTo(messageHandler)
            .InvokingSendAsyncWith(HttpMethod.Post, BaseUrl + "ldap/users")
            .ReturnsAMessageWith(HttpStatusCode.OK, response);

        return new LdapClient(new HttpClient(messageHandler) { BaseAddress = new Uri(BaseUrl) }, A.Fake<ILogger<LdapClient>>());
    }
}
