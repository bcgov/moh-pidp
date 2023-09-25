namespace PlrIntakeTests.Features.Records;

using Xunit;

using PlrIntake.Features.Records;
using PlrIntake.Models;
using PlrIntakeTests.TestingExtensions;

public class SearchTests : InMemoryDbTest
{
    [Fact]
    public async void SearchRecords_SingleRecord_SingleCpn()
    {
        var record = this.TestDb.Has(new PlrRecord
        {
            Cpn = "CPN",
            IdentifierType = "CPSID",
            CollegeId = "12345",
            DateOfBirth = DateTime.Today
        });
        this.TestDb.HasSome(new PlrRecord
        {
            Cpn = "DecoyIdentifier",
            IdentifierType = "PHID",
            CollegeId = record.CollegeId,
            DateOfBirth = record.DateOfBirth
        },
        new PlrRecord
        {
            Cpn = "DecoyId",
            IdentifierType = record.IdentifierType,
            CollegeId = "52345",
            DateOfBirth = record.DateOfBirth
        },
        new PlrRecord
        {
            Cpn = "DecoyBirthdate",
            IdentifierType = record.IdentifierType,
            CollegeId = record.CollegeId,
            DateOfBirth = record.DateOfBirth!.Value.AddDays(1)
        });
        var query = new Search.Query
        {
            CollegeId = record.CollegeId!,
            Birthdate = record.DateOfBirth!.Value,
            IdentifierTypes = new List<string> { record.IdentifierType! }
        };
        var handler = this.MockDependenciesFor<Search.QueryHandler>();

        var result = await handler.HandleAsync(query);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(record.Cpn, result.Single());
    }

    [Theory]
    [MemberData(nameof(IdMatchTestData))]
    public async void SearchRecords_TrimsSpacesTrimsTo5DigitsAndLeadingZeros_Match(string dbId, string searchId)
    {
        var record = this.TestDb.Has(new PlrRecord
        {
            Cpn = "CPN",
            IdentifierType = "CPSID",
            CollegeId = dbId,
            DateOfBirth = DateTime.Today
        });
        var query = new Search.Query
        {
            CollegeId = searchId,
            Birthdate = record.DateOfBirth!.Value,
            IdentifierTypes = new List<string> { record.IdentifierType! }
        };
        var handler = this.MockDependenciesFor<Search.QueryHandler>();

        var result = await handler.HandleAsync(query);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(record.Cpn, result.Single());
    }

    public static IEnumerable<object[]> IdMatchTestData()
    {
        // Should trim search and DB Id of leading zeros
        var leadingZeroCombinations = new[] { "4", "04", "004", "0004", "00004" };
        foreach (var dbId in leadingZeroCombinations)
        {
            foreach (var searchId in leadingZeroCombinations)
            {
                yield return new[] { dbId, searchId };
            }
        }

        // Should trim to five digits and then trim leading zeros.
        yield return new[] { "12345", "9912345" };
        yield return new[] { "9912345", "12345" };
        yield return new[] { "00005", "9900005" };
        yield return new[] { "5", "9900005" };
        yield return new[] { "1234", "9901234" };

        // Should trim leading and trailing spaces.
        yield return new[] { "00012", "          12     " };
        yield return new[] { "12345", "  99912345  " };
    }

    [Theory]
    [InlineData("1234", "91234")]
    [InlineData("90022", "00022")]
    public async void SearchRecords_SimilarButNotMatchingRecords_NoMatch(string dbId, string searchId)
    {
        var record = this.TestDb.Has(new PlrRecord
        {
            Cpn = "CPN",
            IdentifierType = "CPSID",
            CollegeId = dbId,
            DateOfBirth = DateTime.Today
        });
        var query = new Search.Query
        {
            CollegeId = searchId,
            Birthdate = record.DateOfBirth!.Value,
            IdentifierTypes = new List<string> { record.IdentifierType! }
        };
        var handler = this.MockDependenciesFor<Search.QueryHandler>();

        var result = await handler.HandleAsync(query);

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
