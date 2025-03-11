namespace Pidp.Infrastructure.Queue;

using MassTransit;
using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.Services;
using Pidp.Models;
using Pidp.Models.DomainEvents;
using System.Threading.Tasks;

public class SendEmailActivity(IEmailService emailService, PidpDbContext dbContext) : IStateMachineActivity<BCProviderSagaState, KeycloakUserUpdatedEvent>
{
    private readonly IEmailService emailService = emailService;
    private readonly PidpDbContext dbContext = dbContext;

    public void Probe(ProbeContext context) => context.CreateScope("send-email");

    public void Accept(StateMachineVisitor visitor) => visitor.Visit(this);

    public async Task Execute(BehaviorContext<BCProviderSagaState, KeycloakUserUpdatedEvent> context, IBehavior<BCProviderSagaState, KeycloakUserUpdatedEvent> next)
    {
        var message = context.Message;
        this.dbContext.Credentials.Add(new Credential
        {
            UserId = message.UserId,
            PartyId = ConvertGuidToInt(message.PartyId),
            IdpId = message.UserPrincipalName,
            IdentityProvider = IdentityProviders.BCProvider,
            DomainEvents = [new CollegeLicenceUpdated(ConvertGuidToInt(message.PartyId))]
        });

        await this.dbContext.SaveChangesAsync();

        var email = new Email(
            from: EmailService.PidpEmail,
            to: message.Email,
            subject: "BCProvider Account Creation in OneHealthID Confirmation",
            body: $"You have successfully created a BCProvider account in OneHealthID Service. For your reference, your BCProvider username is {message.UserPrincipalName}. You may now login to OneHealthID Service and access the BCProvider card to update your BCProvider password."
        );

        await this.emailService.SendAsync(email);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<BCProviderSagaState, KeycloakUserUpdatedEvent, TException> context, IBehavior<BCProviderSagaState, KeycloakUserUpdatedEvent> next) where TException : Exception => next.Faulted(context);

    private static int ConvertGuidToInt(Guid guid)
    {
        var bytes = guid.ToByteArray();
        var hash = BitConverter.ToInt32(bytes, 0);
        return hash;
    }
}
