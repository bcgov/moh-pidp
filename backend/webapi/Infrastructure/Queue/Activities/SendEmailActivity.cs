namespace Pidp.Infrastructure.Queue.Activities;

using MassTransit;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.Queue.Events;
using Pidp.Infrastructure.Services;
using System.Threading.Tasks;

public class SendEmailActivity(IEmailService emailService) : IStateMachineActivity<BCProviderSagaState, KeycloakUserUpdatedEvent>
{
    private readonly IEmailService emailService = emailService;

    public void Probe(ProbeContext context) => context.CreateScope("send-email");

    public void Accept(StateMachineVisitor visitor) => visitor.Visit(this);

    public async Task Execute(BehaviorContext<BCProviderSagaState, KeycloakUserUpdatedEvent> context, IBehavior<BCProviderSagaState, KeycloakUserUpdatedEvent> next)
    {
        var message = context.Message;
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
}
