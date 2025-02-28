namespace Pidp.Infrastructure.Queue;

public class KeycloakUserUpdatedEvent
{
    public Guid PartyId { get; set; }
    public Guid UserId { get; set; }
}
