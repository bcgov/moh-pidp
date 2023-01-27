namespace PidpTests.Features.AccessRequests;

using FakeItEasy;
using NodaTime;
using Xunit;

using Pidp.Features.AccessRequests;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using PidpTests.TestingExtensions;

public class DriverFitnessTests : InMemoryDbTest
{
    public static IEnumerable<object[]> DriverFitnessIdentifierTypeTestData()
    {
        return TestData.AllIdentifierTypes
             .Select(identifierType => new object[] { identifierType });
    }

    [Theory]
    [MemberData(nameof(DriverFitnessIdentifierTypeTestData))]
    public async void CreateDriverFitnessEnrolment_ValidProfileWithVaryingLicence_MatchesAllowedTypes(IdentifierType identifierType)
    {
        var party = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "FirstName";
            party.LastName = "LastName";
            party.Birthdate = LocalDate.FromDateTime(DateTime.Today);
            party.Email = "Email@email.com";
            party.Phone = "5551234567";
            party.Cpn = "Cpn";
            party.LicenceDeclaration = new PartyLicenceDeclaration();
        });
        var client = A.Fake<IPlrClient>()
            .ReturningAStatandingsDigest(true, identifierType);
        var keycloakClient = A.Fake<IKeycloakAdministrationClient>()
            .ReturningTrueWhenAssigingClientRoles();
        var handler = this.MockDependenciesFor<DriverFitness.CommandHandler>(client, keycloakClient);

        var result = await handler.HandleAsync(new DriverFitness.Command { PartyId = party.Id });

        Assert.Equal(DriverFitness.AllowedIdentifierTypes.Contains(identifierType), result.IsSuccess);
    }

    [Fact]
    public async void CreateDriverFitnessEnrolment_NoLicenceNoEndorsements_Denied()
    {
        var party = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "FirstName";
            party.LastName = "LastName";
            party.Birthdate = LocalDate.FromDateTime(DateTime.Today);
            party.Email = "Email@email.com";
            party.Phone = "5551234567";
            party.Cpn = null;
            party.LicenceDeclaration = new PartyLicenceDeclaration();
        });
        var client = A.Fake<IPlrClient>()
            .ReturningAStatandingsDigest(PlrStandingsDigest.FromEmpty());
        var keycloakClient = A.Fake<IKeycloakAdministrationClient>()
            .ReturningTrueWhenAssigingClientRoles();
        var handler = this.MockDependenciesFor<DriverFitness.CommandHandler>(client, keycloakClient);

        var result = await handler.HandleAsync(new DriverFitness.Command { PartyId = party.Id });

        Assert.False(result.IsSuccess);
    }

    [Theory]
    [MemberData(nameof(DriverFitnessIdentifierTypeTestData))]
    public async void CreateDriverFitnessEnrolment_NoLicenceWithEndorsement_Accepted(IdentifierType otherPartyIdentifierType)
    {
        var party = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "FirstName";
            party.LastName = "LastName";
            party.Birthdate = LocalDate.FromDateTime(DateTime.Today);
            party.Email = "Email@email.com";
            party.Phone = "5551234567";
            party.Cpn = null;
            party.LicenceDeclaration = new PartyLicenceDeclaration();
        });
        var otherParty = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "FirstName2";
            party.LastName = "LastName2";
            party.Birthdate = LocalDate.FromDateTime(DateTime.Today);
            party.Email = "Email2@email.com";
            party.Phone = "5551234567";
            party.Cpn = "cpn";
            party.LicenceDeclaration = new PartyLicenceDeclaration();
        });
        this.TestDb.Has(new Endorsement
        {
            Active = true,
            EndorsementRelationships = new List<EndorsementRelationship>
            {
                new EndorsementRelationship { PartyId = party.Id },
                new EndorsementRelationship { PartyId = otherParty.Id }
            }
        });
        var client = A.Fake<IPlrClient>()
            .ReturningAStatandingsDigest(true, otherPartyIdentifierType);
        var keycloakClient = A.Fake<IKeycloakAdministrationClient>()
            .ReturningTrueWhenAssigingClientRoles();
        var handler = this.MockDependenciesFor<DriverFitness.CommandHandler>(client, keycloakClient);

        var result = await handler.HandleAsync(new DriverFitness.Command { PartyId = party.Id });

        Assert.True(result.IsSuccess);
        A.CallTo(() => client.GetAggregateStandingsDigestAsync(An<IEnumerable<string?>>.That.IsSameSequenceAs(new[] { otherParty.Cpn }))).MustHaveHappened();
        A.CallTo(() => keycloakClient.AssignClientRole(party.PrimaryUserId, MohClients.DriverFitness.ClientId, MohClients.DriverFitness.AccessRole)).MustHaveHappened();
    }

    [Fact]
    public async void CreateDriverFitnessEnrolment_WrongLicenceWithEndorsement_Denied()
    {
        var party = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "FirstName";
            party.LastName = "LastName";
            party.Birthdate = LocalDate.FromDateTime(DateTime.Today);
            party.Email = "Email@email.com";
            party.Phone = "5551234567";
            party.Cpn = "cpn";
            party.LicenceDeclaration = new PartyLicenceDeclaration();
        });
        var otherParty = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "FirstName2";
            party.LastName = "LastName2";
            party.Birthdate = LocalDate.FromDateTime(DateTime.Today);
            party.Email = "Email2@email.com";
            party.Phone = "5551234567";
            party.Cpn = "cpn2";
            party.LicenceDeclaration = new PartyLicenceDeclaration();
        });
        this.TestDb.Has(new Endorsement
        {
            Active = true,
            EndorsementRelationships = new List<EndorsementRelationship>
            {
                new EndorsementRelationship { PartyId = party.Id },
                new EndorsementRelationship { PartyId = otherParty.Id }
            }
        });
        var client = A.Fake<IPlrClient>();
        A.CallTo(() => client.GetStandingsDigestAsync(party.Cpn)).Returns(AMock.StandingsDigest(true, IdentifierType.DentalSurgeon));
        A.CallTo(() => client.GetAggregateStandingsDigestAsync(new[] { otherParty.Cpn })).Returns(AMock.StandingsDigest(true, IdentifierType.PhysiciansAndSurgeons));
        var keycloakClient = A.Fake<IKeycloakAdministrationClient>()
            .ReturningTrueWhenAssigingClientRoles();
        var handler = this.MockDependenciesFor<DriverFitness.CommandHandler>(client, keycloakClient);

        var result = await handler.HandleAsync(new DriverFitness.Command { PartyId = party.Id });

        Assert.False(result.IsSuccess);
        A.CallTo(() => client.GetAggregateStandingsDigestAsync(A<IEnumerable<string?>>._)).MustNotHaveHappened();
    }
}
