namespace PidpTests.Features.AccessRequests;

using FakeItEasy;
using NodaTime;
using Xunit;

using Pidp.Features.AccessRequests;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using PidpTests.TestingExtensions;

public class SAEformsTests : InMemoryDbTest
{
    [Theory]
    [MemberData(nameof(SAEformsIdentifierTypeTestData))]
    public async void CreateSAEformsEnrolment_ValidProfileWithVaryingLicence_MatchesExcludedTypes(IdentifierType identifierType, bool expected)
    {
        var party = this.TestDb.Has(new Party
        {
            FirstName = "FirstName",
            LastName = "LastName",
            Birthdate = LocalDate.FromDateTime(DateTime.Today),
            Email = "Email@email.com",
            Phone = "5551234567",
            Cpn = "Cpn"
        });
        var client = A.Fake<IPlrClient>()
            .ReturningAStatandingsDigest(true, identifierType);
        var keycloak = A.Fake<IKeycloakAdministrationClient>();
        A.CallTo(() => keycloak.AssignClientRole(A<Guid>._, A<string>._, A<string>._)).Returns(true);
        var handler = this.MockDependenciesFor<SAEforms.CommandHandler>(client, keycloak);

        var result = await handler.HandleAsync(new SAEforms.Command { PartyId = party.Id });

        Assert.Equal(expected, result.IsSuccess);
        if (expected)
        {
            A.CallTo(() => keycloak.AssignClientRole(party.UserId, MohClients.SAEforms.ClientId, MohClients.SAEforms.AccessRole)).MustHaveHappened();
        }
        else
        {
            A.CallTo(() => keycloak.AssignClientRole(A<Guid>._, A<string>._, A<string>._)).MustNotHaveHappened();
        }
    }

    public static IEnumerable<object[]> SAEformsIdentifierTypeTestData()
    {
        return TestingUtils.AllIdentifierTypes
             .Select(identifierType => new object[] { identifierType, !SAEforms.ExcludedIdentifierTypes.Contains(identifierType) });
    }
}
