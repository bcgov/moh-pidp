namespace PidpTests.Features.AccessRequests;

using FakeItEasy;
using NodaTime;
using Xunit;

using static Pidp.Features.AccessRequests.MSTeamsClinicMember;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.Services;
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
    public async void Create_WithEndorsementFromPrivacyOfficer_SuccessWithEmails()
    {
        var party = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "partyfirst";
            party.LastName = "partylast";
            party.Birthdate = LocalDate.FromDateTime(DateTime.Today).PlusYears(-20);
            party.Email = "partyemail@testemail.com";
            party.Phone = "5555545555";
        });
        var privacyOfficer = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "privacyfirst";
            party.LastName = "officerlast";
            party.Email = "privacy@officer.com";
        });
        var clinic = this.TestDb.Has(new MSTeamsClinic
        {
            PrivacyOfficerId = privacyOfficer.Id,
            Name = "clinic name!",
            Address = new MSTeamsClinicAddress
            {
                CountryCode = "CA",
                ProvinceCode = "BC",
                Street = "streetz",
                Postal = "x8x8x8",
                City = "city44"
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
        var emailService = AMock.EmailService();
        var testClock = AMock.Clock(Instant.FromUnixTimeSeconds(123456));

        var handler = this.MockDependenciesFor<CommandHandler>(emailService, testClock);

        var result = await handler.HandleAsync(new Command { PartyId = party.Id, ClinicId = clinic.Id });

        Assert.True(result.IsSuccess);
        var accessRequest = this.TestDb.MSTeamsClinicMemberEnrolments
            .Where(request => request.PartyId == party.Id
                && request.AccessTypeCode == AccessTypeCode.MSTeamsClinicMember);
        Assert.Single(accessRequest);
        Assert.Equal(clinic.Id, accessRequest.Single().ClinicId);

        Assert.Equal(2, emailService.SentEmails.Count);
        var privacyOfficerEmail = emailService.SentEmails.SingleOrDefault(email => email.To.Contains(privacyOfficer.Email));
        Assert.NotNull(privacyOfficerEmail);
        Assert.Contains(party.FullName, privacyOfficerEmail.Body);

        var enrolmentEmailBody = emailService.SentEmails.Single(email => email != privacyOfficerEmail).Body;
        var expectedEnrolmentDate = testClock.GetCurrentInstant().InZone(DateTimeZoneProviders.Tzdb.GetZoneOrNull("America/Vancouver")!).Date;
        Assert.Contains(expectedEnrolmentDate.ToString(), enrolmentEmailBody);
        Assert.Contains(privacyOfficer.FullName, enrolmentEmailBody);
        Assert.Contains(party.FirstName, enrolmentEmailBody);
        Assert.Contains(party.LastName, enrolmentEmailBody);
        Assert.Contains(party.Birthdate.ToString()!, enrolmentEmailBody);
        Assert.Contains(party.Email!, enrolmentEmailBody);
        Assert.Contains(party.Phone!, enrolmentEmailBody);
        Assert.Contains(clinic.Name, enrolmentEmailBody);
        Assert.Contains(clinic.Address!.CountryCode, enrolmentEmailBody);
        Assert.Contains(clinic.Address.ProvinceCode, enrolmentEmailBody);
        Assert.Contains(clinic.Address.City, enrolmentEmailBody);
        Assert.Contains(clinic.Address.Postal, enrolmentEmailBody);
        Assert.Contains(clinic.Address.Street, enrolmentEmailBody);
    }
}
