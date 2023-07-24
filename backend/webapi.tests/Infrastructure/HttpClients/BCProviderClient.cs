namespace PidpTests.Infrastructure.HttpClients;

using FakeItEasy;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Net;
using System.Text.Json;
using Xunit;

using Pidp;
using Pidp.Infrastructure.HttpClients.BCProvider;
using PidpTests.TestingExtensions;

public class BCProviderClientTests
{
    [Theory]
    [MemberData(nameof(IllegalCharactersTestCases))]
    public async void CreateBCProvider_NameWithIllegalCharacters_IllegalCharactersRemoved(string firstName, string lastName)
    {
        var allowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789'.-_!#^~";
        var filteredCharacters = $"{firstName}.{lastName}"
            .Where(allowedCharacters.Contains);
        var expectedName = new string(filteredCharacters.ToArray());

        var userRep = new NewUserRepresentation
        {
            FirstName = firstName,
            LastName = lastName,
            Password = "pwrd",
            PidpEmail = "e@mail.com"
        };
        User? capturedUser = null;
        var messageHandler = A.Fake<HttpMessageHandler>();
        A.CallTo(messageHandler)
            .InvokingSendAsyncWith(HttpMethod.Post)
            .Invokes(async i => capturedUser = await JsonSerializer.DeserializeAsync<User>(await i.GetArgument<HttpRequestMessage>(0)!.Content!.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true }))
            .ReturnsAMessageWith(HttpStatusCode.Created);
        var bcProviderClient = new BCProviderClient(new GraphServiceClient(new HttpClient(messageHandler)), A.Fake<ILogger<BCProviderClient>>(), new PidpConfiguration());

        await bcProviderClient.CreateBCProviderAccount(userRep);

        Assert.NotNull(capturedUser);
        Assert.NotNull(capturedUser.UserPrincipalName);
        var userPrincipal = capturedUser.UserPrincipalName.TrimEnd('@');
        Assert.Equal(expectedName, userPrincipal);
    }

    public static IEnumerable<object[]> IllegalCharactersTestCases()
    {
        yield return new object[] { "E r r", "Dr " };
        yield return new object[] { "32'.-_!#^~e", "name" };
        yield return new object[] { "àèìòu", "last" };
    }
}
