namespace PidpTests.TestingExtensions;

using FakeItEasy;
using FakeItEasy.Configuration;
using System.Net;
using System.Text.Json;

using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;

public static class FakeItEasyExtensions
{
    public static IReturnValueConfiguration<Task<HttpResponseMessage>> InvokingSendAsyncWith(this IWhereConfiguration<IAnyCallConfigurationWithNoReturnTypeSpecified> configuration, HttpMethod method, string? url = null, HttpContent? content = null)
    {
        return configuration
            .Where(x => x.Method.Name == "SendAsync")
            .WithReturnType<Task<HttpResponseMessage>>()
            .WhenArgumentsMatch((HttpRequestMessage message, CancellationToken token) => message.Method == method
                && (url == null
                    || message.RequestUri == new Uri(url))
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

    public static IKeycloakAdministrationClient ReturningTrueWhenAssigingClientRoles(this IKeycloakAdministrationClient client, Guid? userId = null)
    {
        if (userId == null)
        {
            A.CallTo(() => client.AssignClientRole(A<Guid>._, A<string>._, A<string>._)).Returns(true);
            A.CallTo(() => client.AssignAccessRoles(A<Guid>._, A<MohKeycloakEnrolment>._)).Returns(true);
        }
        else
        {
            A.CallTo(() => client.AssignClientRole(userId.Value, A<string>._, A<string>._)).Returns(true);
            A.CallTo(() => client.AssignAccessRoles(userId.Value, A<MohKeycloakEnrolment>._)).Returns(true);
        }

        return client;
    }

    public static void AssertNoRolesAssigned(this IKeycloakAdministrationClient client, Guid? userId = null)
    {
        if (userId == null)
        {
            A.CallTo(() => client.AssignAccessRoles(A<Guid>._, A<MohKeycloakEnrolment>._)).MustNotHaveHappened();
            A.CallTo(() => client.AssignClientRole(A<Guid>._, A<string>._, A<string>._)).MustNotHaveHappened();
            A.CallTo(() => client.AssignRealmRole(A<Guid>._, A<string>._)).MustNotHaveHappened();
        }
        else
        {
            A.CallTo(() => client.AssignAccessRoles(userId.Value, A<MohKeycloakEnrolment>._)).MustNotHaveHappened();
            A.CallTo(() => client.AssignClientRole(userId.Value, A<string>._, A<string>._)).MustNotHaveHappened();
            A.CallTo(() => client.AssignRealmRole(userId.Value, A<string>._)).MustNotHaveHappened();
        }
    }

    public static IPlrClient ReturningAStandingsDigest(this IPlrClient client, bool goodStanding, string? identifierType = null)
        => client.ReturningAStandingsDigest(AMock.StandingsDigest(goodStanding, identifierType));

    public static IPlrClient ReturningAStandingsDigest(this IPlrClient client, PlrStandingsDigest digest)
    {
        A.CallTo(() => client.GetStandingAsync(A<string?>._)).Returns(digest.HasGoodStanding);
        A.CallTo(() => client.GetStandingsDigestAsync(A<string?>._)).Returns(digest);
        A.CallTo(() => client.GetAggregateStandingsDigestAsync(A<IEnumerable<string?>>._)).Returns(digest);

        return client;
    }

    public static IPlrClient ReturningMultipleStandingsDigests(this IPlrClient client, PlrStandingsDigest digest, PlrStandingsDigest aggregateDigest)
    {
        A.CallTo(() => client.GetStandingAsync(A<string?>._)).Returns(digest.HasGoodStanding);
        A.CallTo(() => client.GetStandingsDigestAsync(A<string?>._)).Returns(digest);
        A.CallTo(() => client.GetAggregateStandingsDigestAsync(A<IEnumerable<string?>>._)).Returns(aggregateDigest);

        return client;
    }

    public static IPlrClient ReturningAStandingsDigestWhenCalledWithCpn(this IPlrClient client, string cpn, bool goodStanding)
    {
        A.CallTo(() => client.GetStandingAsync(cpn)).Returns(goodStanding);
        A.CallTo(() => client.GetStandingsDigestAsync(cpn)).Returns(AMock.StandingsDigest(goodStanding));

        return client;
    }
}
