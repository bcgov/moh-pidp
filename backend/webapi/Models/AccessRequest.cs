namespace Pidp.Models;

using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum AccessType
{
    SAEforms = 1,
    HcimReEnrolment
}

[Table(nameof(AccessRequest))]
public class AccessRequest : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public int PartyId { get; set; }

    public Party? Party { get; set; }

    public Instant RequestedOn { get; set; }

    public AccessType AccessType { get; set; }
}

[Table(nameof(HcimReEnrolmentAccessRequest))]
public class HcimReEnrolmentAccessRequest : AccessRequest
{
    public string LdapUsername { get; set; } = string.Empty;
}
