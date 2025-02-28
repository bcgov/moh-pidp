namespace Pidp.Infrastructure.Queue;

public class BCProviderEmailSentEvent
{
    public Guid PartyId { get; set; }
    public string UserPrincipalName { get; set; }
    public string Email { get; set; }
}
