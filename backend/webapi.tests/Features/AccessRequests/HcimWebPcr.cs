namespace PidpTests.Features.AccessRequests;

using FakeItEasy;
using Xunit;

using Pidp.Features.AccessRequests;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.Lookups;
using PidpTests.TestingExtensions;

public class HcimWebPcrTests : InMemoryDbTest
{
    /// <summary>
    /// The org_details the Registry reads: the placeholder organization id paired with the college name
    /// from CollegeLookup. The id is a placeholder; the names are the real ones and must not drift.
    /// </summary>
    private const string CpsbcOrgDetails = """{"id":"46013722","name":"College of Physicians and Surgeons of BC"}""";

    private const string BccnmOrgDetails = """{"id":"12249113","name":"BC College of Nurses and Midwives"}""";

    [Theory]
    [MemberData(nameof(IdentifierTypeTestData))]
    public async Task CreateHcimWebPcrEnrolment_VaryingLicence_MatchesAllowedColleges(IdentifierType identifierType, bool expected)
    {
        var party = this.HasAnEligibleParty();
        var (plr, keycloak, _) = this.SetupFor(AMock.StandingsDigest(true, identifierType));
        var handler = this.MockDependenciesFor<HcimWebPcr.CommandHandler>(plr, keycloak);

        var result = await handler.HandleAsync(new HcimWebPcr.Command { PartyId = party.Id });

        Assert.Equal(expected, result.IsSuccess);
        Assert.Equal(expected, this.TestDb.AccessRequests.Any(request => request.PartyId == party.Id
            && request.AccessTypeCode == AccessTypeCode.HcimWebPcr));
    }

    public static TheoryData<IdentifierType, bool> IdentifierTypeTestData()
    {
        var testData = new TheoryData<IdentifierType, bool>();

        foreach (var identifierType in TestData.AllIdentifierTypes)
        {
            testData.Add(identifierType, HcimWebPcr.AllowedIdentifierTypes.Contains(identifierType));
        }

        return testData;
    }

    [Fact]
    public async Task CreateHcimWebPcrEnrolment_GrantsTheRoleToTheBCProviderCredentialOnly()
    {
        var party = this.HasAnEligibleParty();
        var (plr, keycloak, _) = this.SetupFor(AMock.StandingsDigest(true, IdentifierType.PhysiciansAndSurgeons));
        var handler = this.MockDependenciesFor<HcimWebPcr.CommandHandler>(plr, keycloak);

        var result = await handler.HandleAsync(new HcimWebPcr.Command { PartyId = party.Id });

        Assert.True(result.IsSuccess);
        foreach (var credential in party.Credentials)
        {
            var expected = credential.IdentityProvider == IdentityProviders.BCProvider;
            var call = A.CallTo(() => keycloak.AssignAccessRoles(credential.UserId, MohKeycloakEnrolment.HcimWebPcr));

            if (expected)
            {
                call.MustHaveHappened();
            }
            else
            {
                call.MustNotHaveHappened();
            }
        }
    }

    [Theory]
    [InlineData("CPSID", CpsbcOrgDetails)]
    [InlineData("RNID", BccnmOrgDetails)]
    [InlineData("RMID", BccnmOrgDetails)]
    public async Task CreateHcimWebPcrEnrolment_WritesTheOrganizationForTheLicensingCollege(string identifierType, string expected)
    {
        var party = this.HasAnEligibleParty();
        var (plr, keycloak, user) = this.SetupFor(AMock.StandingsDigest(true, identifierType));
        var handler = this.MockDependenciesFor<HcimWebPcr.CommandHandler>(plr, keycloak);

        var result = await handler.HandleAsync(new HcimWebPcr.Command { PartyId = party.Id });

        Assert.True(result.IsSuccess);
        // Pins the exact attribute key and the JSON shape the Registry reads. A plain string, or the key
        // "organization", would be written happily by Keycloak and read by nobody.
        Assert.Equal([expected], user.Attributes["org_details"]);
        // Written to the same User that received the role, which is the BC Provider credential's.
        var bcProviderUserId = party.Credentials.Single(credential => credential.IdentityProvider == IdentityProviders.BCProvider).UserId;
        A.CallTo(() => keycloak.UpdateUser(bcProviderUserId, A<Action<UserRepresentation>>._)).MustHaveHappened();
    }

