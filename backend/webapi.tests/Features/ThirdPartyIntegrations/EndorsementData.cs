namespace PidpTests.Features.ThirdPartyIntegrations;

using Xunit;
using DomainResults.Common;
using FakeItEasy;

using Pidp.Features.ThirdPartyIntegrations;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using PidpTests.TestingExtensions;
using Pidp.Infrastructure.Auth;

public class EndorsementDataTests : InMemoryDbTest
{
    private static Func<PlrRecord, EndorsementData.Model.LicenceInformation, bool> RecordModelPredicate => (PlrRecord record, EndorsementData.Model.LicenceInformation result) => record.IdentifierType == result.IdentifierType
        && record.StatusCode == result.StatusCode
        && record.StatusReasonCode == result.StatusReasonCode;

    [Fact]
    public async Task GetEndorsementData_NoParty_404()
    {
        this.TestDb.Has(APartyWith(id: 1, hpdid: "hpdid@bcsc"));
        var query = new EndorsementData.Query { Hpdid = "NoMatch" };
        var handler = this.MockDependenciesFor<EndorsementData.QueryHandler>();

        var result = await handler.HandleAsync(query);

        Assert.Equal(DomainOperationStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task GetEndorsementData_NoEndorsements_Empty()
    {
        this.TestDb.Has(APartyWith(id: 1, hpdid: "hpdid@bcsc"));
        var query = new EndorsementData.Query { Hpdid = "hpdid" };
        var handler = this.MockDependenciesFor<EndorsementData.QueryHandler>();

        var result = await handler.HandleAsync(query);

        Assert.True(result.IsSuccess);
        Assert.Equal([], result.Value);
    }

    [Fact]
    public async Task GetEndorsementData_OneEndorsementNotActive_Empty()
    {
        this.TestDb.Has(APartyWith(id: 1, hpdid: "hpdid@bcsc"));
        this.TestDb.Has(APartyWith(id: 2, hpdid: "anotherHpdid@bcsc"));
        this.TestDb.Has(AnEndorsementBetween(1, 2, isActive: false));
        var query = new EndorsementData.Query { Hpdid = "hpdid" };
        var handler = this.MockDependenciesFor<EndorsementData.QueryHandler>();

        var result = await handler.HandleAsync(query);

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }

    [Theory]
    [InlineData("hpdid")]
    [InlineData("hpdid@bcsc")]
    public async Task GetEndorsementData_OneEndorsementNoCpn_MatchingHpdidEmptyLicences(string hpdidQuery)
    {
        this.TestDb.Has(APartyWith(id: 1, hpdid: "hpdid@bcsc"));
        var otherParty = this.TestDb.Has(APartyWith(id: 2, hpdid: "anotherHpdid@bcsc"));
        this.TestDb.Has(AnEndorsementBetween(1, 2));
        var query = new EndorsementData.Query { Hpdid = hpdidQuery }; // Should match either "hpdid" or "hpdid@bcsc" (the actual form for HPDID we get from Keycloak and save in our database)
        var handler = this.MockDependenciesFor<EndorsementData.QueryHandler>();

        var result = await handler.HandleAsync(query);

        Assert.True(result.IsSuccess);
        Assert.Single(result.Value);

        var model = result.Value.Single();
        AssertBasicInfoMatches(otherParty, model);
        Assert.NotNull(model.Licences);
        Assert.Empty(model.Licences);
    }

    [Fact]
    public async Task GetEndorsementData_OneEndorsementWithCpn_MatchingHpdidWithLicences()
    {
        var expectedCpn = "cpn";
        var expectedPlrRecords = new[]
        {
            new PlrRecord
            {
                Cpn = expectedCpn,
                IdentifierType = "IdentifierType1",
                StatusCode = "StatusCode1",
                StatusReasonCode = "StatusReasonCode1"
            },
            new PlrRecord
            {
                Cpn = expectedCpn,
                IdentifierType = "IdentifierType2",
                StatusCode = "StatusCode2",
                StatusReasonCode = "StatusReasonCode2"
            }
        };
        this.TestDb.Has(APartyWith(id: 1, hpdid: "hpdid@bcsc"));
        var otherParty = this.TestDb.Has(APartyWith(id: 2, hpdid: "otherHpdid@bcsc", cpn: expectedCpn));
        this.TestDb.Has(AnEndorsementBetween(1, 2));
        var query = new EndorsementData.Query { Hpdid = "hpdid" };
        var client = A.Fake<IPlrClient>();
        A.CallTo(() => client.GetRecordsAsync(expectedCpn))
            .Returns(expectedPlrRecords);
        var handler = this.MockDependenciesFor<EndorsementData.QueryHandler>(client);

        var result = await handler.HandleAsync(query);

        Assert.True(result.IsSuccess);
        Assert.Single(result.Value);

        var model = result.Value.Single();
        AssertBasicInfoMatches(otherParty, model);
        Assert.NotNull(model.Licences);
        AssertThat.CollectionsAreEquivalent(expectedPlrRecords, model.Licences, RecordModelPredicate);
    }

    [Fact]
    public async Task GetEndorsementData_TwoEndorsementsWithCpn_MatchingHpdidsWithLicences()
    {
        var expectedCpn = "cpn";
        var expectedPlrRecords = new[]
        {
            new PlrRecord
            {
                Cpn = expectedCpn,
                IdentifierType = "IdentifierType1",
                StatusCode = "StatusCode1",
                StatusReasonCode = "StatusReasonCode1"
            },
            new PlrRecord
            {
                Cpn = expectedCpn,
                IdentifierType = "IdentifierType2",
                StatusCode = "StatusCode2",
                StatusReasonCode = "StatusReasonCode2"
            }
        };

        this.TestDb.Has(APartyWith(id: 1, hpdid: "hpdid@bcsc"));
        var licencedOtherParty = this.TestDb.Has(APartyWith(id: 2, hpdid: "otherHpdid@bcsc", cpn: expectedCpn));
        var unlicencedOtherParty = this.TestDb.Has(APartyWith(id: 3, hpdid: "otherHpdid2@bcsc"));
        this.TestDb.Has(AnEndorsementBetween(1, 2));
        this.TestDb.Has(AnEndorsementBetween(1, 3));
        var query = new EndorsementData.Query { Hpdid = "hpdid" };
        var client = A.Fake<IPlrClient>();
        A.CallTo(() => client.GetRecordsAsync(expectedCpn, null))
            .Returns(expectedPlrRecords);
        var handler = this.MockDependenciesFor<EndorsementData.QueryHandler>(client);

        var result = await handler.HandleAsync(query);

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Count);

        var found = result.Value.SingleOrDefault(res => res.Hpdid == licencedOtherParty.Credentials.Single().Hpdid);
        Assert.NotNull(found);
        AssertBasicInfoMatches(licencedOtherParty, found);
        AssertThat.CollectionsAreEquivalent(expectedPlrRecords, found.Licences, RecordModelPredicate);

        found = result.Value.SingleOrDefault(res => res.Hpdid == unlicencedOtherParty.Credentials.Single().Hpdid);
        Assert.NotNull(found);
        AssertBasicInfoMatches(unlicencedOtherParty, found);
        Assert.NotNull(found.Licences);
        Assert.Empty(found.Licences);
    }

    public static Party APartyWith(int id, string hpdid, string? cpn = null)
    {
        return new Party
        {
            Id = id,
            Cpn = cpn,
            FirstName = $"First{id}",
            LastName = $"Last{id}",
            Email = $"email@{id}.com",
            Credentials =
            [
                new Credential
                {
                    IdentityProvider = IdentityProviders.BCServicesCard,
                    IdpId = hpdid
                }
            ]
        };
    }

    public static Endorsement AnEndorsementBetween(int partyId1, int partyId2, bool isActive = true)
    {
        return new Endorsement
        {
            Active = isActive,
            EndorsementRelationships =
            [
                new EndorsementRelationship { PartyId = partyId1 },
                new EndorsementRelationship { PartyId = partyId2 }
            ]
        };
    }

    private static void AssertBasicInfoMatches(Party expected, EndorsementData.Model actual)
    {
        Assert.Equal(expected.Credentials.Single().Hpdid, actual.Hpdid);
        Assert.Equal(expected.FirstName, actual.FirstName);
        Assert.Equal(expected.LastName, actual.LastName);
        Assert.Equal(expected.Email, actual.Email);
    }
}
