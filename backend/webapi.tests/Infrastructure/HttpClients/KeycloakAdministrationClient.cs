namespace PidpTests.Infrastructure.HttpClients;

using FakeItEasy;
using Microsoft.Extensions.Logging;
using System.Net;
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
        string? capturedMessageContent = null;
        var messageHandler = A.Fake<HttpMessageHandler>();
        A.CallTo(messageHandler)
            .InvokingSendAsyncWith(HttpMethod.Get, BaseUrl + "clients")
            .ReturnsAMessageWith(HttpStatusCode.OK, new[] { client });
        A.CallTo(messageHandler)
            .InvokingSendAsyncWith(HttpMethod.Get, BaseUrl + $"clients/{client.Id}/roles")
            .ReturnsAMessageWith(HttpStatusCode.OK, new[] { role });
        A.CallTo(messageHandler)
            .InvokingSendAsyncWith(HttpMethod.Post, BaseUrl + $"users/{userId}/role-mappings/clients/{client.Id}")
            .Invokes(async i => capturedMessageContent = await i.GetArgument<HttpRequestMessage>(0)!.Content!.ReadAsStringAsync())
            .ReturnsAMessageWith(HttpStatusCode.OK);
        var keycloakClient = new KeycloakAdministrationClient(new HttpClient(messageHandler) { BaseAddress = new Uri(BaseUrl) }, A.Fake<ILogger<KeycloakAdministrationClient>>());

        var success = await keycloakClient.AssignClientRole(userId, client.ClientId, role.Name);

        Assert.True(success);
        A.CallTo(messageHandler)
            .InvokingSendAsyncWith(HttpMethod.Post, BaseUrl + $"users/{userId}/role-mappings/clients/{client.Id}")
            .MustHaveHappenedOnceExactly();

        Assert.NotNull(capturedMessageContent);
        var sentRoles = JsonSerializer.Deserialize<IEnumerable<Role>>(capturedMessageContent!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        Assert.Single(sentRoles);
        var sentRole = sentRoles.Single();
        Assert.Equal(role.ContainerId, sentRole.ContainerId);
        Assert.Equal(role.Name, sentRole.Name);
    }

    [Fact]
    public async void AssignAccessRoles_HapyPath_Success()
    {
        var userId = Guid.NewGuid();
        var client = new Client
        {
            Id = "12345",
            ClientId = MohKeycloakEnrolment.SAEforms.ClientId
        };
        var role = new Role
        {
            ContainerId = client.Id,
            Name = MohKeycloakEnrolment.SAEforms.AccessRoles.Single()
        };
        string? capturedMessageContent = null;
        var messageHandler = A.Fake<HttpMessageHandler>();
        A.CallTo(messageHandler)
            .InvokingSendAsyncWith(HttpMethod.Get, BaseUrl + "clients")
            .ReturnsAMessageWith(HttpStatusCode.OK, new[] { client });
        A.CallTo(messageHandler)
            .InvokingSendAsyncWith(HttpMethod.Get, BaseUrl + $"clients/{client.Id}/roles")
            .ReturnsAMessageWith(HttpStatusCode.OK, new[] { role });
        A.CallTo(messageHandler)
            .InvokingSendAsyncWith(HttpMethod.Post, BaseUrl + $"users/{userId}/role-mappings/clients/{client.Id}")
            .Invokes(async i => capturedMessageContent = await i.GetArgument<HttpRequestMessage>(0)!.Content!.ReadAsStringAsync())
            .ReturnsAMessageWith(HttpStatusCode.OK);
        var keycloakClient = new KeycloakAdministrationClient(new HttpClient(messageHandler) { BaseAddress = new Uri(BaseUrl) }, A.Fake<ILogger<KeycloakAdministrationClient>>());

        var success = await keycloakClient.AssignAccessRoles(userId, MohKeycloakEnrolment.SAEforms);

        Assert.True(success);
        A.CallTo(messageHandler)
            .InvokingSendAsyncWith(HttpMethod.Post, BaseUrl + $"users/{userId}/role-mappings/clients/{client.Id}")
            .MustHaveHappenedOnceExactly();

        Assert.NotNull(capturedMessageContent);
        var sentRoles = JsonSerializer.Deserialize<IEnumerable<Role>>(capturedMessageContent!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        Assert.Single(sentRoles);
        var sentRole = sentRoles.Single();
        Assert.Equal(role.ContainerId, sentRole.ContainerId);
        Assert.Equal(role.Name, sentRole.Name);
    }

    [Fact]
    public async Task RemoveAccessRoles_HapyPath_Success()
    {
        var userId = Guid.NewGuid();
        var client = new Client
        {
            Id = "12345",
            ClientId = MohKeycloakEnrolment.InfantRsvEforms.ClientId
        };
        var role = new Role
        {
            ContainerId = client.Id,
            Name = MohKeycloakEnrolment.InfantRsvEforms.AccessRoles.Single()
        };
        string? capturedMessageContent = null;
        var messageHandler = A.Fake<HttpMessageHandler>();
        A.CallTo(messageHandler)
            .InvokingSendAsyncWith(HttpMethod.Get, BaseUrl + "clients")
            .ReturnsAMessageWith(HttpStatusCode.OK, new[] { client });
        A.CallTo(messageHandler)
            .InvokingSendAsyncWith(HttpMethod.Get, BaseUrl + $"clients/{client.Id}/roles")
            .ReturnsAMessageWith(HttpStatusCode.OK, new[] { role });
        A.CallTo(messageHandler)
            .InvokingSendAsyncWith(HttpMethod.Delete, BaseUrl + $"users/{userId}/role-mappings/clients/{client.Id}")
            .Invokes(async i => capturedMessageContent = await i.GetArgument<HttpRequestMessage>(0)!.Content!.ReadAsStringAsync())
            .ReturnsAMessageWith(HttpStatusCode.NoContent);
        var keycloakClient = new KeycloakAdministrationClient(new HttpClient(messageHandler) { BaseAddress = new Uri(BaseUrl) }, A.Fake<ILogger<KeycloakAdministrationClient>>());

        var success = await keycloakClient.RemoveAccessRoles(userId, MohKeycloakEnrolment.InfantRsvEforms);

        Assert.True(success);
        A.CallTo(messageHandler)
            .InvokingSendAsyncWith(HttpMethod.Delete, BaseUrl + $"users/{userId}/role-mappings/clients/{client.Id}")
            .MustHaveHappenedOnceExactly();

        Assert.NotNull(capturedMessageContent);
        var sentRoles = JsonSerializer.Deserialize<IEnumerable<Role>>(capturedMessageContent!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        Assert.Single(sentRoles);
        var sentRole = sentRoles.Single();
        Assert.Equal(role.ContainerId, sentRole.ContainerId);
        Assert.Equal(role.Name, sentRole.Name);
    }

    [Fact]
    public async Task RemoveAccessRoles_RoleNotFoundInClient_Fails()
    {
        // Guards against issuing a DELETE that would silently succeed against the wrong role set.
        var userId = Guid.NewGuid();
        var client = new Client
        {
            Id = "12345",
            ClientId = MohKeycloakEnrolment.InfantRsvEforms.ClientId
        };
        var messageHandler = A.Fake<HttpMessageHandler>();
        A.CallTo(messageHandler)
            .InvokingSendAsyncWith(HttpMethod.Get, BaseUrl + "clients")
            .ReturnsAMessageWith(HttpStatusCode.OK, new[] { client });
        A.CallTo(messageHandler)
            .InvokingSendAsyncWith(HttpMethod.Get, BaseUrl + $"clients/{client.Id}/roles")
            .ReturnsAMessageWith(HttpStatusCode.OK, Array.Empty<Role>());
        var keycloakClient = new KeycloakAdministrationClient(new HttpClient(messageHandler) { BaseAddress = new Uri(BaseUrl) }, A.Fake<ILogger<KeycloakAdministrationClient>>());

        var success = await keycloakClient.RemoveAccessRoles(userId, MohKeycloakEnrolment.InfantRsvEforms);

        Assert.False(success);
        A.CallTo(messageHandler)
            .InvokingSendAsyncWith(HttpMethod.Delete)
            .MustNotHaveHappened();
    }
}
