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
            Ipc = "IPC1",
            Cpn = "CPN",
            IdentifierType = "CPSID",
            CollegeId = "12345",
            ProviderRoleType = "ProviderRoleType",
            StatusCode = "StatusCode",
            StatusStartDate = DateTime.Today,
            StatusReasonCode = "StatusReasonCode",
            DateOfBirth = DateTime.Today
        });
        this.TestDb.Has(new PlrRecord
        {
            Ipc = "DECOY",
            Cpn = "DECOYCPN",
            IdentifierType = "CPSID",
            CollegeId = "54321",
            ProviderRoleType = "DecoyProviderRoleType",
            StatusCode = "DecoyStatusCode",
            StatusStartDate = DateTime.Today,
            StatusReasonCode = "DecoyStatusReasonCode",
            DateOfBirth = DateTime.Today
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
}
