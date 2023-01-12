namespace PidpTests.Features.AccessRequests;

using FakeItEasy;
using NodaTime;
using Xunit;

using Pidp.Features.AccessRequests;
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
        var party = this.TestDb.Has(new Party
        {
            FirstName = "FirstName",
            LastName = "LastName",
            Birthdate = LocalDate.FromDateTime(DateTime.Today),
            Email = "Email@email.com",
            Phone = "5551234567",
            Cpn = "Cpn",
            LicenceDeclaration = new PartyLicenceDeclaration()
        });
        var client = A.Fake<IPlrClient>()
            .ReturningAStatandingsDigest(true, identifierType);
        var handler = this.MockDependenciesFor<DriverFitness.CommandHandler>(client);

        var result = await handler.HandleAsync(new DriverFitness.Command { PartyId = party.Id });

        Assert.Equal(DriverFitness.AllowedIdentifierTypes.Contains(identifierType), result.IsSuccess);
    }

    [Fact]
    public async void CreateDriverFitnessEnrolment_NoLicenceNoEndorsements_Denied()
    {
        var party = this.TestDb.Has(new Party
        {
            FirstName = "FirstName",
            LastName = "LastName",
            Birthdate = LocalDate.FromDateTime(DateTime.Today),
            Email = "Email@email.com",
            Phone = "5551234567",
            Cpn = null,
            LicenceDeclaration = new PartyLicenceDeclaration()
        });
        var client = A.Fake<IPlrClient>()
            .ReturningAStatandingsDigest(PlrStandingsDigest.FromEmpty());
        var handler = this.MockDependenciesFor<DriverFitness.CommandHandler>(client);

        var result = await handler.HandleAsync(new DriverFitness.Command { PartyId = party.Id });

        Assert.False(result.IsSuccess);
    }

    [Theory]
    [MemberData(nameof(DriverFitnessIdentifierTypeTestData))]
    public async void CreateDriverFitnessEnrolment_NoLicenceWithEndorsement_Accepted(IdentifierType identifierType)
    {
        var party = this.TestDb.Has(new Party
        {
            FirstName = "FirstName",
            LastName = "LastName",
            Birthdate = LocalDate.FromDateTime(DateTime.Today),
            Email = "Email@email.com",
            Phone = "5551234567",
            Cpn = null,
            LicenceDeclaration = new PartyLicenceDeclaration()
        });
        var otherParty = this.TestDb.Has(new Party
        {
            FirstName = "FirstName2",
            LastName = "LastName2",
            Birthdate = LocalDate.FromDateTime(DateTime.Today),
            Email = "Email2@email.com",
            Phone = "5551234567",
            Cpn = "cpn",
            LicenceDeclaration = new PartyLicenceDeclaration()
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
            .ReturningAStatandingsDigest(true, identifierType);
        var handler = this.MockDependenciesFor<DriverFitness.CommandHandler>(client);

        var result = await handler.HandleAsync(new DriverFitness.Command { PartyId = party.Id });

        Assert.True(result.IsSuccess);
        A.CallTo(() => client.GetAggregateStandingsDigestAsync(An<IEnumerable<string?>>.That.IsSameSequenceAs(new[] { otherParty.Cpn }))).MustHaveHappened();
    }

    [Fact]
    public async void CreateDriverFitnessEnrolment_WrongLicenceWithEndorsement_Denied()
    {
        var party = this.TestDb.Has(new Party
        {
            FirstName = "FirstName",
            LastName = "LastName",
            Birthdate = LocalDate.FromDateTime(DateTime.Today),
            Email = "Email@email.com",
            Phone = "5551234567",
            Cpn = "cpn",
            LicenceDeclaration = new PartyLicenceDeclaration()
        });
        var otherParty = this.TestDb.Has(new Party
        {
            FirstName = "FirstName2",
            LastName = "LastName2",
            Birthdate = LocalDate.FromDateTime(DateTime.Today),
            Email = "Email2@email.com",
            Phone = "5551234567",
            Cpn = "cpn2",
            LicenceDeclaration = new PartyLicenceDeclaration()
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

        var handler = this.MockDependenciesFor<DriverFitness.CommandHandler>(client);

        var result = await handler.HandleAsync(new DriverFitness.Command { PartyId = party.Id });

        Assert.False(result.IsSuccess);
        A.CallTo(() => client.GetAggregateStandingsDigestAsync(A<IEnumerable<string?>>._)).MustNotHaveHappened();
    }
}
