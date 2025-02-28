namespace Pidp.Infrastructure.Queue;

public class AccessRolesAssignedEvent
{
    public Guid PartyId { get; set; }
    public Guid UserId { get; set; }

    public bool SAEformsEnroled { get; set; }
    public string UserPrincipalName { get; set; }
}
