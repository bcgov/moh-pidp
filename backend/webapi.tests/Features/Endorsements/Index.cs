namespace PidpTests.Features.Endorsements;

using Xunit;

using static Pidp.Features.Endorsements.Index;
using Pidp.Models;
using Pidp.Models.Lookups;
using PidpTests.TestingExtensions;
using NodaTime;

public class EndorsmentIndexTests : InMemoryDbTest
{
    [Fact]
    public async void Get_WithNoEndorsements_EmptyList()
    {
        var party = this.TestDb.HasAParty();
        var handler = this.MockDependenciesFor<QueryHandler>();

        var result = await handler.HandleAsync(new Query { PartyId = party.Id });

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async void Get_WithActiveAndNonActiveEndorsements_OnlyActiveAppeaer()
    {
        var party = this.TestDb.HasAParty();
        var party2 = this.TestDb.HasAParty();
        var party3 = this.TestDb.HasAParty();
        var party4 = this.TestDb.HasAParty();

        this.TestDb.Has(new Endorsement
        {
            Active = true,
            EndorsementRelationships = new[]
            {
                new EndorsementRelationship { PartyId = party.Id },
                new EndorsementRelationship { PartyId = party2.Id }
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
        this.TestDb.Has(new Endorsement
        {
            Active = false,
            EndorsementRelationships = new[]
            {
                new EndorsementRelationship { PartyId = party.Id },
                new EndorsementRelationship { PartyId = party4.Id }
            }
        });
        var handler = this.MockDependenciesFor<QueryHandler>();

        var result = await handler.HandleAsync(new Query { PartyId = party.Id });

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async void Get_WithActiveEndorsement_AllFieldsAppear()
    {
        var party = this.TestDb.HasAParty();
        var party2 = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "Firrst";
            party.LastName = "Laast";
            party.LicenceDeclaration = new PartyLicenceDeclaration
            {
                CollegeCode = CollegeCode.NaturopathicPhysicians,
                LicenceNumber = "12345"
            };
        });
        var endorsement = this.TestDb.Has(new Endorsement
        {
            Active = true,
            CreatedOn = Instant.FromDateTimeOffset(DateTimeOffset.Now),
            EndorsementRelationships = new[]
            {
                new EndorsementRelationship { PartyId = party.Id },
                new EndorsementRelationship { PartyId = party2.Id }
            }
        });

        var handler = this.MockDependenciesFor<QueryHandler>();

        var results = await handler.HandleAsync(new Query { PartyId = party.Id });

        var result = results.Single();
        Assert.Equal(endorsement.Id, result.Id);
        Assert.Equal(party2.FullName, result.PartyName);
        Assert.Equal(party2.LicenceDeclaration!.CollegeCode, result.CollegeCode);
        Assert.Equal(endorsement.CreatedOn, result.CreatedOn);
    }
}
