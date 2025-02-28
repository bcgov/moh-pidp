namespace Pidp.Infrastructure.Queue;

using MassTransit;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.Services;

public class SendBCProviderCreationEmailConsumer(IEmailService emailService) : IConsumer<BCProviderEmailSentEvent>
{
    private readonly IEmailService emailService = emailService;

    public async Task Consume(ConsumeContext<BCProviderEmailSentEvent> context)
    {
        var command = context.Message;
        await this.SendBCProviderCreationEmail(command.Email, command.UserPrincipalName);

        await context.Publish(new BCProviderEmailSentEvent
        {
            PartyId = command.PartyId,
            UserPrincipalName = command.UserPrincipalName
        });
    }

    private async Task SendBCProviderCreationEmail(string partyEmail, string userPrincipalName)
    {
        var email = new Email(
            from: EmailService.PidpEmail,
            to: partyEmail,
            subject: "BCProvider Account Creation in OneHealthID Confirmation",
            body: $"You have successfully created a BCProvider account in OneHealthID Service. For your reference, your BCProvider username is {userPrincipalName}. You may now login to OneHealthID Service and access the BCProvider card to update your BCProvider password."
        );

        await this.emailService.SendAsync(email);
    }
}
