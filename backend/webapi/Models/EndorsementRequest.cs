namespace Pidp.Models;

using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum EndorsementRequestStatus
{
    Created = 1,
    Received,
    Approved,
    Declined,
    Confirmed,
    Cancelled
}

[Table(nameof(EndorsementRequest))]
public class EndorsementRequest : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public int RequestingPartyId { get; set; }

    public Party? RequestingParty { get; set; }

    public int? ReceivingPartyId { get; set; }

    public Party? ReceivingParty { get; set; }

    public Guid Token { get; set; }

    public string RecipientEmail { get; set; } = string.Empty;

    public string? AdditionalInformation { get; set; }

    public EndorsementRequestStatus Status { get; set; }

    public Instant StatusDate { get; set; }
}
