namespace PidpTests.Infrastructure.HttpClients;

using Xunit;

using Pidp.Infrastructure.HttpClients.Plr;

public class PlrStatusDigestTests
{
    [Fact]
    public void HasGoodStanding_FromEmpty_FalseNoExceptions()
    {
        var digest = PlrStandingsDigest.FromEmpty();

        var result = digest.HasGoodStanding;

        Assert.False(result);
    }

    [Fact]
    public void HasGoodStanding_FromEmptyWithIdentifiers_FalseNoExceptions()
    {
        var digest = PlrStandingsDigest.FromEmpty();

        var result = digest
            .With(IdentifierType.Nurse, IdentifierType.PhysiciansAndSurgeons)
            .HasGoodStanding;

        Assert.False(result);
    }

    [Fact]
    public void HasGoodStanding_FromEmptyExcludingIdentifiers_FalseNoExceptions()
    {
        var digest = PlrStandingsDigest.FromEmpty();

        var result = digest
            .Excluding(IdentifierType.Nurse, IdentifierType.PhysiciansAndSurgeons)
            .HasGoodStanding;

        Assert.False(result);
    }

    [Fact]
    public void HasGoodStanding_FromError_FalseNoExceptions()
    {
        var digest = PlrStandingsDigest.FromError();

        var result = digest.HasGoodStanding;

        Assert.False(result);
    }

    [Fact]
    public void HasGoodStanding_FromErrorWithIdentifiers_FalseNoExceptions()
    {
        var digest = PlrStandingsDigest.FromError();

        var result = digest
            .With(IdentifierType.Nurse, IdentifierType.PhysiciansAndSurgeons)
            .HasGoodStanding;

        Assert.False(result);
    }

    [Fact]
    public void HasGoodStanding_FromErrorExcludingIdentifiers_FalseNoExceptions()
    {
        var digest = PlrStandingsDigest.FromError();

        var result = digest
            .Excluding(IdentifierType.Nurse, IdentifierType.PhysiciansAndSurgeons)
            .HasGoodStanding;

        Assert.False(result);
    }

    [Fact]
    public void HasGoodStanding_FromRecordsOneGood_True()
    {
        var digest = PlrStandingsDigest.FromRecords(new[] { new PlrRecordMock(true) });

        var result = digest.HasGoodStanding;

        Assert.True(result);
    }

    [Fact]
    public void HasGoodStanding_FromRecordsOneBad_False()
    {
        var digest = PlrStandingsDigest.FromRecords(new[] { new PlrRecordMock(false) });

        var result = digest.HasGoodStanding;

        Assert.False(result);
    }

    [Fact]
    public void HasGoodStanding_FromRecordsOneGoodOneBad_True()
    {
        var digest = PlrStandingsDigest.FromRecords(new[] { new PlrRecordMock(true), new PlrRecordMock(false) });

        var result = digest.HasGoodStanding;

        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(WithIdentityTypeFilterTestData))]
    public void HasGoodStanding_FromRecordsTwoRecordsWithFiltering_NoExceptions(IdentifierType withType, bool expected)
    {
        var digest = PlrStandingsDigest.FromRecords(new[] { new PlrRecordMock(true, IdentifierType.Nurse), new PlrRecordMock(false, IdentifierType.NaturopathicPhysician) });

        var result = digest
            .With(withType)
            .HasGoodStanding;

        Assert.Equal(expected, result);
    }

    public static IEnumerable<object[]> WithIdentityTypeFilterTestData()
    {
        yield return new object[] { IdentifierType.DentalSurgeon, false };
        yield return new object[] { IdentifierType.Nurse, true };
        yield return new object[] { IdentifierType.NaturopathicPhysician, false };
    }

    [Fact]
    public void HasGoodStanding_FromRecordsTwoRecordsWithDifferentStatusFiltering_True()
    {
        var digest = PlrStandingsDigest.FromRecords(new[] { new PlrRecordMock(true, IdentifierType.Nurse), new PlrRecordMock(false, IdentifierType.Nurse) });

        var result = digest
            .With(IdentifierType.Nurse)
            .HasGoodStanding;

        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(ExcludingIdentityTypeFilterTestData))]
    public void HasGoodStanding_FromRecordsTwoRecordsExcludingFiltering_NoExceptions(IdentifierType exceptType, bool expected)
    {
        var digest = PlrStandingsDigest.FromRecords(new[] { new PlrRecordMock(true, IdentifierType.Nurse), new PlrRecordMock(false, IdentifierType.NaturopathicPhysician) });

        var result = digest
            .Excluding(exceptType)
            .HasGoodStanding;

        Assert.Equal(expected, result);
    }

    public static IEnumerable<object[]> ExcludingIdentityTypeFilterTestData()
    {
        yield return new object[] { IdentifierType.DentalSurgeon, true };
        yield return new object[] { IdentifierType.Nurse, false };
        yield return new object[] { IdentifierType.NaturopathicPhysician, true };
    }
}
