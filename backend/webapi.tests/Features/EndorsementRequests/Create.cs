namespace PidpTests.Features.EndorsementRequests;

using FakeItEasy;
using Xunit;

using Pidp.Features.EndorsementRequests;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Models;
using PidpTests.TestingExtensions;

public class EndorsementRequestCreateTests : InMemoryDbTest
{
    [Fact]
    public async Task Create_NotPreApproved_CreatedEmailSentToRecipient()
    {
        var requester = this.TestDb.HasAParty();
        var command = new Create.Command
        {
            PartyId = requester.Id,
            RecipientEmail = "email@emailz.com",
            AdditionalInformation = "addditional info",
            PreApproved = false
        };
        var emailService = AMock.EmailService();
        var handler = this.MockDependenciesFor<Create.CommandHandler>(emailService);

        var result = await handler.HandleAsync(command);

        Assert.False(result.DuplicateEndorsementRequest);

        var endorsementRequest = this.TestDb.EndorsementRequests.SingleOrDefault();
        Assert.NotNull(endorsementRequest);
        Assert.Equal(requester.Id, endorsementRequest.RequestingPartyId);
        Assert.Equal(command.RecipientEmail, endorsementRequest.RecipientEmail);
        Assert.Equal(command.AdditionalInformation, endorsementRequest.AdditionalInformation);
        Assert.NotEqual(Guid.Empty, endorsementRequest.Token);
        Assert.Equal(EndorsementRequestStatus.Created, endorsementRequest.Status);
        Assert.False(endorsementRequest.PreApproved);

        A.CallTo(() => emailService.SendAsync(An<Email>._)).MustHaveHappenedOnceExactly();
        var email = emailService.SentEmails.Single();
        Assert.Single(email.To);
        Assert.Equal(command.RecipientEmail, email.To.Single());
        Assert.Contains(endorsementRequest.Token.ToString(), email.Body);
    }

    [Fact]
    public async Task Create_PreApprovedOneMatchingEmailDifferentCase_RecievedEmailSentToRecipient()
    {
        var requester = this.TestDb.HasAParty();
        var reciever = this.TestDb.HasAParty(party => party.Email = "emailz@emal.com");
        var command = new Create.Command
        {
            PartyId = requester.Id,
            RecipientEmail = "Emailz@eMal.com",
            PreApproved = true
        };
        var emailService = AMock.EmailService();
        var handler = this.MockDependenciesFor<Create.CommandHandler>(emailService);

        var result = await handler.HandleAsync(command);

        Assert.False(result.DuplicateEndorsementRequest);

        var endorsementRequest = this.TestDb.EndorsementRequests.SingleOrDefault();
        Assert.NotNull(endorsementRequest);
        Assert.Equal(requester.Id, endorsementRequest.RequestingPartyId);
        Assert.Equal(reciever.Id, endorsementRequest.ReceivingPartyId);
        Assert.Equal(command.RecipientEmail, endorsementRequest.RecipientEmail);
        Assert.NotEqual(Guid.Empty, endorsementRequest.Token);
        Assert.Equal(EndorsementRequestStatus.Received, endorsementRequest.Status);
        Assert.True(endorsementRequest.PreApproved);

        A.CallTo(() => emailService.SendAsync(An<Email>._)).MustHaveHappenedOnceExactly();
        var email = emailService.SentEmails.Single();
        Assert.Single(email.To);
        Assert.Equal(command.RecipientEmail, email.To.Single());
        Assert.DoesNotContain(endorsementRequest.Token.ToString(), email.Body);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task Create_PreApprovedZeroOrManyMatchingEmails_CreatedEmailSentToRecipient(int numberOfMatchingEmails)
    {
        var requester = this.TestDb.HasAParty();
        var recipientEmail = "email@mail.com";
        for (var i = 0; i < numberOfMatchingEmails; i++)
        {
            this.TestDb.HasAParty(party => party.Email = recipientEmail);
        }
        var command = new Create.Command
        {
            PartyId = requester.Id,
            RecipientEmail = recipientEmail,
            PreApproved = true
        };
        var emailService = AMock.EmailService();
        var handler = this.MockDependenciesFor<Create.CommandHandler>(emailService);

        var result = await handler.HandleAsync(command);

        Assert.False(result.DuplicateEndorsementRequest);

        var endorsementRequest = this.TestDb.EndorsementRequests.SingleOrDefault();
        Assert.NotNull(endorsementRequest);
        Assert.Equal(requester.Id, endorsementRequest.RequestingPartyId);
        Assert.Null(endorsementRequest.ReceivingPartyId);
        Assert.Equal(command.RecipientEmail, endorsementRequest.RecipientEmail);
        Assert.NotEqual(Guid.Empty, endorsementRequest.Token);
        Assert.Equal(EndorsementRequestStatus.Created, endorsementRequest.Status);
        Assert.False(endorsementRequest.PreApproved);

        A.CallTo(() => emailService.SendAsync(An<Email>._)).MustHaveHappenedOnceExactly();
        var email = emailService.SentEmails.Single();
        Assert.Single(email.To);
        Assert.Equal(command.RecipientEmail, email.To.Single());
        Assert.Contains(endorsementRequest.Token.ToString(), email.Body);
    }
}
