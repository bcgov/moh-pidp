namespace PlrIntakeTests.Features.Records;

using Xunit;

using PlrIntake.Features.Records;
using PlrIntake.Models;
using PlrIntakeTests.TestingExtensions;

public class IndexTests : InMemoryDbTest
{
    [Fact]
    public async void GetRecords_SingleCpnSingleRecord_SingleMatchingRecord()
    {
        var cpn = "CPN";
        var record = this.TestDb.Has(new PlrRecord
        {
            Ipc = "IPC1",
            Cpn = cpn,
            IdentifierType = "CPSID",
            CollegeId = "12345",
            ProviderRoleType = "ProviderRoleType",
            StatusCode = "StatusCode",
            StatusStartDate = DateTime.Today,
            StatusReasonCode = "StatusReasonCode"
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
            StatusReasonCode = "DecoyStatusReasonCode"
        });
        var handler = this.MockDependenciesFor<Index.QueryHandler>();

        var result = await handler.HandleAsync(new Index.Query { Cpns = new List<string> { cpn } });

        Assert.NotNull(result);
        Assert.Single(result);

        AssertEquivalentRecords(new[] { record }, result);
    }

    [Fact]
    public async void GetRecords_SingleCpnMultipleRecords_MultipleMatchingRecords()
    {
        var cpn = "CPN";
        var records = this.TestDb.HasSome(new[]
        {
            new PlrRecord
            {
                Ipc = "IPC1",
                Cpn = cpn,
                IdentifierType = "CPSID",
                CollegeId = "12345",
                ProviderRoleType = "ProviderRoleType",
                StatusCode = "StatusCode",
                StatusStartDate = DateTime.Today,
                StatusReasonCode = "StatusReasonCode"
            },
            new PlrRecord
            {
                Ipc = "IPC2",
                Cpn = cpn,
                IdentifierType = "NID",
                CollegeId = "22222",
                ProviderRoleType = "ProviderRoleType2",
                StatusCode = "StatusCode2",
                StatusStartDate = DateTime.Today,
                StatusReasonCode = "StatusReasonCode2"
            }
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
            StatusReasonCode = "DecoyStatusReasonCode"
        });
        var handler = this.MockDependenciesFor<Index.QueryHandler>();

        var result = await handler.HandleAsync(new Index.Query { Cpns = new List<string> { cpn } });

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        AssertEquivalentRecords(records, result);
    }

    [Fact]
    public async void GetRecords_MultipleCpnsMultipleRecords_MultipleMatchingRecords()
    {
        var cpn1 = "CPN1";
        var cpn2 = "CPN2";
        var records = this.TestDb.HasSome(new[]
        {
            new PlrRecord
            {
                Ipc = "IPC11",
                Cpn = cpn1,
                IdentifierType = "CPSID",
                CollegeId = "12345",
                ProviderRoleType = "ProviderRoleType11",
                StatusCode = "StatusCode11",
                StatusStartDate = DateTime.Today,
                StatusReasonCode = "StatusReasonCode11"
            },
            new PlrRecord
            {
                Ipc = "IPC12",
                Cpn = cpn1,
                IdentifierType = "NID",
                CollegeId = "22222",
                ProviderRoleType = "ProviderRoleType12",
                StatusCode = "StatusCode12",
                StatusStartDate = DateTime.Today,
                StatusReasonCode = "StatusReasonCode2"
            },
            new PlrRecord
            {
                Ipc = "IPC2",
                Cpn = cpn2,
                IdentifierType = "2NID",
                CollegeId = "33333",
                ProviderRoleType = "ProviderRoleType2",
                StatusCode = "StatusCode2",
                StatusStartDate = DateTime.Today,
                StatusReasonCode = "StatusReasonCode2"
            }
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
            StatusReasonCode = "DecoyStatusReasonCode"
        });
        var handler = this.MockDependenciesFor<Index.QueryHandler>();

        var result = await handler.HandleAsync(new Index.Query { Cpns = new List<string> { cpn1, cpn2 } });

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);

        AssertEquivalentRecords(records, result);
    }

    private static void AssertEquivalentRecords(IEnumerable<PlrRecord> records, IEnumerable<Index.Model> results)
    {
        Assert.Equal(records.Count(), results.Count());

        static bool comparesEqual(PlrRecord record, Index.Model result) => record.Cpn == result.Cpn
            && record.IdentifierType == result.IdentifierType
            && record.CollegeId == result.CollegeId
            && record.ProviderRoleType == result.ProviderRoleType
            && record.StatusCode == result.StatusCode
            && record.StatusStartDate == result.StatusStartDate
            && record.StatusReasonCode == result.StatusReasonCode;

        foreach (var record in records)
        {
            Assert.Single(results.Where(result => comparesEqual(record, result)));
        }
    }
}
