namespace Pidp.Infrastructure.Queue;

using MassTransit;
using Pidp.Infrastructure.HttpClients.Keycloak;

public class BCProviderSaga : MassTransitStateMachine<BCProviderSagaState>
{
    public required State AssigningAccessRoles { get; set; }
    public required State SendingEmail { get; set; }
    public required State CreateCredential { get; set; }
    public required State Completed { get; set; }
    public required Event<BCProviderCreatedEvent> KeycloakUserUpdated { get; set; }

    private readonly IKeycloakAdministrationClient keycloakClient;

    public BCProviderSaga(IKeycloakAdministrationClient keycloakClient)
    {
        this.keycloakClient = keycloakClient;

        this.InstanceState(x => x.CurrentState);

        this.Event(() => this.KeycloakUserUpdated, x => x.CorrelateById(context => ConvertIntToGuid(context.Message.PartyId)));

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
                .TransitionTo(this.CreateCredential)
                .Activity(x => x.OfType<CreateCredentialActivity>())
                .TransitionTo(this.SendingEmail)
                .Activity(x => x.OfType<SendEmailActivity>())
                .TransitionTo(this.Completed));
    }

    private static Guid ConvertIntToGuid(int value)
    {
        var bytes = new byte[16];
        BitConverter.GetBytes(value).CopyTo(bytes, 0);
        return new Guid(bytes);
    }
}

