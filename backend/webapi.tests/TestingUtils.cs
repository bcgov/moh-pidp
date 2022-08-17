namespace PidpTests;

using FakeItEasy;
using System.Reflection;
using System.Security.Claims;

using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Plr;

public static class TestingUtils
{
    public static IEnumerable<IdentifierType> AllIdentifierTypes => typeof(IdentifierType)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(field => field.FieldType == typeof(IdentifierType))
        .Select(field => (IdentifierType)field.GetValue(null)!);
}

public static class AMock
{
    /// <summary>
    /// Creates a digest containing a single record with the properties indicated.
    /// </summary>
    /// <param name="goodStanding"></param>
    /// <param name="identifierType"></param>
    public static PlrStandingsDigest StandingsDigest(bool goodStanding, string? identifierType = null) => StandingsDigest((goodStanding, identifierType));

    /// <summary>
    /// Creates a digest containing multiple records with properties as indicated.
    /// </summary>
    /// <param name="records"></param>
    public static PlrStandingsDigest StandingsDigest(params (bool GoodStanding, string? IdentifierType)[] records)
    {
        return PlrStandingsDigest.FromRecords
        (
            records.Select(record => new PlrRecordMock(record.GoodStanding, record.IdentifierType))
        );
    }

    public static ClaimsPrincipal BcscUser() => User(ClaimValues.BCServicesCard);

    public static ClaimsPrincipal User(string idp)
    {
        var user = A.Fake<ClaimsPrincipal>();
        A.CallTo(() => user.FindFirst(Claims.IdentityProvider)).Returns(new Claim(Claims.IdentityProvider, idp));
        return user;
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
