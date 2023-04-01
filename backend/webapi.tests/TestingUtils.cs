namespace PidpTests;

using FakeItEasy;
using System.Reflection;
using System.Security.Claims;
using Xunit;

using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Plr;

public static class AssertThat
{
    /// <summary>
    /// Verifies that two collections are "equivalent" using a comparison lambda. Order is irrelevant.
    /// </summary>
    public static void CollectionsAreEquivalent<T1, T2>(IEnumerable<T1> collection1, IEnumerable<T2> collection2, Func<T1, T2, bool> predicate)
    {
        Assert.Equal(collection1.Count(), collection2.Count());

        var list2 = collection2.ToList();

        foreach (var item1 in collection1)
        {
            var index = list2.FindIndex(item2 => predicate(item1, item2));
            Assert.NotEqual(-1, index);

            list2.RemoveAt(index);
        }

        Assert.Empty(list2);
    }
}

public static class TestData
{
    public static IEnumerable<IdentifierType> AllIdentifierTypes => typeof(IdentifierType)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(field => field.FieldType == typeof(IdentifierType))
        .Select(field => (IdentifierType)field.GetValue(null)!);

    public static IEnumerable<TEnum> AllValuesOf<TEnum>() where TEnum : Enum => (TEnum[])Enum.GetValues(typeof(TEnum));
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

    public static ClaimsPrincipal BcscUser() => User(IdentityProviders.BCServicesCard);

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
