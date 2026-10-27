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
            Assert.Contains(this.TestDb.BusinessEvents, businessEvent => businessEvent is AccessRequestSubmitted);
        }
        else
        {
            keycloak.AssertNoRolesAssigned();
            Assert.DoesNotContain(this.TestDb.AccessRequests, request => request.PartyId == party.Id);
            Assert.Contains(this.TestDb.BusinessEvents, businessEvent => businessEvent is AccessRequestFailed);
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
    public async Task CreateNpdpEformsEnrolment_PharmacyTechnician_Denied()
    {
        // Pharmacy Technicians carry their own identifier type (PHTID) and are deliberately outside this
        // card, the same line Special Authority eForms draws. Named rather than left to the theory above
        // because it is a business decision, not a side effect of the list.
        var party = this.TestDb.HasAParty(party =>
        {
            party.Email = "Email@email.com";
            party.Cpn = "Cpn";
            party.Credentials = [new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCServicesCard }];
        });
        var client = A.Fake<IPlrClient>().ReturningAStandingsDigest(true, IdentifierType.PharmacyTech);
        var keycloak = A.Fake<IKeycloakAdministrationClient>()
            .ReturningTrueWhenAssigingClientRoles();
        var handler = this.MockDependenciesFor<NpdpEforms.CommandHandler>(client, keycloak);

        var result = await handler.HandleAsync(new NpdpEforms.Command { PartyId = party.Id });

        Assert.False(result.IsSuccess);
        keycloak.AssertNoRolesAssigned();
    }

    [Fact]
    public async Task CreateNpdpEformsEnrolment_NoCpn_Denied()
    {
        // The card is granted on the Party's own pharmacist licence and has no endorsement path, so a
        // Party without a CPN cannot qualify however well their endorsers are standing.
        var party = this.TestDb.HasAParty(party =>
        {
            party.Email = "Email@email.com";
            party.Cpn = null;
            party.Credentials = [new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCServicesCard }];
        });
        // The endorsement digest is armed with a Pharmacist in good standing precisely to show it is
        // never consulted; arming it with nothing would prove less.
        var client = A.Fake<IPlrClient>().ReturningMultipleStandingsDigests(
            PlrStandingsDigest.FromEmpty(),
            AMock.StandingsDigest(true, IdentifierType.Pharmacist));
        var keycloak = A.Fake<IKeycloakAdministrationClient>()
            .ReturningTrueWhenAssigingClientRoles();
        var handler = this.MockDependenciesFor<NpdpEforms.CommandHandler>(client, keycloak);

        var result = await handler.HandleAsync(new NpdpEforms.Command { PartyId = party.Id });

        Assert.False(result.IsSuccess);
        keycloak.AssertNoRolesAssigned();
        A.CallTo(() => client.GetAggregateStandingsDigestAsync(A<IEnumerable<string?>>._)).MustNotHaveHappened();
    }

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
            .ReturningAStandingsDigest(true, IdentifierType.Pharmacist);
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
            .ReturningAStandingsDigest(true, IdentifierType.Pharmacist);
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
            .ReturningAStandingsDigest(true, IdentifierType.Pharmacist);
        var keycloak = A.Fake<IKeycloakAdministrationClient>()
            .ReturningTrueWhenAssigingClientRoles();
        var handler = this.MockDependenciesFor<NpdpEforms.CommandHandler>(client, keycloak);

        var result = await handler.HandleAsync(new NpdpEforms.Command { PartyId = party.Id });

        Assert.False(result.IsSuccess);
        keycloak.AssertNoRolesAssigned();
    }

    [Fact]
    public async Task CreateNpdpEformsEnrolment_AlsoGrantsSAEforms()
    {
        // No licence check is needed in this direction: the card is Pharmacists only, so anyone who
        // gets it qualifies for Special Authority too.
        var bcsc = new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCServicesCard };
        var bcProvider = new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCProvider };
        var party = this.TestDb.HasAParty(party =>
        {
            party.Email = "Email@email.com";
            party.Cpn = "Cpn";
            party.Credentials = [bcsc, bcProvider];
        });
        var client = A.Fake<IPlrClient>().ReturningAStandingsDigest(true, IdentifierType.Pharmacist);
        var keycloak = A.Fake<IKeycloakAdministrationClient>().ReturningTrueWhenAssigingClientRoles();
        var handler = this.MockDependenciesFor<NpdpEforms.CommandHandler>(client, keycloak);

        var result = await handler.HandleAsync(new NpdpEforms.Command { PartyId = party.Id });

        Assert.True(result.IsSuccess);
        Assert.Contains(this.TestDb.AccessRequests, request => request.PartyId == party.Id
            && request.AccessTypeCode == AccessTypeCode.SAEforms);
        // Special Authority reaches BC Provider credentials as well, unlike NPDP itself.
        A.CallTo(() => keycloak.AssignAccessRoles(bcsc.UserId, MohKeycloakEnrolment.SAEforms)).MustHaveHappened();
        A.CallTo(() => keycloak.AssignAccessRoles(bcProvider.UserId, MohKeycloakEnrolment.SAEforms)).MustHaveHappened();
        // ...while NPDP itself reaches only the BC Services Card one.
        A.CallTo(() => keycloak.AssignAccessRoles(bcProvider.UserId, MohKeycloakEnrolment.NpdpEforms)).MustNotHaveHappened();
    }

    [Fact]
    public async Task CreateNpdpEformsEnrolment_AlreadyHoldsSAEforms_DoesNotGrantItTwice()
    {
        var party = this.TestDb.HasAParty(party =>
        {
            party.Email = "Email@email.com";
            party.Cpn = "Cpn";
            party.Credentials = [new Credential { UserId = Guid.NewGuid(), IdentityProvider = IdentityProviders.BCServicesCard }];
            party.AccessRequests = [new AccessRequest { AccessTypeCode = AccessTypeCode.SAEforms, RequestedOn = Instant.FromUtc(2026, 1, 1, 0, 0) }];
        });
        var client = A.Fake<IPlrClient>().ReturningAStandingsDigest(true, IdentifierType.Pharmacist);
        var keycloak = A.Fake<IKeycloakAdministrationClient>().ReturningTrueWhenAssigingClientRoles();
        var handler = this.MockDependenciesFor<NpdpEforms.CommandHandler>(client, keycloak);

        var result = await handler.HandleAsync(new NpdpEforms.Command { PartyId = party.Id });

        Assert.True(result.IsSuccess);
        Assert.Single(this.TestDb.AccessRequests, request => request.AccessTypeCode == AccessTypeCode.SAEforms);
        A.CallTo(() => keycloak.AssignAccessRoles(A<Guid>._, MohKeycloakEnrolment.SAEforms)).MustNotHaveHappened();
    }
}
