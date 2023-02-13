namespace PidpTests.Features.Parties;

using FakeItEasy;
using NodaTime;
using System.Security.Claims;
using Xunit;

using Pidp.Extensions;
using Pidp.Models;
using Pidp.Models.Lookups;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;
using static Pidp.Features.Parties.LicenceDeclaration;
using PidpTests.TestingExtensions;
using Pidp.Infrastructure.Auth;

public class LicenceDeclarationTests : InMemoryDbTest
{
    [Theory]
    [MemberData(nameof(CollegeCodeTestCases))]
    public async void HandleAsync_LicenceFoundInGoodStanding_KeycloakUpdates(CollegeCode collegeCode)
    {
        var expectedCpn = "Cpn";
        var licenceNumber = collegeCode.ToString();
        var party = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "first";
            party.LastName = "last";
            party.Birthdate = LocalDate.FromDateTime(DateTime.Today);
            party.Email = "an@email.com";
            party.Phone = "5555555555";
        });

        var keycloakClient = A.Fake<IKeycloakAdministrationClient>()
            .ReturningTrueWhenAssigingClientRoles();
        var plrClient = A.Fake<IPlrClient>()
            .ReturningAStatandingsDigest(true);
        A.CallTo(() => plrClient.FindCpnAsync(collegeCode, licenceNumber, party.Birthdate!.Value)).Returns(expectedCpn);
        var handler = this.MockDependenciesFor<CommandHandler>(keycloakClient, plrClient);

        var result = await handler.HandleAsync(new Command { PartyId = party.Id, LicenceNumber = licenceNumber, CollegeCode = collegeCode });

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedCpn, party.Cpn);
        Assert.NotNull(party.LicenceDeclaration);
        Assert.Equal(licenceNumber, party.LicenceDeclaration.LicenceNumber);
        Assert.Equal(collegeCode, party.LicenceDeclaration.CollegeCode);

        A.CallTo(() => keycloakClient.UpdateUserCpn(party.PrimaryUserId, party.Cpn)).MustHaveHappened();
        A.CallTo(() => keycloakClient.AssignClientRole(party.PrimaryUserId, MohClients.LicenceStatus.ClientId, MohClients.LicenceStatus.PractitionerRole)).MustHaveHappened();
    }

    [Theory]
    [MemberData(nameof(CollegeCodeTestCases))]
    public async void HandleAsync_LicenceNotFound_NoKeycloakUpdates(CollegeCode collegeCode)
    {
        var licenceNumber = collegeCode.ToString();
        var party = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "first";
            party.LastName = "last";
            party.Birthdate = LocalDate.FromDateTime(DateTime.Today);
            party.Email = "an@email.com";
            party.Phone = "5555555555";
        });

        var keycloakClient = A.Fake<IKeycloakAdministrationClient>();
        var plrClient = A.Fake<IPlrClient>();
        A.CallTo(() => plrClient.FindCpnAsync(A<CollegeCode>._, A<string>._, A<LocalDate>._)).Returns((string?)null);
        var handler = this.MockDependenciesFor<CommandHandler>(keycloakClient, plrClient);

        var result = await handler.HandleAsync(new Command { PartyId = party.Id, LicenceNumber = licenceNumber, CollegeCode = collegeCode });

        Assert.True(result.IsSuccess);
        Assert.Null(party.Cpn);
        Assert.NotNull(party.LicenceDeclaration);
        Assert.Equal(licenceNumber, party.LicenceDeclaration.LicenceNumber);
        Assert.Equal(collegeCode, party.LicenceDeclaration.CollegeCode);

        A.CallTo(() => keycloakClient.UpdateUserCpn(A<Guid>._, A<string>._)).MustNotHaveHappened();
        A.CallTo(() => keycloakClient.AssignClientRole(A<Guid>._, A<string>._, A<string>._)).MustNotHaveHappened();
    }

    [Theory]
    [MemberData(nameof(CollegeCodeTestCases))]
    public async void HandleAsync_LicenceFoundInBadStanding_CpnUpdatedButNoRoleAssigned(CollegeCode collegeCode)
    {
        var expectedCpn = "Cpn";
        var licenceNumber = collegeCode.ToString();
        var party = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "first";
            party.LastName = "last";
            party.Birthdate = LocalDate.FromDateTime(DateTime.Today);
            party.Email = "an@email.com";
            party.Phone = "5555555555";
        });

        var keycloakClient = A.Fake<IKeycloakAdministrationClient>()
            .ReturningTrueWhenAssigingClientRoles();
        var plrClient = A.Fake<IPlrClient>()
            .ReturningAStatandingsDigest(false);
        A.CallTo(() => plrClient.FindCpnAsync(collegeCode, licenceNumber, party.Birthdate!.Value)).Returns(expectedCpn);
        var handler = this.MockDependenciesFor<CommandHandler>(keycloakClient, plrClient);

        var result = await handler.HandleAsync(new Command { PartyId = party.Id, LicenceNumber = licenceNumber, CollegeCode = collegeCode });

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedCpn, party.Cpn);
        Assert.NotNull(party.LicenceDeclaration);
        Assert.Equal(licenceNumber, party.LicenceDeclaration.LicenceNumber);
        Assert.Equal(collegeCode, party.LicenceDeclaration.CollegeCode);

        A.CallTo(() => keycloakClient.UpdateUserCpn(party.PrimaryUserId, party.Cpn)).MustHaveHappened();
        A.CallTo(() => keycloakClient.AssignClientRole(A<Guid>._, A<string>._, A<string>._)).MustNotHaveHappened();
    }

    public static IEnumerable<object[]> CollegeCodeTestCases()
    {
        return TestData.AllValuesOf<CollegeCode>()
            .Select(code => new object[] { code });
    }
}
