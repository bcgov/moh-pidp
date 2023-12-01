namespace EndorsementReminderTests;

using FakeItEasy;
using MassTransit;
using NodaTime;
using Xunit;

using EndorsementReminder;
using Pidp;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.Services;
using Pidp.Models;
using PidpTests;
using PidpTests.TestingExtensions;

public class EndorsementReminderServiceTests : InMemoryDbTest
{
    [Theory]
    [MemberData(nameof(ScheduleTestCases))]
    public async void EndorsementReminderService_DoWorkAsync_SendsEmailsOnSchedule(EndorsementRequestStatus status, Instant statusDate, Instant scheduleDate, bool emailExpected)
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

        var clockMock = A.Fake<IClock>();
        A.CallTo(() => clockMock.GetCurrentInstant()).Returns(scheduleDate);
        var emailServiceMock = A.Fake<IEmailService>();
        Email capturedEmail = null!;
        A.CallTo(() => emailServiceMock.SendAsync(A<Email>._)).Invokes(i => capturedEmail = i.GetArgument<Email>(0)!);
        var reminderService = this.MockDependenciesFor<EndorsementReminderService>(clockMock, emailServiceMock);

        await reminderService.DoWorkAsync();

        if (emailExpected)
        {
            A.CallTo(() => emailServiceMock.SendAsync(An<Email>._)).MustHaveHappenedOnceExactly();

            if (status == EndorsementRequestStatus.Created)
            {
                Assert.Contains(endorsementRequest.Token.ToString(), capturedEmail.Body);
                Assert.Equal(capturedEmail.To.Single(), endorsementRequest.RecipientEmail);
            }
            else if (status == EndorsementRequestStatus.Received)
            {
                Assert.Equal(capturedEmail.To.Single(), endorsementRequest.RecipientEmail);
            }
            else
            {
                Assert.Equal(capturedEmail.To.Single(), requestingParty.Email);
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
    public async void EndorsementReminderService_DoWorkAsync_ShouldNotSendDuplicateEmails()
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

        var clockMock = A.Fake<IClock>();
        A.CallTo(() => clockMock.GetCurrentInstant()).Returns(now);
        var emailServiceMock = A.Fake<IEmailService>();
        var reminderService = this.MockDependenciesFor<EndorsementReminderService>(clockMock, emailServiceMock);

        await reminderService.DoWorkAsync();

        A.CallTo(() => emailServiceMock.SendAsync(An<Email>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async void EndorsementReminderService_DoWorkAsync_ShouldNotSendIfAlreadyEndorsed()
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

        // TODO:
    }
}
