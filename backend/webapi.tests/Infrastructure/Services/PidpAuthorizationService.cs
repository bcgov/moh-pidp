namespace PidpTests.Infrastructure.Services;

using DomainResults.Common;
using FakeItEasy;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Xunit;

using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;
using Pidp.Models;

public class PidpAuthorizationServiceTests : InMemoryDbTest
{
    [Fact]
    public async void CheckResourceAccessibility_NoParty_NotFound()
    {
        var service = this.MockDependenciesFor<PidpAuthorizationService>();
        this.TestDb.Has(new Party { Id = 1 });

        var result = await service.CheckResourceAccessibility((Party p) => p.Id == 2, A.Fake<ClaimsPrincipal>());

        Assert.Equal(DomainOperationStatus.NotFound, result.Status);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("a-policy")]
    public async void CheckResourceAccessibility_PartyExists_AuthPropertiesAreForwarded(string? policy)
    {
        var authService = A.Fake<IAuthorizationService>();
        A.CallTo(() => authService.AuthorizeAsync(A<ClaimsPrincipal>._, A<object?>._, A<string>._)).Returns(AuthorizationResult.Success());
        var service = this.MockDependenciesFor<PidpAuthorizationService>(authService);
        var party = this.TestDb.Has(new Party { Id = 1, UserId = Guid.NewGuid() });
        var user = A.Fake<ClaimsPrincipal>();

        if (policy == null)
        {
            await service.CheckResourceAccessibility((Party p) => p.Id == 1, user);
        }
        else
        {
            await service.CheckResourceAccessibility((Party p) => p.Id == 1, user, policy);
        }

        A.CallTo(() => authService.AuthorizeAsync(user, A<IOwnedResource>.That.Matches(x => x.UserId == party.UserId), policy ?? Policies.UserOwnsResource)).MustHaveHappened();
    }

    [Theory]
    [MemberData(nameof(AuthResultTestData))]
    public async void CheckResourceAccessibility_PartyExists_DomainResult(AuthorizationResult authServiceResponse, DomainOperationStatus expected)
    {
        var authService = A.Fake<IAuthorizationService>();
        A.CallTo(() => authService.AuthorizeAsync(A<ClaimsPrincipal>._, A<object?>._, A<string>._)).Returns(authServiceResponse);
        var service = this.MockDependenciesFor<PidpAuthorizationService>(authService);
        this.TestDb.Has(new Party { Id = 1 });

        var result = await service.CheckResourceAccessibility((Party p) => p.Id == 1, A.Fake<ClaimsPrincipal>());

        Assert.Equal(expected, result.Status);
    }

    public static IEnumerable<object[]> AuthResultTestData()
    {
        yield return new object[] { AuthorizationResult.Failed(), DomainOperationStatus.Unauthorized };
        yield return new object[] { AuthorizationResult.Success(), DomainOperationStatus.Success };
    }
}
