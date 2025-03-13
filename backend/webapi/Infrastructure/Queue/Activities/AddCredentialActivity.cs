namespace Pidp.Infrastructure.Queue;

using MassTransit;
using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Models;
using Pidp.Models.DomainEvents;
using System.Threading.Tasks;

public class CreateCredentialActivity(PidpDbContext dbContext) : IStateMachineActivity<BCProviderSagaState, BCProviderCreatedEvent>
{
    private readonly PidpDbContext dbContext = dbContext;

    public void Probe(ProbeContext context) => context.CreateScope("add-credential");

    public void Accept(StateMachineVisitor visitor) => visitor.Visit(this);

    public async Task Execute(BehaviorContext<BCProviderSagaState, BCProviderCreatedEvent> context, IBehavior<BCProviderSagaState, BCProviderCreatedEvent> next)
    {
        var message = context.Message;

        this.dbContext.Credentials.Add(new Credential
        {
            UserId = message.UserId,
            PartyId = message.PartyId,
            IdpId = message.UserPrincipalName,
            IdentityProvider = IdentityProviders.BCProvider,
            DomainEvents = [new CollegeLicenceUpdated(message.PartyId)]
        });

        await this.dbContext.SaveChangesAsync();

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<BCProviderSagaState, BCProviderCreatedEvent, TException> context, IBehavior<BCProviderSagaState, BCProviderCreatedEvent> next) where TException : Exception => next.Faulted(context);
}
