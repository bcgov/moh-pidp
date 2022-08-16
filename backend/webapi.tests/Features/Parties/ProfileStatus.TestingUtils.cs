namespace PidpTests.Features.Parties;

using NodaTime;
using Xunit;

using static Pidp.Features.Parties.ProfileStatus;
using static Pidp.Features.Parties.ProfileStatus.Model;
using Pidp.Infrastructure.Auth;
using Pidp.Models;
using Pidp.Models.Lookups;
using PidpTests;

public class ProfileStatusTest : InMemoryDbTest
{
    public static IEnumerable<object[]> AllIdpsUserTestCases()
    {
        yield return new object[] { AMock.BcscUser() };
        yield return new object[] { AMock.User(ClaimValues.Phsa) };
        yield return new object[] { AMock.User(ClaimValues.Idir) };
    }

    public static IEnumerable<object[]> NonBcscUserTestCases()
    {
        yield return new object[] { AMock.User(ClaimValues.Phsa) };
        yield return new object[] { AMock.User(ClaimValues.Idir) };
    }

    public static IEnumerable<object[]> AllIdentifierTypesTestCases() => TestingUtils.AllIdentifierTypes.Select(type => new object[] { type });
}

public static class AParty
{
    public static Party WithNoProfile(string? identityProvider = null)
    {
        var party = new Party
        {
            UserId = Guid.NewGuid(),
            FirstName = "FirstName",
            LastName = "LastName"
        };

        if (identityProvider == ClaimValues.BCServicesCard)
        {
            party.Hpdid = "HPDID";
            party.Birthdate = LocalDate.FromDateTime(DateTime.Today);
        }

        return party;
    }

    public static Party WithDemographics(string? identityProvider = null)
    {
        var party = WithNoProfile(identityProvider);
        party.Email = "Email@email.com";
        party.Phone = "5551234567";
        return party;
    }

    public static Party WithNoLicenceDeclared(string? identityProvider = null)
    {
        var party = WithDemographics(identityProvider);
        party.LicenceDeclaration = new PartyLicenceDeclaration();
        return party;
    }

    public static Party WithLicenceDeclared(string? cpn = "Cpn", CollegeCode collegeCode = CollegeCode.PhysiciansAndSurgeons, string licenceNumber = "12345")
    {
        var party = WithDemographics(ClaimValues.BCServicesCard);
        party.LicenceDeclaration = new PartyLicenceDeclaration
        {
            CollegeCode = collegeCode,
            LicenceNumber = licenceNumber
        };
        party.Cpn = cpn;
        return party;
    }
}

public static class ProfileStatusTestingExtensions
{
    public static void AssertNoAlerts(this ProfileSection profileSection) => Assert.Equal(new HashSet<Alert>(), profileSection.Alerts);

    public static T Section<T>(this Model profileStatus) where T : ProfileSection => profileStatus.Status.Values.OfType<T>().Single();
}
