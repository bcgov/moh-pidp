namespace Pidp.Infrastructure.Queue;

using MassTransit;
using Pidp.Infrastructure.HttpClients.Keycloak;
using System.Threading.Tasks;

public class KeycloakUserUpdatedConsumer(IKeycloakAdministrationClient keycloakClient) : IConsumer<KeycloakUserUpdatedEvent>
{
    private readonly IKeycloakAdministrationClient keycloakClient = keycloakClient;

    public async Task Consume(ConsumeContext<KeycloakUserUpdatedEvent> context)
    {
        var command = context.Message;
        await this.keycloakClient.UpdateUser(command.UserId, user => user.SetOpId(command.PartyId.ToString()));

        await context.Publish(new AccessRolesAssignedEvent
        {
            PartyId = command.PartyId,
            UserId = command.UserId
        });
    }
}
