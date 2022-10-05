namespace PidpTests.Features.Lookups;

using Xunit;

using Pidp.Features.Lookups;
using Pidp.Models;
using PidpTests.TestingExtensions;
using DomainResults.Common;

public class CommonEmailDomainsTests : InMemoryDbTest
{
    [Theory]
    [InlineData("1@one.com")]
    [InlineData("2@one.ca")]
    [InlineData("3@three.gov.bc.ca")]
    [InlineData("4@four.net")]
    public async void FindCommonEmailDomains_SingleOccurrances_False(string email)
    {
        this.TestDb.HasSome(new[]
        {
            new Party { Email = "1@one.com" },
            new Party { Email = "2@one.ca" },
            new Party { Email = "3@three.gov.bc.ca" },
            new Party { Email = "4@four.net" },
        });
        var query = new CommonEmailDomains.Query { Email = email };
        var handler = this.MockDependenciesFor<CommonEmailDomains.QueryHandler>();

        var result = await handler.HandleAsync(query);

        Assert.Equal(DomainOperationStatus.NotFound, result.Status);
    }

    [Theory]
    [InlineData("test1@one.com", DomainOperationStatus.NotFound)]
    [InlineData("test2@two.com", DomainOperationStatus.Success)]
    [InlineData("test3@three.com", DomainOperationStatus.Success)]
    public async void FindCommonEmailDomains_TwoOrMoreOccurrances_True(string email, DomainOperationStatus expected)
    {
        this.TestDb.HasSome(new[]
        {
            new Party { Email = "1@one.com" },
            new Party { Email = "2@two.com" },
            new Party { Email = "3@two.com" },
            new Party { Email = "4@three.com" },
            new Party { Email = "5@three.com" },
            new Party { Email = "6@three.com" },
        });
        var query = new CommonEmailDomains.Query { Email = email };
        var handler = this.MockDependenciesFor<CommonEmailDomains.QueryHandler>();

        var result = await handler.HandleAsync(query);

        Assert.Equal(expected, result.Status);
    }

    [Theory]
    [InlineData("test1@one.com", DomainOperationStatus.NotFound)]
    [InlineData("test2@two.com", DomainOperationStatus.Success)]
    [InlineData("test3@TwO.COm", DomainOperationStatus.Success)]
    [InlineData("test4@three.com", DomainOperationStatus.Success)]
    [InlineData("test5@THREE.coM", DomainOperationStatus.Success)]
    public async void FindCommonEmailDomains_DifferentCases_True(string email, DomainOperationStatus expected)
    {
        this.TestDb.HasSome(new[]
        {
            new Party { Email = "1@one.com" },
            new Party { Email = "2@two.com" },
            new Party { Email = "3@TWO.COM" },
            new Party { Email = "4@three.com" },
            new Party { Email = "5@Three.Com" },
            new Party { Email = "6@THREE.COM" },
        });
        var query = new CommonEmailDomains.Query { Email = email };
        var handler = this.MockDependenciesFor<CommonEmailDomains.QueryHandler>();

        var result = await handler.HandleAsync(query);

        Assert.Equal(expected, result.Status);
    }

    [Fact]
    public async void FindCommonEmailDomains_DomainOnly_True()
    {
        this.TestDb.HasSome(new[]
        {
            new Party { Email = "1@email.com" },
            new Party { Email = "2@email.com" },
        });
        var query = new CommonEmailDomains.Query { Email = "email.com" };
        var handler = this.MockDependenciesFor<CommonEmailDomains.QueryHandler>();

        var result = await handler.HandleAsync(query);

        Assert.Equal(DomainOperationStatus.Success, result.Status);
    }
}
