namespace PidpTests.Features.Lookups;

using Xunit;

using Pidp.Features.Lookups;
using Pidp.Models;
using PidpTests.TestingExtensions;

public class CommonEmailDomainsTests : InMemoryDbTest
{
    [Fact]
    public async void GetCommonEmailDomains_SingleOccurrances_Empty()
    {
        this.TestDb.HasSome(new[]
        {
            new Party { Email = "1@one.com" },
            new Party { Email = "2@one.ca" },
            new Party { Email = "3@three.gov.bc.ca" },
            new Party { Email = "4@four.net" },
        });

        var handler = this.MockDependenciesFor<CommonEmailDomains.QueryHandler>();

        var result = await handler.HandleAsync(new CommonEmailDomains.Query());

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async void GetCommonEmailDomains_TwoOrMoreOccurrances_InResult()
    {
        var expected = new[] { "two.com", "three.com" };
        this.TestDb.HasSome(new[]
        {
            new Party { Email = "1@one.com" },
            new Party { Email = "2@two.com" },
            new Party { Email = "3@two.com" },
            new Party { Email = "4@three.com" },
            new Party { Email = "5@three.com" },
            new Party { Email = "6@three.com" },
        });

        var handler = this.MockDependenciesFor<CommonEmailDomains.QueryHandler>();

        var result = await handler.HandleAsync(new CommonEmailDomains.Query());

        Assert.NotNull(result);
        Assert.Equal(expected.OrderBy(x => x), result.OrderBy(x => x));
    }

    [Fact]
    public async void GetCommonEmailDomains_DifferentCases_InResult()
    {
        var expected = new[] { "two.com", "three.com" };
        this.TestDb.HasSome(new[]
        {
            new Party { Email = "1@one.com" },
            new Party { Email = "2@two.com" },
            new Party { Email = "3@TWO.COM" },
            new Party { Email = "4@three.com" },
            new Party { Email = "5@Three.Com" },
            new Party { Email = "6@THREE.COM" },
        });

        var handler = this.MockDependenciesFor<CommonEmailDomains.QueryHandler>();

        var result = await handler.HandleAsync(new CommonEmailDomains.Query());

        Assert.NotNull(result);
        Assert.Equal(expected.OrderBy(x => x), result.OrderBy(x => x));
    }
}
