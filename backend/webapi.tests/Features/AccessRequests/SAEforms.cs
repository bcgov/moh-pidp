namespace PidpTests.Features.AccessRequests;

using FakeItEasy;
using NodaTime;
using Xunit;

using Pidp.Features.AccessRequests;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;
using PidpTests.TestingExtensions;

public class SAEformsTests : InMemoryDbTest
{
    [Theory]
    [MemberData(nameof(SAEformsIdentifierTypeTestData))]
    public async void CreateSAEformsEnrolment_ValidProfileWithVaryingLicence_MatchesExcludedTypes(IdentifierType identifierType, bool expected)
    {
        var party = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "FirstName";
            party.LastName = "LastName";
            party.Birthdate = LocalDate.FromDateTime(DateTime.Today);
            party.Email = "Email@email.com";
            party.Phone = "5551234567";
            party.Cpn = "Cpn";
        });
        var client = A.Fake<IPlrClient>()
            .ReturningAStatandingsDigest(true, identifierType);
        var keycloak = A.Fake<IKeycloakAdministrationClient>()
            .ReturningTrueWhenAssigingClientRoles();
        var handler = this.MockDependenciesFor<SAEforms.CommandHandler>(client, keycloak);

        var result = await handler.HandleAsync(new SAEforms.Command { PartyId = party.Id });

        Assert.Equal(expected, result.IsSuccess);
        if (expected)
        {
            A.CallTo(() => keycloak.AssignAccessRoles(party.PrimaryUserId, MohKeycloakEnrolment.SAEforms)).MustHaveHappened();
        }
        else
        {
            keycloak.AssertNoRolesAssigned();
        }
    }

    public static IEnumerable<object[]> SAEformsIdentifierTypeTestData()
    {
        return TestData.AllIdentifierTypes
             .Select(identifierType => new object[] { identifierType, !SAEforms.ExcludedIdentifierTypes.Contains(identifierType) });
    }
}
