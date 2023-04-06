namespace PidpTests.Features.AccessRequests;

using FakeItEasy;
using NodaTime;
using Xunit;

using Pidp.Features.AccessRequests;
using Pidp.Infrastructure.HttpClients.Plr;
using PidpTests.TestingExtensions;

public class MSTeamsPrivacyOfficerTests : InMemoryDbTest
{
    [Theory]
    [MemberData(nameof(MSTeamsIdentifierTypeTestData))]
    public async void CreateMSTeamsEnrolment_ValidProfileWithVaryingLicence_MatchesAllowedTypes(IdentifierType identifierType)
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
        var handler = this.MockDependenciesFor<MSTeamsPrivacyOfficer.CommandHandler>(client);

        var result = await handler.HandleAsync(new MSTeamsPrivacyOfficer.Command { PartyId = party.Id });

        Assert.Equal(MSTeamsPrivacyOfficer.AllowedIdentifierTypes.Contains(identifierType), result.IsSuccess);
    }

    public static IEnumerable<object[]> MSTeamsIdentifierTypeTestData()
    {
        return TestData.AllIdentifierTypes
             .Select(identifierType => new object[] { identifierType });
    }
}
