namespace PidpTests.Features.AccessRequests;

using FakeItEasy;
using NodaTime;
using Xunit;

using Pidp.Features.AccessRequests;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.Lookups;
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

    [Fact]
    public async Task CreateSAEformsEnrolment_Pharmacist_AlsoGrantsNpdp()
    {
        // Pharmacists hold Special Authority and NPDP together, whichever one they applied for.
        var bcsc = new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCServicesCard };
        var bcProvider = new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCProvider };
        var party = this.HasAnEligibleParty(party => party.Credentials = [bcsc, bcProvider]);
        var (keycloak, handler) = this.SetupFor(IdentifierType.Pharmacist);

        var result = await handler.HandleAsync(new SAEforms.Command { PartyId = party.Id });

        Assert.True(result.IsSuccess);
        Assert.Contains(this.TestDb.AccessRequests, request => request.PartyId == party.Id
            && request.AccessTypeCode == AccessTypeCode.NpdpEforms);
        // NPDP is granted to BC Services Card credentials only, unlike Special Authority itself - the
        // two cards target different credentials and the paired grant has to respect that.
        A.CallTo(() => keycloak.AssignAccessRoles(bcsc.UserId, MohKeycloakEnrolment.NpdpEforms)).MustHaveHappened();
        A.CallTo(() => keycloak.AssignAccessRoles(bcProvider.UserId, MohKeycloakEnrolment.NpdpEforms)).MustNotHaveHappened();
        // ...while Special Authority itself reaches both.
        A.CallTo(() => keycloak.AssignAccessRoles(bcsc.UserId, MohKeycloakEnrolment.SAEforms)).MustHaveHappened();
        A.CallTo(() => keycloak.AssignAccessRoles(bcProvider.UserId, MohKeycloakEnrolment.SAEforms)).MustHaveHappened();
    }

    [Theory]
    [InlineData("CPSID")]
    [InlineData("RNID")]
    public async Task CreateSAEformsEnrolment_NotAPharmacist_DoesNotGrantNpdp(string identifierType)
    {
        // Special Authority admits nearly every college, so most of its enrolees must not receive NPDP.
        var party = this.HasAnEligibleParty();
        var (keycloak, handler) = this.SetupFor(identifierType);

        var result = await handler.HandleAsync(new SAEforms.Command { PartyId = party.Id });

        Assert.True(result.IsSuccess);
        Assert.DoesNotContain(this.TestDb.AccessRequests, request => request.AccessTypeCode == AccessTypeCode.NpdpEforms);
        A.CallTo(() => keycloak.AssignAccessRoles(A<Guid>._, MohKeycloakEnrolment.NpdpEforms)).MustNotHaveHappened();
    }

    [Fact]
    public async Task CreateSAEformsEnrolment_PharmacistAlreadyHoldsNpdp_DoesNotGrantItTwice()
    {
        var party = this.HasAnEligibleParty(party => party.AccessRequests =
            [new AccessRequest { AccessTypeCode = AccessTypeCode.NpdpEforms, RequestedOn = Instant.FromUtc(2026, 1, 1, 0, 0) }]);
        var (keycloak, handler) = this.SetupFor(IdentifierType.Pharmacist);

        var result = await handler.HandleAsync(new SAEforms.Command { PartyId = party.Id });

        Assert.True(result.IsSuccess);
        Assert.Single(this.TestDb.AccessRequests, request => request.PartyId == party.Id
            && request.AccessTypeCode == AccessTypeCode.NpdpEforms);
        A.CallTo(() => keycloak.AssignAccessRoles(A<Guid>._, MohKeycloakEnrolment.NpdpEforms)).MustNotHaveHappened();
    }

    [Fact]
    public async Task CreateSAEformsEnrolment_PairedNpdpGrantFails_KeepsTheSAEformsAccess()
    {
        // The Party applied for Special Authority; a failure on the bonus card must not take it away.
        var party = this.HasAnEligibleParty();
        var keycloak = A.Fake<IKeycloakAdministrationClient>().ReturningTrueWhenAssigingClientRoles();
        A.CallTo(() => keycloak.AssignAccessRoles(A<Guid>._, MohKeycloakEnrolment.NpdpEforms)).Returns(false);
        var plr = A.Fake<IPlrClient>().ReturningAStandingsDigest(true, IdentifierType.Pharmacist);
        var handler = this.MockDependenciesFor<SAEforms.CommandHandler>(plr, keycloak);

        var result = await handler.HandleAsync(new SAEforms.Command { PartyId = party.Id });

        Assert.True(result.IsSuccess);
        Assert.Contains(this.TestDb.AccessRequests, request => request.AccessTypeCode == AccessTypeCode.SAEforms);
        Assert.DoesNotContain(this.TestDb.AccessRequests, request => request.AccessTypeCode == AccessTypeCode.NpdpEforms);
    }

    private Party HasAnEligibleParty(Action<Party>? config = null) => this.TestDb.HasAParty(party =>
    {
        party.Email = "Email@email.com";
        party.Cpn = "Cpn";
        party.Credentials = [new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCServicesCard }];
        config?.Invoke(party);
    });

    private (IKeycloakAdministrationClient Keycloak, SAEforms.CommandHandler Handler) SetupFor(string identifierType)
    {
        var plr = A.Fake<IPlrClient>().ReturningAStandingsDigest(true, identifierType);
        var keycloak = A.Fake<IKeycloakAdministrationClient>().ReturningTrueWhenAssigingClientRoles();

        return (keycloak, this.MockDependenciesFor<SAEforms.CommandHandler>(plr, keycloak));
    }
}
