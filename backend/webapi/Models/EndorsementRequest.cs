namespace Pidp.Models;

using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(EndorsementRequest))]
public class EndorsementRequest : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public int RequestingPartyId { get; set; }

    public Party? RequestingParty { get; set; }

    public int? EndorsingPartyId { get; set; }

    public Party? EndorsingParty { get; set; }

    public Guid Token { get; set; }

    public string RecipientEmail { get; set; } = string.Empty;

    public string JobTitle { get; set; } = string.Empty;

    public bool? Approved { get; set; }

    public Instant? AdjudicatedOn { get; set; }
}