    [Fact]
    public void OrganizationCollegeFor_RegisteredWithTwoColleges_TakesTheFirstByPrecedence()
    {
        // Improbable, but the rule has to be decided rather than left to record order from PLR. Asserted on
        // the predicate rather than the written attribute, because both colleges share one placeholder
        // organization today and the difference would not be visible through Keycloak.
        var digest = AMock.StandingsDigest(
            (true, IdentifierType.Nurse, null),
            (true, IdentifierType.PhysiciansAndSurgeons, null));

        Assert.Equal(CollegeCode.PhysiciansAndSurgeons, HcimWebPcr.OrganizationCollegeFor(digest));
    }

    [Theory]
    [InlineData("CPSID", (int)CollegeCode.PhysiciansAndSurgeons)]
    [InlineData("RNID", (int)CollegeCode.NursesAndMidwives)]
    [InlineData("RMID", (int)CollegeCode.NursesAndMidwives)]
    public void OrganizationCollegeFor_MapsEachIdentifierTypeToItsCollege(string identifierType, int expected)
    {
        Assert.Equal((CollegeCode)expected, HcimWebPcr.OrganizationCollegeFor(AMock.StandingsDigest(true, identifierType)));
    }

    [Fact]
    public void OrganizationCollegeFor_IneligibleCollege_IsNull()
    {
        Assert.Null(HcimWebPcr.OrganizationCollegeFor(AMock.StandingsDigest(true, IdentifierType.Pharmacist)));
    }

    [Fact]
    public async Task CreateHcimWebPcrEnrolment_MoaEndorsedByAllowedCollege_Success()
    {
        // An MOA has no CPN and qualifies on their endorsers, taking the college of the practitioner they
        // work under as their organization.
        var party = this.HasAnEligibleParty(party => party.Cpn = null);
        var endorser = this.TestDb.HasAParty(endorser => endorser.Cpn = "EndorserCpn");
        this.TestDb.Has(new Endorsement
        {
            Active = true,
            EndorsementRelationships =
            [
                new EndorsementRelationship { PartyId = party.Id },
                new EndorsementRelationship { PartyId = endorser.Id }
            ]
        });
        var (plr, keycloak, user) = this.SetupFor(
            PlrStandingsDigest.FromEmpty(),
            AMock.StandingsDigest(true, IdentifierType.Nurse));
        var handler = this.MockDependenciesFor<HcimWebPcr.CommandHandler>(plr, keycloak);

        var result = await handler.HandleAsync(new HcimWebPcr.Command { PartyId = party.Id });

        Assert.True(result.IsSuccess);
        // An MOA takes the organization of the college their endorser is registered with - here a Nurse.
        Assert.Equal([BccnmOrgDetails], user.Attributes["org_details"]);
        // Judged on the endorsement digest; their own standing is never asked for.
        A.CallTo(() => plr.GetAggregateStandingsDigestAsync(A<IEnumerable<string?>>._)).MustHaveHappened();
        A.CallTo(() => plr.GetStandingsDigestAsync(A<string?>._)).MustNotHaveHappened();
    }

