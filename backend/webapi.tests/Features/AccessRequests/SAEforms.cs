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
    public async Task CreateSAEformsEnrolment_ValidProfileWithVaryingLicence_MatchesExcludedTypes(IdentifierType identifierType, bool expected)
    {
        var party = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "FirstName";
            party.LastName = "LastName";
            party.Birthdate = LocalDate.FromDateTime(DateTime.Today);
            party.Email = "Email@email.com";
            party.Phone = "5551234567";
            party.Cpn = "Cpn";
            party.Credentials = [
                new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCServicesCard},
                new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCProvider},
            ];
        });
        var client = A.Fake<IPlrClient>()
            .ReturningAStandingsDigest(true, identifierType);
        var keycloak = A.Fake<IKeycloakAdministrationClient>()
            .ReturningTrueWhenAssigingClientRoles();
        var handler = this.MockDependenciesFor<SAEforms.CommandHandler>(client, keycloak);

        var result = await handler.HandleAsync(new SAEforms.Command { PartyId = party.Id });

        Assert.Equal(expected, result.IsSuccess);
        if (expected)
        {
            foreach (var credential in party.Credentials)
            {
                A.CallTo(() => keycloak.AssignAccessRoles(credential.UserId, MohKeycloakEnrolment.SAEforms)).MustHaveHappened();
            }
        }
        else
        {
            keycloak.AssertNoRolesAssigned();
        }
    }

    public static TheoryData<IdentifierType, bool> SAEformsIdentifierTypeTestData()
    {
        var testData = new TheoryData<IdentifierType, bool>();

        foreach (var identifierType in TestData.AllIdentifierTypes)
        {
            testData.Add(identifierType, !SAEforms.ExcludedIdentifierTypes.Contains(identifierType));
        }

        return testData;
    }
}
