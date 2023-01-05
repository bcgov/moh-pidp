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
    public async void GetEndorsementData_NoParty_404()
    {
        this.TestDb.Has(APartyWith(id: 1, hpdid: "hpdid"));
        var query = new EndorsementData.Query { Hpdid = "NoMatch" };
        var handler = this.MockDependenciesFor<EndorsementData.QueryHandler>();

        var result = await handler.HandleAsync(query);

        Assert.Equal(DomainOperationStatus.NotFound, result.Status);
    }

    [Fact]
    public async void GetEndorsementData_NoEndorsements_Empty()
    {
        this.TestDb.Has(APartyWith(id: 1, hpdid: "hpdid"));
        var query = new EndorsementData.Query { Hpdid = "hpdid" };
        var handler = this.MockDependenciesFor<EndorsementData.QueryHandler>();

        var result = await handler.HandleAsync(query);

        Assert.True(result.IsSuccess);
        Assert.Equal(new List<EndorsementData.Model>(), result.Value);
    }

    [Fact]
    public async void GetEndorsementData_OneEndorsementNotActive_Empty()
    {
        var expectedHpdid = "expected";
        this.TestDb.Has(APartyWith(id: 1, hpdid: "hpdid"));
        this.TestDb.Has(APartyWith(id: 2, hpdid: expectedHpdid));
        this.TestDb.Has(new Endorsement
        {
            Active = false,
            EndorsementRelationships = new[]
            {
                new EndorsementRelationship { PartyId = 1 },
                new EndorsementRelationship { PartyId = 2 }
            }
        });
        var query = new EndorsementData.Query { Hpdid = "hpdid" };
        var handler = this.MockDependenciesFor<EndorsementData.QueryHandler>();

        var result = await handler.HandleAsync(query);

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }

    [Theory]
    [InlineData("hpdid")]
    [InlineData("hpdid@bcsc")]
    public async void GetEndorsementData_OneEndorsementNoCpn_MatchingHpdidEmptyLicences(string partyHpdid)
    {
        var expectedHpdid = "expected";
        this.TestDb.Has(APartyWith(id: 1, hpdid: partyHpdid));
        this.TestDb.Has(APartyWith(id: 2, hpdid: expectedHpdid));
        this.TestDb.Has(new Endorsement
        {
            Active = true,
            EndorsementRelationships = new[]
            {
                new EndorsementRelationship { PartyId = 1 },
                new EndorsementRelationship { PartyId = 2 }
            }
        });
        var query = new EndorsementData.Query { Hpdid = "hpdid" }; // Should match either "hpdid" or "hpdid@bcsc" (the actual form for HPDID we get from Keycloak)
        var handler = this.MockDependenciesFor<EndorsementData.QueryHandler>();

        var result = await handler.HandleAsync(query);

        Assert.True(result.IsSuccess);
        Assert.Single(result.Value);

        var model = result.Value.Single();
        Assert.Equal(expectedHpdid, model.Hpdid);
        Assert.NotNull(model.Licences);
        Assert.Empty(model.Licences);
    }

    [Fact]
    public async void GetEndorsementData_OneEndorsementWithCpn_MatchingHpdidWithLicences()
    {
        var expectedHpdid = "expected";
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
        this.TestDb.Has(APartyWith(id: 1, hpdid: "hpdid"));
        this.TestDb.Has(APartyWith(id: 2, hpdid: expectedHpdid, cpn: expectedCpn));
        this.TestDb.Has(new Endorsement
        {
            Active = true,
            EndorsementRelationships = new[]
            {
                new EndorsementRelationship { PartyId = 1 },
                new EndorsementRelationship { PartyId = 2 }
            }
        });
        var query = new EndorsementData.Query { Hpdid = "hpdid" };
        var client = A.Fake<IPlrClient>();
        A.CallTo(() => client.GetRecordsAsync(expectedCpn))
            .Returns(expectedPlrRecords);
        var handler = this.MockDependenciesFor<EndorsementData.QueryHandler>(client);

        var result = await handler.HandleAsync(query);

        Assert.True(result.IsSuccess);
        Assert.Single(result.Value);

        var model = result.Value.Single();
        Assert.Equal(expectedHpdid, model.Hpdid);
        Assert.NotNull(model.Licences);
        AssertThat.CollectionsAreEquivalent(expectedPlrRecords, model.Licences, RecordModelPredicate);
    }

    [Fact]
    public async void GetEndorsementData_TwoEndorsementsWithCpn_MatchingHpdidsWithLicences()
    {
        var expectedHpdid1 = "expected1";
        var expectedHpdid2 = "expected2";
        var expectedCpn1 = "cpn";
        var expectedPlrRecords1 = new[]
        {
            new PlrRecord
            {
                Cpn = expectedCpn1,
                IdentifierType = "IdentifierType1",
                StatusCode = "StatusCode1",
                StatusReasonCode = "StatusReasonCode1"
            },
            new PlrRecord
            {
                Cpn = expectedCpn1,
                IdentifierType = "IdentifierType2",
                StatusCode = "StatusCode2",
                StatusReasonCode = "StatusReasonCode2"
            }
        };

        this.TestDb.Has(APartyWith(id: 1, hpdid: "hpdid"));
        this.TestDb.Has(APartyWith(id: 2, hpdid: expectedHpdid1, cpn: expectedCpn1));
        this.TestDb.Has(APartyWith(id: 3, hpdid: expectedHpdid2));
        this.TestDb.Has(new Endorsement
        {
            Active = true,
            EndorsementRelationships = new[]
            {
                new EndorsementRelationship { PartyId = 1 },
                new EndorsementRelationship { PartyId = 2 }
            }
        });
        this.TestDb.Has(new Endorsement
        {
            Active = true,
            EndorsementRelationships = new[]
            {
                new EndorsementRelationship { PartyId = 1 },
                new EndorsementRelationship { PartyId = 3 }
            }
        });
        var query = new EndorsementData.Query { Hpdid = "hpdid" };
        var client = A.Fake<IPlrClient>();
        A.CallTo(() => client.GetRecordsAsync(expectedCpn1, null))
            .Returns(expectedPlrRecords1);
        A.CallTo(() => client.GetRecordsAsync(expectedCpn1))
            .Returns(expectedPlrRecords1);
        var handler = this.MockDependenciesFor<EndorsementData.QueryHandler>(client);

        var result = await handler.HandleAsync(query);

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Count);

        var found = result.Value.SingleOrDefault(res => res.Hpdid == expectedHpdid1);
        Assert.NotNull(found);
        AssertThat.CollectionsAreEquivalent(expectedPlrRecords1, found!.Licences, RecordModelPredicate);

        found = result.Value.SingleOrDefault(res => res.Hpdid == expectedHpdid2);
        Assert.NotNull(found);
        AssertThat.CollectionsAreEquivalent(Array.Empty<PlrRecord>(), found!.Licences, RecordModelPredicate);
    }

    public static Party APartyWith(int id, string hpdid, string? cpn = null)
    {
        return new Party
        {
            Id = id,
            Cpn = cpn,
            Credentials = new List<Credential>
            {
                new Credential
                {
                    IdentityProvider = IdentityProviders.BCServicesCard,
                    IdpId = hpdid
                }
            }
        };
    }
}
