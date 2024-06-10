namespace PidpTests.Features.AccessRequests;

using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Xunit;

using static Pidp.Features.AccessRequests.MSTeamsPrivacyOfficer;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Infrastructure.Services;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Models.Lookups;
using PidpTests.TestingExtensions;

public class MSTeamsPrivacyOfficerTests : InMemoryDbTest
{
    [Theory]
    [MemberData(nameof(MSTeamsIdentifierTypeTestData))]
    public async void CreateMSTeamsEnrolment_ValidProfileWithVaryingLicence_MatchesAllowedTypes(IdentifierType identifierType)
    {
        var party = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "FirstName";
            party.LastName = "LastName";
            party.Birthdate = LocalDate.FromDateTime(DateTime.Today);
            party.Email = "Email@email.com";
            party.Phone = "5551234567";
            party.Cpn = "Cpn";
        });
        var command = new Command
        {
            PartyId = party.Id,
            ClinicName = "clinic name",
            ClinicAddress = new Command.Address
            {
                CountryCode = "CA",
                ProvinceCode = "BC",
                Street = "streetz",
                Postal = "x8x8x8",
                City = "city44"
            }
        };
        var client = A.Fake<IPlrClient>()
            .ReturningAStandingsDigest(true, identifierType);
        var handler = this.MockDependenciesFor<CommandHandler>(client);

        var result = await handler.HandleAsync(command);

        Assert.Equal(AllowedIdentifierTypes.Contains(identifierType), result.IsSuccess);
    }

    public static IEnumerable<object[]> MSTeamsIdentifierTypeTestData()
    {
        return TestData.AllIdentifierTypes
             .Select(identifierType => new object[] { identifierType });
    }

    [Fact]
    public async void CreateMSTeamsEnrolment_ValidProfileWithValidLicence_EmailsSent()
    {
        var party = this.TestDb.HasAParty(party =>
        {
            party.FirstName = "Privacci";
            party.LastName = "Offiker";
            party.Birthdate = LocalDate.FromDateTime(DateTime.Today).PlusYears(-20);
            party.Email = "Email@domain.com";
            party.Phone = "5551234567";
            party.Cpn = "Cpn7";
        });
        var command = new Command
        {
            PartyId = party.Id,
            ClinicName = "clinic name",
            ClinicAddress = new Command.Address
            {
                CountryCode = "CA",
                ProvinceCode = "BC",
                Street = "streetz",
                Postal = "x8x8x8",
                City = "city44"
            }
        };
        var client = A.Fake<IPlrClient>()
            .ReturningAStandingsDigest(true, AllowedIdentifierTypes[0]);
        var emailService = AMock.EmailService();
        var testClock = AMock.Clock(Instant.FromUnixTimeSeconds(123456));

        var handler = this.MockDependenciesFor<CommandHandler>(client, emailService, testClock);

        var result = await handler.HandleAsync(command);

        Assert.True(result.IsSuccess);
        Assert.Single(this.TestDb.AccessRequests
            .Where(request => request.PartyId == party.Id
                && request.AccessTypeCode == AccessTypeCode.MSTeamsPrivacyOfficer));
        var clinic = this.TestDb.MSTeamsClinics
            .Include(clinic => clinic.Address)
            .SingleOrDefault(clinic => clinic.PrivacyOfficerId == party.Id);
        Assert.NotNull(clinic);
        Assert.Equal(command.ClinicName, clinic.Name);
        Assert.NotNull(clinic.Address);
        Assert.Equal(command.ClinicAddress.CountryCode, clinic.Address.CountryCode);
        Assert.Equal(command.ClinicAddress.ProvinceCode, clinic.Address.ProvinceCode);
        Assert.Equal(command.ClinicAddress.Street, clinic.Address.Street);
        Assert.Equal(command.ClinicAddress.Postal, clinic.Address.Postal);
        Assert.Equal(command.ClinicAddress.City, clinic.Address.City);

        Assert.Equal(2, emailService.SentEmails.Count);
        var confirmationEmail = emailService.SentEmails.SingleOrDefault(email => email.To.Contains(party.Email));
        Assert.NotNull(confirmationEmail);
        Assert.Contains(party.FullName, confirmationEmail.Body);

        var enrolmentEmailBody = emailService.SentEmails.Single(email => email != confirmationEmail).Body;
        var expectedEnrolmentDate = testClock.GetCurrentInstant().InZone(DateTimeZoneProviders.Tzdb.GetZoneOrNull("America/Vancouver")!).Date;
        Assert.Contains(expectedEnrolmentDate.ToString(), enrolmentEmailBody);
        Assert.Contains(party.FirstName, enrolmentEmailBody);
        Assert.Contains(party.LastName, enrolmentEmailBody);
        Assert.Contains(party.Birthdate.ToString()!, enrolmentEmailBody);
        Assert.Contains(party.Email!, enrolmentEmailBody);
        Assert.Contains(party.Phone!, enrolmentEmailBody);
        Assert.Contains(command.ClinicName, enrolmentEmailBody);
        Assert.Contains(command.ClinicAddress!.CountryCode, enrolmentEmailBody);
        Assert.Contains(command.ClinicAddress.ProvinceCode, enrolmentEmailBody);
        Assert.Contains(command.ClinicAddress.City, enrolmentEmailBody);
        Assert.Contains(command.ClinicAddress.Postal, enrolmentEmailBody);
        Assert.Contains(command.ClinicAddress.Street, enrolmentEmailBody);
    }
}
