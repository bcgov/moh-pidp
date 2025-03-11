namespace Pidp.Infrastructure.Services;

using MassTransit;
using Pidp.Infrastructure.Queue.Events;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.Queue;
using Pidp.Infrastructure.Queue.Activities;


public class BCProviderSagaService : MassTransitStateMachine<BCProviderSagaState>
{
    private readonly IKeycloakAdministrationClient keycloakClient;

    public BCProviderSagaService(IKeycloakAdministrationClient keycloakClient)
    {
        this.keycloakClient = keycloakClient;

        this.InstanceState(x => x.CurrentState);

        this.Event(() => this.KeycloakUserUpdated, x => x.CorrelateById(context => context.Message.PartyId));

        this.Initially(
            this.When(this.KeycloakUserUpdated)
                .Then(async context =>
                {
                    var message = context.Message;
                    await this.keycloakClient.UpdateUser(message.UserId, user => user.SetOpId(message.OpId));
                })
                .TransitionTo(this.AssigningAccessRoles)
                .Then(async context =>
                {
                    var message = context.Message;
                    if (message.SAEformsEnroled)
                    {
                        await this.keycloakClient.AssignAccessRoles(message.UserId, MohKeycloakEnrolment.SAEforms);
                    }
                })
                .TransitionTo(this.SendingEmail)
                .Activity(x => x.OfType<SendEmailActivity>())
                .TransitionTo(this.Completed));
    }

    public required State AssigningAccessRoles { get; set; }
    public required State SendingEmail { get; set; }
    public required State Completed { get; set; }

    public required Event<KeycloakUserUpdatedEvent> KeycloakUserUpdated { get; set; }
}

