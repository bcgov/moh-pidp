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

    [Fact]
    public async void CheckPartyAccessibility_PartyExistsMatchingUserId_Success()
    {
        var party = this.TestDb.HasAParty();
        var user = A.Fake<ClaimsPrincipal>();
        A.CallTo(() => user.FindFirst(Claims.Subject)).Returns(new Claim(Claims.Subject, party.PrimaryUserId.ToString()));
        var service = this.MockDependenciesFor<PidpAuthorizationService>();

        var result = await service.CheckPartyAccessibility(party.Id, user);

        Assert.Equal(DomainOperationStatus.Success, result.Status);
    }

    [Fact]
    public async void CheckPartyAccessibility_PartyExistsNotMatchingUserId_Fail()
    {
        var party = this.TestDb.HasAParty();
        var user = A.Fake<ClaimsPrincipal>();
        A.CallTo(() => user.FindFirst(Claims.Subject)).Returns(new Claim(Claims.Subject, Guid.NewGuid().ToString()));
        var service = this.MockDependenciesFor<PidpAuthorizationService>();

        var result = await service.CheckPartyAccessibility(party.Id, user);

        Assert.Equal(DomainOperationStatus.Unauthorized, result.Status);
    }
}
