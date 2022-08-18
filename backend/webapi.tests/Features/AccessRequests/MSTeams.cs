namespace PidpTests.Features.AccessRequests;

using FakeItEasy;
using NodaTime;
using Xunit;

using Pidp.Features.AccessRequests;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using PidpTests.TestingExtensions;

public class MSTeamsTests : InMemoryDbTest
{
    [Theory]
    [MemberData(nameof(MSTeamsIdentifierTypeTestData))]
    public async void CreateMSTeamsEnrolment_ValidProfileWithVaryingLicence_MatchesAllowedTypes(IdentifierType identifierType)
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
        var handler = this.MockDependenciesFor<MSTeams.CommandHandler>(client);

        var result = await handler.HandleAsync(new MSTeams.Command { PartyId = party.Id });

        Assert.Equal(MSTeams.AllowedIdentifierTypes.Contains(identifierType), result.IsSuccess);
    }

    public static IEnumerable<object[]> MSTeamsIdentifierTypeTestData()
    {
        return TestingUtils.AllIdentifierTypes
             .Select(identifierType => new object[] { identifierType });
    }
}