    [Fact]
    public async Task CreateHcimWebPcrEnrolment_MoaWithNoGoodEndorsement_Failure()
    {
        var party = this.HasAnEligibleParty(party => party.Cpn = null);
        var (plr, keycloak, _) = this.SetupFor(
            PlrStandingsDigest.FromEmpty(),
            AMock.StandingsDigest(false, IdentifierType.PhysiciansAndSurgeons));
        var handler = this.MockDependenciesFor<HcimWebPcr.CommandHandler>(plr, keycloak);

        var result = await handler.HandleAsync(new HcimWebPcr.Command { PartyId = party.Id });

        Assert.False(result.IsSuccess);
        keycloak.AssertNoRolesAssigned();
        Assert.DoesNotContain(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
    }

    [Fact]
    public async Task CreateHcimWebPcrEnrolment_MoaEndorsedByIneligibleCollege_Failure()
    {
        var party = this.HasAnEligibleParty(party => party.Cpn = null);
        var (plr, keycloak, _) = this.SetupFor(
            PlrStandingsDigest.FromEmpty(),
            AMock.StandingsDigest(true, IdentifierType.Pharmacist));
        var handler = this.MockDependenciesFor<HcimWebPcr.CommandHandler>(plr, keycloak);

        var result = await handler.HandleAsync(new HcimWebPcr.Command { PartyId = party.Id });

        Assert.False(result.IsSuccess);
        keycloak.AssertNoRolesAssigned();
    }

    [Fact]
    public async Task CreateHcimWebPcrEnrolment_OrganizationWriteFails_NoAccessRequestSoItCanBeRetried()
    {
        // The Registry reads the organization, so an enrolment without one is not a finished enrolment.
        // Leaving the Access Request unwritten means the retry re-runs the whole grant; the role
        // assignment ahead of it is idempotent.
        var party = this.HasAnEligibleParty();
        var (plr, keycloak, _) = this.SetupFor(AMock.StandingsDigest(true, IdentifierType.PhysiciansAndSurgeons));
        A.CallTo(() => keycloak.UpdateUser(A<Guid>._, A<Action<UserRepresentation>>._)).Returns(false);
        var handler = this.MockDependenciesFor<HcimWebPcr.CommandHandler>(plr, keycloak);

        var result = await handler.HandleAsync(new HcimWebPcr.Command { PartyId = party.Id });

        Assert.False(result.IsSuccess);
        Assert.DoesNotContain(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
    }

    [Fact]
    public async Task CreateHcimWebPcrEnrolment_NoBCProviderCredential_Failure()
    {
        // The role has nowhere to land without one, and there would be no User to hold the organization.
        var party = this.HasAnEligibleParty(party => party.Credentials =
            [new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCServicesCard }]);
        var (plr, keycloak, _) = this.SetupFor(AMock.StandingsDigest(true, IdentifierType.PhysiciansAndSurgeons));
        var handler = this.MockDependenciesFor<HcimWebPcr.CommandHandler>(plr, keycloak);

        var result = await handler.HandleAsync(new HcimWebPcr.Command { PartyId = party.Id });

        Assert.False(result.IsSuccess);
        keycloak.AssertNoRolesAssigned();
    }

    [Fact]
    public async Task CreateHcimWebPcrEnrolment_AlreadyEnroled_Failure()
    {
        var party = this.HasAnEligibleParty(party => party.AccessRequests =
            [new AccessRequest { AccessTypeCode = AccessTypeCode.HcimWebPcr, RequestedOn = NodaTime.Instant.FromUtc(2026, 1, 1, 0, 0) }]);
        var (plr, keycloak, _) = this.SetupFor(AMock.StandingsDigest(true, IdentifierType.PhysiciansAndSurgeons));
        var handler = this.MockDependenciesFor<HcimWebPcr.CommandHandler>(plr, keycloak);

        var result = await handler.HandleAsync(new HcimWebPcr.Command { PartyId = party.Id });

        Assert.False(result.IsSuccess);
        keycloak.AssertNoRolesAssigned();
    }

    /// <summary>
    /// A Party who meets every non-PLR precondition: a BC Services Card credential to qualify, and the
    /// BC Provider credential the role and the organization are written to.
    /// </summary>
    private Party HasAnEligibleParty(Action<Party>? config = null) => this.TestDb.HasAParty(party =>
    {
        party.Cpn = "Cpn";
        party.Credentials =
        [
            new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCServicesCard },
            new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCProvider }
        ];
        config?.Invoke(party);
    });

    private (IPlrClient Plr, IKeycloakAdministrationClient Keycloak, UserRepresentation User) SetupFor(PlrStandingsDigest digest)
        => this.SetupFor(digest, digest);

    /// <summary>
    /// The two-digest form for MOA cases, where the Party's own standing and their endorsers' must differ.
    /// The returned UserRepresentation captures whatever the handler writes to Keycloak, so a test can
    /// assert on the organization without reaching into the fake.
    /// </summary>
    private (IPlrClient Plr, IKeycloakAdministrationClient Keycloak, UserRepresentation User) SetupFor(PlrStandingsDigest digest, PlrStandingsDigest aggregateDigest)
    {
        var plr = A.Fake<IPlrClient>().ReturningMultipleStandingsDigests(digest, aggregateDigest);
        var keycloak = A.Fake<IKeycloakAdministrationClient>().ReturningTrueWhenAssigingClientRoles();
        var user = new UserRepresentation();

        A.CallTo(() => keycloak.UpdateUser(A<Guid>._, A<Action<UserRepresentation>>._))
            .Invokes((Guid _, Action<UserRepresentation> update) => update(user))
            .Returns(true);

        return (plr, keycloak, user);
    }
}
