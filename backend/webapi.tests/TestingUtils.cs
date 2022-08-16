namespace PidpTests;

using Pidp.Infrastructure.HttpClients.Plr;

public static class TestingUtils
{
    /// <summary>
    /// Creates a digest containing a single record with the properties indicated.
    /// </summary>
    /// <param name="goodStanding"></param>
    /// <param name="identifierType"></param>
    public static PlrStandingsDigest CreateDigestFrom(bool goodStanding, string? identifierType = null) => CreateDigestFrom((goodStanding, identifierType));

    /// <summary>
    /// Creates a digest containing multiple records with properties as indicated.
    /// </summary>
    /// <param name="records"></param>
    public static PlrStandingsDigest CreateDigestFrom(params (bool GoodStanding, string? IdentifierType)[] records)
    {
        return PlrStandingsDigest.FromRecords
        (
            records.Select(record => new PlrRecordMock(record.GoodStanding, record.IdentifierType))
        );
    }
}

public class PlrRecordMock : PlrRecord
{
    private readonly bool goodStanding;

    public PlrRecordMock(bool goodStanding, string? identifierType = null)
    {
        this.goodStanding = goodStanding;
        this.IdentifierType = identifierType;
    }

    public override bool IsGoodStanding() => this.goodStanding;
}
