namespace Pidp.Models;

using NodaTime;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public enum BannerStatus
{
    Info = 1,
    Warning = 2,
    Error = 3,
    Success = 4
}

[Table(nameof(Banner))]
public class Banner : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public string Component { get; set; } = string.Empty;

    public BannerStatus Status { get; set; }

    public string Header { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public Instant StartTime { get; set; }

    public Instant EndTime { get; set; }
}
