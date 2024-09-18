namespace Pidp.Models;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NodaTime;

[Table(nameof(Banners))]
public class Banners : BaseAuditable
{
    [Key]
    public int Id { get; set; }
    public string Header { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public Instant StartTime { get; set; }
    public Instant EndTime { get; set; }
}
