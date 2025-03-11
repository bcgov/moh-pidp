namespace Pidp.Infrastructure.Queue.Events;

public class KeycloakUserUpdatedEvent
{
    public required string OpId { get; set; }
    public Guid PartyId { get; set; }
    public Guid UserId { get; set; }
    public bool SAEformsEnroled { get; set; }
    public required string Email { get; set; }
    public required string UserPrincipalName { get; set; }
}
