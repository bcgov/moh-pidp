namespace PlrTestData;

public static class PlrRecordStatus
{
    public static List<(string Label, string StatusCode, string StatusReason)> Options => new()
    {
        new("GS", "ACTIVE", "PRAC"),
        new("BS", "TERMINATED", "NONPRAC")
    };
}
