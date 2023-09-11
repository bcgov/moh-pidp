namespace PidpTests.Features.Parties;

using FakeItEasy;
using Xunit;

using static Pidp.Features.Parties.ProfileStatus;
using static Pidp.Features.Parties.ProfileStatus.Model;
using Pidp.Infrastructure.HttpClients.Plr;
using PidpTests.TestingExtensions;

public class ProfileStatusUserAccessAgreementTests : ProfileStatusTest
{
    [Fact]
    public async void HandleAsync_UserAccessAgreementMustBeAccepted_Incomplete()
    {
        var party = this.TestDb.Has(AParty.WithNoProfile());
        var client = A.Fake<IPlrClient>()
            .ReturningAStandingsDigest(PlrStandingsDigest.FromEmpty());
        var handler = this.MockDependenciesFor<CommandHandler>(client);

        var profile = await handler.HandleAsync(new Command { Id = party.Id, User = AMock.BcscUser() });

        var uaaSection = profile.Section<UserAccessAgreementSection>();
        uaaSection.AssertNoAlerts();
        Assert.Equal(StatusCode.Incomplete, uaaSection.StatusCode);
    }

    [Fact]
    public async void HandleAsync_UserAccessAgreementAccepted_Complete()
    {
        var party = this.TestDb.Has(AParty.WithUserAccessAgreementAccepted());
        var client = A.Fake<IPlrClient>()
            .ReturningAStandingsDigest(PlrStandingsDigest.FromEmpty());
        var handler = this.MockDependenciesFor<CommandHandler>(client);

        var profile = await handler.HandleAsync(new Command { Id = party.Id, User = AMock.BcscUser() });

        var uaaSection = profile.Section<UserAccessAgreementSection>();
        uaaSection.AssertNoAlerts();
        Assert.Equal(StatusCode.Complete, uaaSection.StatusCode);
    }
}
