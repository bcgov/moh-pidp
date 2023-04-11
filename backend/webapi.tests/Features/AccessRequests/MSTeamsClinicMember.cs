namespace PidpTests.Features.AccessRequests;

using NodaTime;
using Xunit;

using static Pidp.Features.AccessRequests.MSTeamsClinicMember;
using Pidp.Models;
using Pidp.Models.Lookups;
using PidpTests.TestingExtensions;

public class MSTeamsClinicMemberTests : InMemoryDbTest
{
    [Fact]
    public async void Create_WithProfileNoEndorsements_Fail()
    {
        var party = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "first";
            party.LastName = "last";
            party.Birthdate = LocalDate.FromDateTime(DateTime.Today);
            party.Email = "email@email.com";
            party.Phone = "5555555555";
        });
        var handler = this.MockDependenciesFor<CommandHandler>();

        var result = await handler.HandleAsync(new Command { PartyId = party.Id });

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async void Create_WithEndorsementNotClinicPrivacyOfficer_Fail()
    {
        var party = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "first";
            party.LastName = "last";
            party.Birthdate = LocalDate.FromDateTime(DateTime.Today);
            party.Email = "email@email.com";
            party.Phone = "5555555555";
        });
        var endorsee = this.TestDb.HasAParty();
        this.TestDb.Has(new Endorsement
        {
            Active = true,
            EndorsementRelationships = new[]
            {
                new EndorsementRelationship { PartyId = party.Id },
                new EndorsementRelationship { PartyId = endorsee.Id }
            }
        });
        var privacyOfficer = this.TestDb.HasAParty();
        var clinic = this.TestDb.Has(new MSTeamsClinic
        {
            PrivacyOfficerId = privacyOfficer.Id,
            Name = "clinic name",
            Address = new MSTeamsClinicAddress
            {
                CountryCode = "CA",
                ProvinceCode = "BC",
                Street = "street",
                Postal = "x8x8x8",
                City = "city"
            }
        });
        var handler = this.MockDependenciesFor<CommandHandler>();

        var result = await handler.HandleAsync(new Command { PartyId = party.Id, ClinicId = clinic.Id });

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async void Create_WithEndorsementFromPrivacyOfficer_Success()
    {
        var party = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "first";
            party.LastName = "last";
            party.Birthdate = LocalDate.FromDateTime(DateTime.Today);
            party.Email = "email@email.com";
            party.Phone = "5555555555";
        });
        var privacyOfficer = this.TestDb.HasAParty();
        var clinic = this.TestDb.Has(new MSTeamsClinic
        {
            PrivacyOfficerId = privacyOfficer.Id,
            Name = "clinic name",
            Address = new MSTeamsClinicAddress
            {
                CountryCode = "CA",
                ProvinceCode = "BC",
                Street = "street",
                Postal = "x8x8x8",
                City = "city"
            }
        });
        this.TestDb.Has(new Endorsement
        {
            Active = true,
            EndorsementRelationships = new[]
            {
                new EndorsementRelationship { PartyId = party.Id },
                new EndorsementRelationship { PartyId = privacyOfficer.Id }
            }
        });
        var handler = this.MockDependenciesFor<CommandHandler>();

        var result = await handler.HandleAsync(new Command { PartyId = party.Id, ClinicId = clinic.Id });

        Assert.True(result.IsSuccess);

        var accessRequest = this.TestDb.MSTeamsClinicMemberEnrolments
            .Where(request => request.PartyId == party.Id
                && request.AccessTypeCode == AccessTypeCode.MSTeamsClinicMember);
        Assert.Single(accessRequest);
        Assert.Equal(clinic.Id, accessRequest.Single().ClinicId);
    }
}
