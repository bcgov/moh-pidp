namespace Pidp.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(ClientLog))]
public class ClientLog : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public string Message { get; set; } = string.Empty;

    public LogLevel LogLevel { get; set; }

    public string? AdditionalInformation { get; set; }
}
