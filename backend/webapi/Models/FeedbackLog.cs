namespace Pidp.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(FeedbackLog))]
public class FeedbackLog : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public int PartyId { get; set; }
    public string Feedback { get; set; } = string.Empty;

    public string? AttachmentInformation { get; set; } = string.Empty;
}
