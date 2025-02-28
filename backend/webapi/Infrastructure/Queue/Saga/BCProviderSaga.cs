namespace Pidp.Infrastructure.Queue;

using MassTransit;

public class BCProviderSaga : MassTransitStateMachine<BCProviderSagaState>
{
    public BCProviderSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => KeycloakUserUpdated, x => x.CorrelateById(context => context.Message.PartyId));
        Event(() => AccessRolesAssigned, x => x.CorrelateById(context => context.Message.PartyId));
        Event(() => BCProviderEmailSent, x => x.CorrelateById(context => context.Message.PartyId));

        Initially(
            When(KeycloakUserUpdated)
                .Then(context =>
                {
                    context.Instance.PartyId = context.Data.PartyId;
                    context.Instance.UserId = context.Data.UserId;
                })
                .TransitionTo(AssigningAccessRoles)
                .Publish(context => new AccessRolesAssignedEvent
                {
                    PartyId = context.Instance.PartyId,
                    UserId = context.Instance.UserId,
                    SAEformsEnroled = context.Instance.SAEformsEnroled
                }));

        During(AssigningAccessRoles,
            When(AccessRolesAssigned)
                .Then(context =>
                {
                    context.Instance.UserId = context.Data.UserId;
                })
                .TransitionTo(SendingEmail)
                .Publish(context => new BCProviderEmailSentEvent
                {
                    PartyId = context.Instance.PartyId,
                    Email = context.Instance.Email,
                    UserPrincipalName = context.Instance.UserPrincipalName
                }));

        During(SendingEmail,
            When(BCProviderEmailSent)
                .TransitionTo(Completed));
    }

    public State AssigningAccessRoles { get; private set; }
    public State SendingEmail { get; private set; }
    public State Completed { get; private set; }

    public Event<KeycloakUserUpdatedEvent> KeycloakUserUpdated { get; private set; }
    public Event<AccessRolesAssignedEvent> AccessRolesAssigned { get; private set; }
    public Event<BCProviderEmailSentEvent> BCProviderEmailSent { get; private set; }
}
