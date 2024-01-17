namespace EndorsementReminderTests;

using FakeItEasy;
using NodaTime;
using Xunit;

using EndorsementReminder;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.Services;
using Pidp.Models;
using PidpTests;
using PidpTests.TestingExtensions;

public class EndorsementMaintenanceServiceTests : InMemoryDbTest
{
    [Theory]
    [MemberData(nameof(ExpiryScheduleTestCases))]
    public async void EndorsementMainenanceService_ExpireOldEndorsementRequestsAsync_ShouldNotSendDuplicateEmails(EndorsementRequestStatus status, Instant statusDate, Instant scheduleDate, bool expectedExpired)
    {
        var requestingParty = this.TestDb.HasAParty();
        requestingParty.Email = "requesting@email.com";
        var endorsementRequest = this.TestDb.Has(new EndorsementRequest
        {
            Token = Guid.NewGuid(),
            RequestingPartyId = requestingParty.Id,
            Status = status,
            StatusDate = statusDate,
            RecipientEmail = "recipient@email.com"
        });

        var clockMock = AMock.Clock(scheduleDate);
        var service = this.MockDependenciesFor<EndorsementMaintenanceService>(clockMock);

        await service.ExpireOldEndorsementRequestsAsync();

        Assert.Equal(expectedExpired ? EndorsementRequestStatus.Expired : status, endorsementRequest.Status);
        if (expectedExpired)
        {
            Assert.Equal(scheduleDate, endorsementRequest.StatusDate);
        }
    }

    public static IEnumerable<object[]> ExpiryScheduleTestCases()
    {
        // Scheduler runs at 8 AM on a monday
        var scheduleDate = new ZonedDateTime(new LocalDateTime(2023, 11, 13, 8, 0), DateTimeZoneProviders.Tzdb.GetZoneOrNull("America/Vancouver")!, Offset.FromHours(-8)).ToInstant();

        foreach (var status in TestData.AllValuesOf<EndorsementRequestStatus>())
        {
            foreach (var daysOld in Enumerable.Range(0, 32))
            {
                // We expect that the request will be marked as expired when the Request is 30 days or more old.
                var expected = daysOld >= 30 && status is EndorsementRequestStatus.Created or EndorsementRequestStatus.Received or EndorsementRequestStatus.Approved;

                yield return new object[] { status, scheduleDate - Duration.FromHours(1) - Duration.FromDays(daysOld), scheduleDate, expected };
            }
        }
    }

    [Theory]
    [MemberData(nameof(ScheduleTestCases))]
    public async void EndorsementMaintenanceService_SendReminderEmailsAsync_SendsEmailsOnSchedule(EndorsementRequestStatus status, Instant statusDate, Instant scheduleDate, bool emailExpected)
    {
        var requestingParty = this.TestDb.HasAParty();
        requestingParty.Email = "requesting@email.com";
        var endorsementRequest = this.TestDb.Has(new EndorsementRequest
        {
            Token = Guid.NewGuid(),
            RequestingPartyId = requestingParty.Id,
            Status = status,
            StatusDate = statusDate,
            RecipientEmail = "recipient@email.com"
        });

        var clockMock = AMock.Clock(scheduleDate);
        var emailServiceMock = AMock.EmailService();
        var service = this.MockDependenciesFor<EndorsementMaintenanceService>(clockMock, emailServiceMock);

        await service.SendReminderEmailsAsync();

        if (emailExpected)
        {
            A.CallTo(() => emailServiceMock.SendAsync(An<Email>._)).MustHaveHappenedOnceExactly();
            Assert.Single(emailServiceMock.SentEmails);
            var sentEmail = emailServiceMock.SentEmails.Single();

            if (status == EndorsementRequestStatus.Created)
            {
                Assert.Contains(endorsementRequest.Token.ToString(), sentEmail.Body);
                Assert.Equal(sentEmail.To.Single(), endorsementRequest.RecipientEmail);
            }
            else if (status == EndorsementRequestStatus.Received)
            {
                Assert.Equal(sentEmail.To.Single(), endorsementRequest.RecipientEmail);
            }
            else
            {
                Assert.Equal(sentEmail.To.Single(), requestingParty.Email);
            }
        }
        else
        {
            A.CallTo(() => emailServiceMock.SendAsync(An<Email>._)).MustNotHaveHappened();
        }
    }

