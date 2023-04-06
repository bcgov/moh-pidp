namespace PidpTests.Features.Endorsements;

using Xunit;


using static Pidp.Features.Endorsements.MSTeamsPrivacyOfficers;
using Pidp.Models;
using Pidp.Models.Lookups;
using PidpTests.TestingExtensions;

public class MSTeamsPrivacyOfficersTests : InMemoryDbTest
{
    [Fact]
    public async void Get_WithNoEndorsements_EmptyList()
    {
        var party = this.TestDb.HasAParty();
        var handler = this.MockDependenciesFor<QueryHandler>();

        var result = await handler.HandleAsync(new Query { PartyId = party.Id });

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async void Get_WithEndorsementNoPrivacyOfficer_EmptyList()
    {
        var party = this.TestDb.HasAParty();
        var party2 = this.TestDb.HasAParty();
        this.TestDb.Has(new Endorsement
        {
            Active = true,
            EndorsementRelationships = new[]
            {
                new EndorsementRelationship { PartyId = party.Id },
                new EndorsementRelationship { PartyId = party2.Id }
            }
        });
        var handler = this.MockDependenciesFor<QueryHandler>();

        var result = await handler.HandleAsync(new Query { PartyId = party.Id });

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async void Get_WithEndorsementsPrivacyOfficerEnrolmentNoClinic_EmptyList()
    {
        var party = this.TestDb.HasAParty();
        var party2 = this.TestDb.HasAParty(party => party.AccessRequests = new[] { new AccessRequest { AccessTypeCode = AccessTypeCode.MSTeamsPrivacyOfficer } });
        this.TestDb.Has(new Endorsement
        {
            Active = true,
            EndorsementRelationships = new[]
            {
                new EndorsementRelationship { PartyId = party.Id },
                new EndorsementRelationship { PartyId = party2.Id }
            }
        });
        var handler = this.MockDependenciesFor<QueryHandler>();

        var result = await handler.HandleAsync(new Query { PartyId = party.Id });

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async void Get_WithEndorsementsPrivacyOfficers_List()
    {
        var party = this.TestDb.HasAParty();

        var party2 = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "first";
            party.LastName = "party2";
        });
        var clinic2 = this.TestDb.Has(new MSTeamsClinic
        {
            PrivacyOfficerId = party2.Id,
            Name = "clinic2",
            Address = new MSTeamsClinicAddress
            {
                CountryCode = "CA",
                ProvinceCode = "BC",
                City = "city",
                Street = "123 street",
                Postal = "X1X1X1"
            }
        });
        this.TestDb.Has(new Endorsement
        {
            Active = true,
            EndorsementRelationships = new[]
            {
                new EndorsementRelationship { PartyId = party.Id },
                new EndorsementRelationship { PartyId = party2.Id }
            }
        });

        var party3 = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "second";
            party.LastName = "party3";
        });
        var clinic3 = this.TestDb.Has(new MSTeamsClinic
        {
            PrivacyOfficerId = party3.Id,
            Name = "clinic3",
            Address = new MSTeamsClinicAddress
            {
                CountryCode = "CA",
                ProvinceCode = "AB",
                City = "city3",
                Street = "321 street",
                Postal = "X2X2X2"
            }
        });
        this.TestDb.Has(new Endorsement
        {
            Active = true,
            EndorsementRelationships = new[]
            {
                new EndorsementRelationship { PartyId = party.Id },
                new EndorsementRelationship { PartyId = party3.Id }
            }
        });

        var expectedResult = new[]
        {
            new Model
            {
                FullName = $"{party2.FirstName} {party2.LastName}",
                ClinicId = clinic2.Id,
                ClinicName = clinic2.Name,
                ClinicAddress = new Model.Address
                {
                    CountryCode = clinic2.Address!.CountryCode,
                    ProvinceCode = clinic2.Address.ProvinceCode,
                    City = clinic2.Address.City,
                    Street = clinic2.Address.Street,
                    Postal = clinic2.Address.Postal
                }
            },
            new Model
            {
                FullName = $"{party3.FirstName} {party3.LastName}",
                ClinicId = clinic3.Id,
                ClinicName = clinic3.Name,
                ClinicAddress = new Model.Address
                {
                    CountryCode = clinic3.Address!.CountryCode,
                    ProvinceCode = clinic3.Address.ProvinceCode,
                    City = clinic3.Address.City,
                    Street = clinic3.Address.Street,
                    Postal = clinic3.Address.Postal
                }
            }
        };
        var handler = this.MockDependenciesFor<QueryHandler>();

        var result = await handler.HandleAsync(new Query { PartyId = party.Id });

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        AssertThat.CollectionsAreEquivalent(expectedResult, result.Value, (model1, model2) =>
            model1.FullName == model2.FullName
            && model1.ClinicId == model2.ClinicId
            && model1.ClinicName == model2.ClinicName
            && model1.ClinicAddress.CountryCode == model2.ClinicAddress.CountryCode
            && model1.ClinicAddress.ProvinceCode == model2.ClinicAddress.ProvinceCode
            && model1.ClinicAddress.City == model2.ClinicAddress.City
            && model1.ClinicAddress.Street == model2.ClinicAddress.Street
            && model1.ClinicAddress.Postal == model2.ClinicAddress.Postal);
    }
}
