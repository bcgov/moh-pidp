namespace Pidp.Models;

using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(InvitedEntraAccount))]
public class InvitedEntraAccount : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public int PartyId { get; set; }

    public Party? Party { get; set; }

    /// <summary>
    /// The UPN of the guest account in the BC Provider tenant
    /// </summary>
    public string UserPrincipalName { get; set; } = string.Empty;

    /// <summary>
    /// The UPN of the invited account in its home tenant
    /// </summary>
    public string InvitedUserPrincipalName { get; set; } = string.Empty;

    public Instant InvitedAt { get; set; }
}
