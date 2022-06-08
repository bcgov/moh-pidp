namespace PlrIntake.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(StatusChageLog))]
public class StatusChageLog : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public int PlrRecordId { get; set; }

    public PlrRecord? PlrRecord { get; set; }

    public string? OldStatusCode { get; set; }

    public string? OldStatusReasonCode { get; set; }

    public string? NewStatusCode { get; set; }

    public string? NewStatusReasonCode { get; set; }
}
