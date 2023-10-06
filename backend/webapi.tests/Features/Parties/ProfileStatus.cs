namespace PidpTests.Features.Parties;

using FakeItEasy;
using NodaTime;
using Xunit;

using static Pidp.Features.Parties.ProfileStatus;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models.DomainEvents;
using Pidp.Models.Lookups;
using PidpTests.TestingExtensions;

public class ProfileStatusTests : InMemoryDbTest
{
    [Fact]
    public async void ProfileStatus_LicenceInitiallyNotFoundPlrNotFound_PlrCalledButNoEventPublished()
    {
        var licenceNumber = "123L";
        var collegeCode = CollegeCode.Optometrists;
        var party = this.TestDb.Has(AParty.WithLicenceDeclared(cpn: null, collegeCode, licenceNumber));
        var client = A.Fake<IPlrClient>()
            .ReturningAStandingsDigest(false);
        A.CallTo(() => client.FindCpnAsync(A<CollegeCode>._, A<string>._, A<LocalDate>._)).Returns<string?>(null);
        var handler = this.MockDependenciesFor<QueryHandler>(client);

        var profile = await handler.HandleAsync(new Query { Id = party.Id, User = AMock.BcscUser() });

        Assert.NotNull(profile);
        Assert.Null(party.Cpn);
        Assert.Empty(this.PublishedEvents);
        A.CallTo(() => client.FindCpnAsync(collegeCode, licenceNumber, party.Birthdate!.Value)).MustHaveHappened();
    }

    [Fact]
    public async void ProfileStatus_LicenceInitiallyNotFoundPlrBadStanding_CpnUpdatedPlrCalledEventPublished()
    {
        var licenceNumber = "123L";
        var collegeCode = CollegeCode.Optometrists;
        var expectedCpn = "CPN11";
        var party = this.TestDb.Has(AParty.WithLicenceDeclared(cpn: null, collegeCode, licenceNumber));
        var client = A.Fake<IPlrClient>()
            .ReturningAStandingsDigest(false);
        A.CallTo(() => client.FindCpnAsync(A<CollegeCode>._, A<string>._, A<LocalDate>._)).Returns(expectedCpn);
        var handler = this.MockDependenciesFor<QueryHandler>(client);

        var profile = await handler.HandleAsync(new Query { Id = party.Id, User = AMock.BcscUser() });

        Assert.NotNull(profile);
        Assert.Equal(expectedCpn, party.Cpn);
        Assert.Single(this.PublishedEvents.OfType<PlrCpnLookupFound>());
        var foundEvent = this.PublishedEvents.OfType<PlrCpnLookupFound>().Single();
        Assert.Equal(party.Id, foundEvent.PartyId);
        Assert.Equal(party.PrimaryUserId, foundEvent.UserIds.Single());
        Assert.Equal(party.Cpn, foundEvent.Cpn);
        A.CallTo(() => client.FindCpnAsync(collegeCode, licenceNumber, party.Birthdate!.Value)).MustHaveHappened();
    }
}
