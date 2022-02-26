namespace PidpTests.Infrastructure.HttpClients;

using FakeItEasy;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

using Pidp.Infrastructure.HttpClients.Keycloak;
using PidpTests.TestingExtensions;

public class KeycloakAdministrationClientTests
{
    private const string BaseUrl = "http://www.example.com/";

    [Fact]
    public async void AssignClientRole_HapyPath_Success()
    {
        var userId = Guid.NewGuid();
        var client = new Client
        {
            Id = "12345",
            ClientId = "a client"
        };
        var role = new Role
        {
            ContainerId = client.Id,
            Name = "a role"
        };
        var messageHandler = A.Fake<HttpMessageHandler>();
        A.CallTo(messageHandler)
            .InvokingSendAsyncWith(HttpMethod.Get, BaseUrl + "clients")
            .ReturnsAMessageWith(HttpStatusCode.OK, new[] { client });
        A.CallTo(messageHandler)
            .InvokingSendAsyncWith(HttpMethod.Get, BaseUrl + $"clients/{client.Id}/roles")
            .ReturnsAMessageWith(HttpStatusCode.OK, new[] { role });
        A.CallTo(messageHandler)
            .InvokingSendAsyncWith(HttpMethod.Post, BaseUrl + $"users/{userId}/role-mappings/clients/{client.Id}")
            .ReturnsAMessageWith(HttpStatusCode.OK);
        var keycloakClient = new KeycloakAdministrationClient(new HttpClient(messageHandler) { BaseAddress = new Uri(BaseUrl) }, A.Fake<ILogger<KeycloakAdministrationClient>>());

        var success = await keycloakClient.AssignClientRole(userId, client.ClientId, role.Name);

        Assert.True(success);
        A.CallTo(messageHandler)
            .InvokingSendAsyncWith(HttpMethod.Post, BaseUrl + $"users/{userId}/role-mappings/clients/{client.Id}")
            .MustHaveHappenedOnceExactly();
    }


}
