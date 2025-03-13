namespace Pidp.Infrastructure.Queue;

public class BCProviderCreatedEvent
{
    public required string OpId { get; set; }
    public int PartyId { get; set; }
    public Guid UserId { get; set; }
    public bool SAEformsEnroled { get; set; }
    public required string Email { get; set; }
    public required string UserPrincipalName { get; set; }
}
