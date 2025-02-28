namespace Pidp.Infrastructure.Queue;

public class KeycloakUserUpdatedEvent
{
    public Guid PartyId { get; set; }
    public Guid UserId { get; set; }
    public string OpId { get; set; }
    public bool SAEformsEnroled { get; set; }
}
