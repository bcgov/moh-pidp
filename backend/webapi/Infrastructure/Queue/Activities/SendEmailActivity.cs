namespace Pidp.Infrastructure.Queue;

using MassTransit;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.Services;
using System.Threading.Tasks;

public class SendEmailActivity(IEmailService emailService) : IStateMachineActivity<BCProviderSagaState, BCProviderCreatedEvent>
{
    private readonly IEmailService emailService = emailService;

    public void Probe(ProbeContext context) => context.CreateScope("send-email");

    public void Accept(StateMachineVisitor visitor) => visitor.Visit(this);

    public async Task Execute(BehaviorContext<BCProviderSagaState, BCProviderCreatedEvent> context, IBehavior<BCProviderSagaState, BCProviderCreatedEvent> next)
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

    public Task Faulted<TException>(BehaviorExceptionContext<BCProviderSagaState, BCProviderCreatedEvent, TException> context, IBehavior<BCProviderSagaState, BCProviderCreatedEvent> next) where TException : Exception => next.Faulted(context);
}
