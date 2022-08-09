namespace PlrTestData.Data;

public record PlrRecordStatus(string StatusName, string CollegeIdPrefix, string StatusCode, string StatusReasonCode);

public static partial class TestData
{
    public static IEnumerable<PlrRecordStatus> PlrRecordStatuses => new PlrRecordStatus[]
    {
        new("GS", "0", "ACTIVE",     "PRAC"   ),
        new("BS", "9", "TERMINATED", "NONPRAC")
    };
}