    public static IEnumerable<object[]> ScheduleTestCases()
    {
        // Scheduler runs at 8 AM on a monday
        var scheduleDate = new ZonedDateTime(new LocalDateTime(2023, 11, 13, 8, 0), DateTimeZoneProviders.Tzdb.GetZoneOrNull("America/Vancouver")!, Offset.FromHours(-8)).ToInstant();

        foreach (var status in TestData.AllValuesOf<EndorsementRequestStatus>())
        {
            foreach (var daysOld in Enumerable.Range(0, 10))
            {
                // We expect that the email is sent when the Request is 7 over days old but less than 8 days old.
                var expected = daysOld == 7 && status is EndorsementRequestStatus.Created or EndorsementRequestStatus.Received or EndorsementRequestStatus.Approved;

                yield return new object[] { status, scheduleDate - Duration.FromHours(1) - Duration.FromDays(daysOld), scheduleDate, expected };
            }
        }
    }

    [Fact]
    public async void EndorsementMainenanceService_SendReminderEmailsAsync_ShouldNotSendDuplicateEmails()
    {
        var now = SystemClock.Instance.GetCurrentInstant();
        var requestingParty = this.TestDb.HasAParty();
        requestingParty.Email = "requesting@email.com";
        this.TestDb.Has(new EndorsementRequest
        {
            Token = Guid.NewGuid(),
            RequestingPartyId = requestingParty.Id,
            Status = EndorsementRequestStatus.Received,
            StatusDate = now - Duration.FromDays(7) - Duration.FromHours(1),
            RecipientEmail = "recipient@email.com"
        });
        this.TestDb.Has(new EndorsementRequest
        {
            Token = Guid.NewGuid(),
            RequestingPartyId = requestingParty.Id,
            Status = EndorsementRequestStatus.Received,
            StatusDate = now - Duration.FromDays(7) - Duration.FromHours(1),
            RecipientEmail = "recipient@email.com"
        });
        this.TestDb.Has(new EndorsementRequest
        {
            Token = Guid.NewGuid(),
            RequestingPartyId = requestingParty.Id,
            Status = EndorsementRequestStatus.Received,
            StatusDate = now - Duration.FromDays(7) - Duration.FromHours(1),
            RecipientEmail = "recipient@email.com"
        });

        var clockMock = AMock.Clock(now);
        var emailServiceMock = A.Fake<IEmailService>();
        var service = this.MockDependenciesFor<EndorsementMaintenanceService>(clockMock, emailServiceMock);

        await service.SendReminderEmailsAsync();

        A.CallTo(() => emailServiceMock.SendAsync(An<Email>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    // If a 7 day old Endorsment Request is between two users currently in an Active Endorsement, they should not get an email
    public async void EndorsementMaintenanceService_SendReminderEmailsAsync_ShouldNotSendIfAlreadyEndorsed()
    {
        var now = SystemClock.Instance.GetCurrentInstant();
        var requestingParty = this.TestDb.HasAParty();
        requestingParty.Email = "requesting@email.com";
        var recievingParty = this.TestDb.HasAParty();
        this.TestDb.Has(new EndorsementRequest
        {
            Token = Guid.NewGuid(),
            RequestingPartyId = requestingParty.Id,
            Status = EndorsementRequestStatus.Received,
            StatusDate = now - Duration.FromDays(7) - Duration.FromHours(1),
            RecipientEmail = "recipient@email.com"
        });
        this.TestDb.Has(new EndorsementRequest
        {
            Token = Guid.NewGuid(),
            RequestingPartyId = requestingParty.Id,
            ReceivingPartyId = recievingParty.Id,
            Status = EndorsementRequestStatus.Completed,
            StatusDate = now - Duration.FromDays(20),
            RecipientEmail = "recipient@email.com"
        });
        this.TestDb.Has(new Endorsement
        {
            Active = true,
            EndorsementRelationships = new List<EndorsementRelationship>
            {
                new() { PartyId = recievingParty.Id },
                new() { PartyId = requestingParty.Id }
            },
        });

        var clockMock = AMock.Clock(now);
        var emailServiceMock = A.Fake<IEmailService>();
        var service = this.MockDependenciesFor<EndorsementMaintenanceService>(clockMock, emailServiceMock);

        await service.SendReminderEmailsAsync();

        A.CallTo(() => emailServiceMock.SendAsync(An<Email>._)).MustNotHaveHappened();
    }
}
