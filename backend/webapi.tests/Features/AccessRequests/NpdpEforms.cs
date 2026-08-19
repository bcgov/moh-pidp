namespace PidpTests.Features.AccessRequests;

using FakeItEasy;
using NodaTime;
using Xunit;

using Pidp.Features.AccessRequests;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.Lookups;
using PidpTests.TestingExtensions;

public class NpdpEformsTests : InMemoryDbTest
{
    [Theory]
    [MemberData(nameof(NpdpEformsIdentifierTypeTestData))]
    public async Task CreateNpdpEformsEnrolment_ValidProfileWithVaryingLicence_MatchesAllowedTypes(IdentifierType identifierType, bool expected)
    {
        var party = this.TestDb.HasAParty(party =>
        {
            party.Email = "Email@email.com";
            party.Cpn = "Cpn";
            party.Credentials = [
                new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCServicesCard },
                new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCProvider },
            ];
        });
        var client = A.Fake<IPlrClient>()
            .ReturningAStandingsDigest(true, identifierType);
        var keycloak = A.Fake<IKeycloakAdministrationClient>()
            .ReturningTrueWhenAssigingClientRoles();
        var handler = this.MockDependenciesFor<NpdpEforms.CommandHandler>(client, keycloak);

        var result = await handler.HandleAsync(new NpdpEforms.Command { PartyId = party.Id });

        Assert.Equal(expected, result.IsSuccess);
        if (expected)
        {
            // The role is granted to BC Services Card credentials only, never to BC Provider ones.
            foreach (var credential in party.Credentials.Where(credential => credential.IdentityProvider == IdentityProviders.BCServicesCard))
            {
                A.CallTo(() => keycloak.AssignAccessRoles(credential.UserId, MohKeycloakEnrolment.NpdpEforms)).MustHaveHappened();
            }
            foreach (var credential in party.Credentials.Where(credential => credential.IdentityProvider != IdentityProviders.BCServicesCard))
            {
                A.CallTo(() => keycloak.AssignAccessRoles(credential.UserId, MohKeycloakEnrolment.NpdpEforms)).MustNotHaveHappened();
            }
            Assert.Contains(this.TestDb.AccessRequests, request => request.PartyId == party.Id
                && request.AccessTypeCode == AccessTypeCode.NpdpEforms);
        }
        else
        {
            keycloak.AssertNoRolesAssigned();
            Assert.DoesNotContain(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
        }
    }

    public static TheoryData<IdentifierType, bool> NpdpEformsIdentifierTypeTestData()
    {
        var testData = new TheoryData<IdentifierType, bool>();

        foreach (var identifierType in TestData.AllIdentifierTypes)
        {
            testData.Add(identifierType, NpdpEforms.AllowedIdentifierTypes.Contains(identifierType));
        }

        return testData;
    }

    [Fact]
    public async Task CreateNpdpEformsEnrolment_CpsPostgradResident_Success()
    {
        // Residents hold a CPSID licence in PENDING/NONPRAC, which is not "good standing";
        // they qualify only through the IsCpsPostgrad clause.
        var party = this.TestDb.HasAParty(party =>
        {
            party.Email = "Email@email.com";
            party.Cpn = "Cpn";
            party.Credentials = [new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCServicesCard }];
        });
        var digest = PlrStandingsDigest.FromRecords([
            new PlrRecord
            {
                IdentifierType = IdentifierType.PhysiciansAndSurgeons,
                StatusCode = PlrStatusCode.Pending,
                StatusReasonCode = PlrStatusReasonCode.NonPracticing
            }
        ]);
        Assert.False(digest.HasGoodStanding);

        var client = A.Fake<IPlrClient>().ReturningAStandingsDigest(digest);
        var keycloak = A.Fake<IKeycloakAdministrationClient>()
            .ReturningTrueWhenAssigingClientRoles();
        var handler = this.MockDependenciesFor<NpdpEforms.CommandHandler>(client, keycloak);

        var result = await handler.HandleAsync(new NpdpEforms.Command { PartyId = party.Id });

        Assert.True(result.IsSuccess);
        A.CallTo(() => keycloak.AssignAccessRoles(A<Guid>._, MohKeycloakEnrolment.NpdpEforms)).MustHaveHappened();
    }

    [Fact]
    public async Task CreateNpdpEformsEnrolment_NursePractitioner_Success()
    {
        // Nurse Practitioners carry the "RNID" identifier and are distinguished only by the
        // RNP Provider Role Type, so the AllowedIdentifierTypes list already covers them.
        var digest = AMock.StandingsDigest((true, IdentifierType.Nurse, ProviderRoleType.RegisteredNursePractitioner));
        var party = this.TestDb.HasAParty(party =>
        {
            party.Email = "Email@email.com";
            party.Cpn = "Cpn";
            party.Credentials = [new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCServicesCard }];
        });
        var client = A.Fake<IPlrClient>().ReturningAStandingsDigest(digest);
        var keycloak = A.Fake<IKeycloakAdministrationClient>()
            .ReturningTrueWhenAssigingClientRoles();
        var handler = this.MockDependenciesFor<NpdpEforms.CommandHandler>(client, keycloak);

        var result = await handler.HandleAsync(new NpdpEforms.Command { PartyId = party.Id });

        Assert.True(result.IsSuccess);
        A.CallTo(() => keycloak.AssignAccessRoles(A<Guid>._, MohKeycloakEnrolment.NpdpEforms)).MustHaveHappened();
    }

    [Theory]
    [MemberData(nameof(EndorsementStandingTestData))]
    public async Task CreateNpdpEformsEnrolment_NoCpn_UsesEndorsementStanding(PlrStandingsDigest endorsementDigest, bool expected)
    {
        // An MOA has no CPN of their own and qualifies only via an endorsement from a
        // Medical Doctor, a Nurse, or a Midwife - the professions that hold the card themselves.
        var party = this.TestDb.HasAParty(party =>
        {
            party.Email = "Email@email.com";
            party.Cpn = null;
            party.Credentials = [new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCServicesCard }];
        });
        var client = A.Fake<IPlrClient>().ReturningAStandingsDigest(endorsementDigest);
        var keycloak = A.Fake<IKeycloakAdministrationClient>()
            .ReturningTrueWhenAssigingClientRoles();
        var handler = this.MockDependenciesFor<NpdpEforms.CommandHandler>(client, keycloak);

        var result = await handler.HandleAsync(new NpdpEforms.Command { PartyId = party.Id });

        Assert.Equal(expected, result.IsSuccess);
        if (!expected)
        {
            keycloak.AssertNoRolesAssigned();
        }
    }

    public static TheoryData<PlrStandingsDigest, bool> EndorsementStandingTestData() => new()
    {
        { AMock.StandingsDigest(true, providerRoleType: ProviderRoleType.MedicalDoctor), true },
        { AMock.StandingsDigest(true, IdentifierType.Nurse), true },
        { AMock.StandingsDigest(true, IdentifierType.Midwife), true },
        { AMock.StandingsDigest(true, IdentifierType.Pharmacist), false },
        { AMock.StandingsDigest(false, providerRoleType: ProviderRoleType.MedicalDoctor), false },
        { PlrStandingsDigest.FromEmpty(), false },
    };

    [Fact]
    public async Task CreateNpdpEformsEnrolment_AlreadyEnroled_Denied()
    {
        var party = this.TestDb.HasAParty(party =>
        {
            party.Email = "Email@email.com";
            party.Cpn = "Cpn";
            party.Credentials = [new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCServicesCard }];
            party.AccessRequests = [new AccessRequest { AccessTypeCode = AccessTypeCode.NpdpEforms, RequestedOn = Instant.FromUtc(2026, 1, 1, 0, 0) }];
        });
        var client = A.Fake<IPlrClient>()
            .ReturningAStandingsDigest(true, IdentifierType.PhysiciansAndSurgeons);
        var keycloak = A.Fake<IKeycloakAdministrationClient>()
            .ReturningTrueWhenAssigingClientRoles();
        var handler = this.MockDependenciesFor<NpdpEforms.CommandHandler>(client, keycloak);

        var result = await handler.HandleAsync(new NpdpEforms.Command { PartyId = party.Id });

        Assert.False(result.IsSuccess);
        keycloak.AssertNoRolesAssigned();
    }

    [Fact]
    public async Task CreateNpdpEformsEnrolment_NoBcscCredential_Denied()
    {
        // The role can only be granted to a BC Services Card credential, so a Party
        // holding only a BC Provider credential must be denied rather than silently
        // succeeding with no role assigned anywhere.
        var party = this.TestDb.HasAParty(party =>
        {
            party.Email = "Email@email.com";
            party.Cpn = "Cpn";
            party.Credentials = [new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCProvider }];
        });
        var client = A.Fake<IPlrClient>()
            .ReturningAStandingsDigest(true, IdentifierType.PhysiciansAndSurgeons);
        var keycloak = A.Fake<IKeycloakAdministrationClient>()
            .ReturningTrueWhenAssigingClientRoles();
        var handler = this.MockDependenciesFor<NpdpEforms.CommandHandler>(client, keycloak);

        var result = await handler.HandleAsync(new NpdpEforms.Command { PartyId = party.Id });

        Assert.False(result.IsSuccess);
        keycloak.AssertNoRolesAssigned();
        Assert.DoesNotContain(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
    }

    [Fact]
    public async Task CreateNpdpEformsEnrolment_NoEmail_Denied()
    {
        var party = this.TestDb.HasAParty(party =>
        {
            party.Email = null;
            party.Cpn = "Cpn";
            party.Credentials = [new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCServicesCard }];
        });
        var client = A.Fake<IPlrClient>()
            .ReturningAStandingsDigest(true, IdentifierType.PhysiciansAndSurgeons);
        var keycloak = A.Fake<IKeycloakAdministrationClient>()
            .ReturningTrueWhenAssigingClientRoles();
        var handler = this.MockDependenciesFor<NpdpEforms.CommandHandler>(client, keycloak);

        var result = await handler.HandleAsync(new NpdpEforms.Command { PartyId = party.Id });

        Assert.False(result.IsSuccess);
        keycloak.AssertNoRolesAssigned();
    }
}
