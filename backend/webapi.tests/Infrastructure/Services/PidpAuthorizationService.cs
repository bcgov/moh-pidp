namespace PidpTests.Infrastructure.Services;

using DomainResults.Common;
using FakeItEasy;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Xunit;

using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;
using Pidp.Models;
using PidpTests.TestingExtensions;

public class PidpAuthorizationServiceTests : InMemoryDbTest
{
    [Fact]
    public async void CheckPartyAccessibility_NoParty_NotFound()
    {
        this.TestDb.Has(new Party { Id = 1 });
        var service = this.MockDependenciesFor<PidpAuthorizationService>();

        var result = await service.CheckPartyAccessibility(2, A.Fake<ClaimsPrincipal>());

        Assert.Equal(DomainOperationStatus.NotFound, result.Status);
    }

    [Theory]
    [MemberData(nameof(AuthResultTestData))]
    public async void CheckPartyAccessibility_PartyExists_DomainResult(AuthorizationResult authServiceResponse, DomainOperationStatus expected)
    {
        var party = this.TestDb.Has(new Party { UserId = Guid.NewGuid() });
        var user = A.Fake<ClaimsPrincipal>();
        var authService = A.Fake<IAuthorizationService>();
        A.CallTo(() => authService.AuthorizeAsync(user, A<IOwnedResource>.That.Matches(x => x.UserId == party.UserId), Policies.UserOwnsResource)).Returns(authServiceResponse);
        var service = this.MockDependenciesFor<PidpAuthorizationService>(authService);

        var result = await service.CheckPartyAccessibility(party.Id, user);

        Assert.Equal(expected, result.Status);
        A.CallTo(() => authService.AuthorizeAsync(user, A<IOwnedResource>.That.Matches(x => x.UserId == party.UserId), Policies.UserOwnsResource)).MustHaveHappened();
    }

    public static IEnumerable<object[]> AuthResultTestData()
    {
        yield return new object[] { AuthorizationResult.Failed(), DomainOperationStatus.Unauthorized };
        yield return new object[] { AuthorizationResult.Success(), DomainOperationStatus.Success };
    }
}
