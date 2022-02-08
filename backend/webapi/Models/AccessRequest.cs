namespace Pidp.Models;

using System.ComponentModel.DataAnnotations;

using NodaTime;

public enum AccessType
{
    SAEforms = 1
}

public class AccessRequest : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public int PartyId { get; set; }

    public Party? Party { get; set; }

    public Instant RequestedOn { get; set; }

    public AccessType AccessType { get; set; }
}
