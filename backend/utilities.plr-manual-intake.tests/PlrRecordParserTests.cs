namespace PlrManualIntakeTests;

using CsvHelper;
using System.Globalization;
using Xunit;

using PlrManualIntake;

public class PlrRecordParserTests
{
    [Theory]
    [InlineData("A", 0)]
    [InlineData("Z", 25)]
    [InlineData("AA", 26)]
    [InlineData("AB", 27)]
    [InlineData("BA", 52)]
    public void TestGetIndex(string excelColumn, int expectedIndex) => Assert.Equal(expectedIndex, PlrRecordParser.GetIndex(excelColumn));

    [Fact]
    public void TestReadRow()
    {
        using var stream = new StreamReader("PLR_Test_Data_IAT20210617_v2.0.csv");
        using var reader = new CsvReader(stream, CultureInfo.InvariantCulture);

        // Header row
        reader.Read();
        Assert.Equal("IPC", reader.GetField(0));

        reader.Read();
        var record = PlrRecordParser.ReadRow(reader);
        Assert.Equal("IPC.00166847.BC.PRS", record.Ipc);
        Assert.Equal("CPN.00166847.BC.PRS", record.Cpn);
        Assert.Equal("CPSID", record.IdentifierType);
        Assert.Equal("20101", record.CollegeId);
        Assert.Equal("MD", record.ProviderRoleType);
        Assert.Equal("PRIMET", record.FirstName);
        Assert.Equal("Norval", record.SecondName);
        Assert.Equal("TEN", record.LastName);
        Assert.Equal("M", record.Gender);
        Assert.Equal(new DateTime(2000, 5, 30), record.DateOfBirth);
        Assert.Equal("ACTIVE", record.StatusCode);
        Assert.Equal("PRAC", record.StatusReasonCode);
        Assert.Equal(new DateTime(2020, 1, 10), record.StatusStartDate);
        Assert.Null(record.StatusExpiryDate);
        Assert.Equal(new[] { "101" }, record.Expertise.Select(x => x.Code));
        Assert.Equal("7887 Fallen Circus", record.Address1Line1);
        Assert.Equal("Burnaby", record.City1);
        Assert.Equal("BC", record.Province1);
        Assert.Equal("V3N 3N6", record.PostalCode1);
        Assert.Equal(new DateTime(1995, 1, 15), record.Address1StartDate);
        Assert.Equal(new[] { "MD", "PHD" }, record.Credentials.Select(x => x.Value));
        Assert.Equal("218", record.TelephoneAreaCode);
        Assert.Equal("310-6997", record.TelephoneNumber);
        Assert.Equal("PRIMETTEN@test.ca", record.Email);
        Assert.Equal("ADMIN", record.ConditionCode);
        Assert.Equal(new DateTime(2020, 1, 1), record.ConditionStartDate);
        Assert.Null(record.ConditionEndDate);
        Assert.Null(record.Address1Line2);

        reader.Read();
        var provider2 = PlrRecordParser.ReadRow(reader);
        Assert.Null(provider2.ConditionCode);
        Assert.Null(provider2.ConditionStartDate);
        Assert.Null(provider2.ConditionEndDate);
    }
}
