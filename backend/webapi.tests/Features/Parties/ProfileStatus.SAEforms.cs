namespace PidpTests.Features.Parties;

using FakeItEasy;
using NodaTime;
using System.Security.Claims;
using Xunit;

using Pidp.Extensions;
using static Pidp.Features.Parties.ProfileStatus;
using static Pidp.Features.Parties.ProfileStatus.Model;
using Pidp.Models;
using Pidp.Models.Lookups;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Plr;
using PidpTests.TestingExtensions;

public class ProfileStatusSAEformsTests : ProfileStatusTest
{
    [Theory]
    [MemberData(nameof(AllIdpsUserTestCases))]
    public async void HandleAsync_NoProfile_LockedOrHidden(ClaimsPrincipal user)
    {
        var party = this.TestDb.Has(AParty.WithNoProfile(user.GetIdentityProvider()));
        var client = A.Fake<IPlrClient>()
            .ReturningAStatandingsDigest(PlrStandingsDigest.FromEmpty());
        var handler = this.MockDependenciesFor<CommandHandler>(client);

        var profile = await handler.HandleAsync(new Command { Id = party.Id, User = user });

        var eforms = profile.Section<SAEforms>();
        eforms.AssertNoAlerts();
        var expected = user.GetIdentityProvider() == ClaimValues.BCServicesCard
            ? StatusCode.Locked
            : StatusCode.Hidden;
        Assert.Equal(expected, eforms.StatusCode);
        Assert.False(eforms.IncorrectLicenceType);
    }

    [Theory]
    [MemberData(nameof(AllIdpsUserTestCases))]
    public async void HandleAsync_Demographics_LockedOrHidden(ClaimsPrincipal user)
    {
        var party = this.TestDb.Has(AParty.WithDemographics(user.GetIdentityProvider()));
        var client = A.Fake<IPlrClient>()
            .ReturningAStatandingsDigest(PlrStandingsDigest.FromEmpty());
        var handler = this.MockDependenciesFor<CommandHandler>(client);

        var profile = await handler.HandleAsync(new Command { Id = party.Id, User = user });

        var eforms = profile.Section<SAEforms>();
        eforms.AssertNoAlerts();
        var expected = user.GetIdentityProvider() == ClaimValues.BCServicesCard
            ? StatusCode.Locked
            : StatusCode.Hidden;
        Assert.Equal(expected, eforms.StatusCode);
        Assert.False(eforms.IncorrectLicenceType);
    }

    [Theory]
    [MemberData(nameof(AllIdentifierTypesTestCases))]
    public async void HandleAsync_BcscLicenceDeclaredWithGoodStanding_IncompleteOrLockedWithReason(IdentifierType identifierType)
    {
        var party = this.TestDb.Has(AParty.WithLicenceDeclared());
        var client = A.Fake<IPlrClient>()
            .ReturningAStatandingsDigest(true, identifierType);
        var handler = this.MockDependenciesFor<CommandHandler>(client);

        var profile = await handler.HandleAsync(new Command { Id = party.Id, User = AMock.BcscUser() });

        var eforms = profile.Section<SAEforms>();
        eforms.AssertNoAlerts();
        if (identifierType == IdentifierType.Pharmacist
            || identifierType == IdentifierType.PharmacyTech)
        {
            Assert.Equal(StatusCode.Locked, eforms.StatusCode);
            Assert.True(eforms.IncorrectLicenceType);
        }
        else
        {
            Assert.Equal(StatusCode.Incomplete, eforms.StatusCode);
            Assert.False(eforms.IncorrectLicenceType);
        }
    }

    [Fact]
    public async void HandleAsync_BcscNoLicenceDeclared_Locked()
    {
        var party = this.TestDb.Has(AParty.WithNoLicenceDeclared(ClaimValues.BCServicesCard));
        var client = A.Fake<IPlrClient>()
            .ReturningAStatandingsDigest(PlrStandingsDigest.FromEmpty());
        var handler = this.MockDependenciesFor<CommandHandler>(client);

        var profile = await handler.HandleAsync(new Command { Id = party.Id, User = AMock.BcscUser() });

        var eforms = profile.Section<SAEforms>();
        eforms.AssertNoAlerts();
        Assert.Equal(StatusCode.Locked, eforms.StatusCode);
        Assert.False(eforms.IncorrectLicenceType);
    }

    [Theory]
    [MemberData(nameof(PlrFailureTestData))]
    protected async void HandleAsync_BcscLicenceDeclaredWithFailure_Locked(string? cpn, PlrStandingsDigest digest)
    {
        var party = this.TestDb.Has(AParty.WithLicenceDeclared(cpn: cpn));
        var client = A.Fake<IPlrClient>()
            .ReturningAStatandingsDigest(digest);
        A.CallTo(() => client.FindCpnAsync(A<CollegeCode>._, A<string>._, A<LocalDate>._)).Returns((string?)null);
        var handler = this.MockDependenciesFor<CommandHandler>(client);

        var profile = await handler.HandleAsync(new Command { Id = party.Id, User = AMock.BcscUser() });

        var eforms = profile.Section<SAEforms>();
        eforms.AssertNoAlerts();
        Assert.Equal(StatusCode.Locked, eforms.StatusCode);
        Assert.False(eforms.IncorrectLicenceType);
    }

    public static IEnumerable<object[]> PlrFailureTestData()
    {
        yield return new object[] { null!, PlrStandingsDigest.FromEmpty() };
        yield return new object[] { "cpn", PlrStandingsDigest.FromError() };

        foreach (var identifierType in TestingUtils.AllIdentifierTypes)
        {
            yield return new object[] { "cpn", AMock.StandingsDigest(false, identifierType) };
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async void HandleAsync_BcscAccessRequested_Complete(bool standing)
    {
        var party = this.TestDb.Has(AParty.WithLicenceDeclared());
        party.AccessRequests = new[] { new AccessRequest { AccessTypeCode = AccessTypeCode.SAEforms } };
        this.TestDb.SaveChanges();
        var client = A.Fake<IPlrClient>()
            .ReturningAStatandingsDigest(standing);
        var handler = this.MockDependenciesFor<CommandHandler>(client);

        var profile = await handler.HandleAsync(new Command { Id = party.Id, User = AMock.BcscUser() });

        var eforms = profile.Section<SAEforms>();
        eforms.AssertNoAlerts();
        Assert.Equal(StatusCode.Complete, eforms.StatusCode);
        Assert.False(eforms.IncorrectLicenceType);
    }
}
