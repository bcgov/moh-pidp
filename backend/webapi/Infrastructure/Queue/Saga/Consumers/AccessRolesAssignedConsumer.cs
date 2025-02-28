namespace Pidp.Infrastructure.Queue;

using MassTransit;
using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Models;
using Pidp.Models.DomainEvents;
using System.Threading.Tasks;

public class AccessRolesAssignedConsumer(IKeycloakAdministrationClient keycloakClient, PidpDbContext context) : IConsumer<AccessRolesAssignedEvent>
{
    private readonly IKeycloakAdministrationClient keycloakClient = keycloakClient;
    private readonly PidpDbContext context = context;

    public async Task Consume(ConsumeContext<AccessRolesAssignedEvent> context)
    {
        var command = context.Message;
        if (command.SAEformsEnroled)
        {
            await this.keycloakClient.AssignAccessRoles(command.UserId, MohKeycloakEnrolment.SAEforms);
        }

        this.context.Credentials.Add(new Credential
        {
            UserId = command.UserId,
            PartyId = command.PartyId.GetHashCode(),
            IdpId = command.UserPrincipalName,
            IdentityProvider = IdentityProviders.BCProvider,
            DomainEvents = [new CollegeLicenceUpdated(command.PartyId.GetHashCode())]
        });

        await context.Publish(new BCProviderEmailSentEvent
        {
            PartyId = command.PartyId,
            UserPrincipalName = command.UserPrincipalName
        });
    }
}
