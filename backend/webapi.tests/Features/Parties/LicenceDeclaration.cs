namespace PidpTests.Features.Parties;

using FakeItEasy;
using NodaTime;
using Xunit;

using static Pidp.Features.Parties.LicenceDeclaration;
using Pidp.Models.DomainEvents;
using Pidp.Models.Lookups;
using Pidp.Infrastructure.HttpClients.Plr;
using PidpTests.TestingExtensions;

public class LicenceDeclarationTests : InMemoryDbTest
{
    [Theory]
    [MemberData(nameof(CollegeCodeTestCases))]
    public async void HandleAsync_LicenceFoundInGoodStanding_FoundEventPublished(CollegeCode collegeCode)
    {
        var expectedCpn = "Cpnn";
        var licenceNumber = collegeCode.ToString();
        var party = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "first";
            party.LastName = "last";
            party.Birthdate = LocalDate.FromDateTime(DateTime.Today);
            party.Email = "an@email.com";
            party.Phone = "5555555555";
        });

        var plrClient = A.Fake<IPlrClient>()
            .ReturningAStandingsDigest(true);
        A.CallTo(() => plrClient.FindCpnAsync(collegeCode, licenceNumber, party.Birthdate!.Value)).Returns(expectedCpn);
        var handler = this.MockDependenciesFor<CommandHandler>(plrClient);

        var result = await handler.HandleAsync(new Command { PartyId = party.Id, LicenceNumber = licenceNumber, CollegeCode = collegeCode });

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedCpn, party.Cpn);
        Assert.NotNull(party.LicenceDeclaration);
        Assert.Equal(licenceNumber, party.LicenceDeclaration.LicenceNumber);
        Assert.Equal(collegeCode, party.LicenceDeclaration.CollegeCode);

        Assert.Single(this.PublishedEvents.OfType<PlrCpnLookupFound>());
        var foundEvent = this.PublishedEvents.OfType<PlrCpnLookupFound>().Single();
        Assert.Equal(party.Id, foundEvent.PartyId);
        Assert.Equal(party.PrimaryUserId, foundEvent.UserIds.Single());
        Assert.Equal(party.Cpn, foundEvent.Cpn);
    }

    [Theory]
    [MemberData(nameof(CollegeCodeTestCases))]
    public async void HandleAsync_LicenceNotFound_NotFoundEventPublished(CollegeCode collegeCode)
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

        var plrClient = A.Fake<IPlrClient>();
        A.CallTo(() => plrClient.FindCpnAsync(A<CollegeCode>._, A<string>._, A<LocalDate>._)).Returns<string?>(null);
        var handler = this.MockDependenciesFor<CommandHandler>(plrClient);

        var result = await handler.HandleAsync(new Command { PartyId = party.Id, LicenceNumber = licenceNumber, CollegeCode = collegeCode });

        Assert.True(result.IsSuccess);
        Assert.Null(party.Cpn);
        Assert.NotNull(party.LicenceDeclaration);
        Assert.Equal(licenceNumber, party.LicenceDeclaration.LicenceNumber);
        Assert.Equal(collegeCode, party.LicenceDeclaration.CollegeCode);

        Assert.Single(this.PublishedEvents.OfType<PlrCpnLookupNotFound>());
        var notFoundEvent = this.PublishedEvents.OfType<PlrCpnLookupNotFound>().Single();
        Assert.Equal(party.Id, notFoundEvent.PartyId);
    }

    public static IEnumerable<object[]> CollegeCodeTestCases()
    {
        return TestData.AllValuesOf<CollegeCode>()
            .Select(code => new object[] { code });
    }
}
