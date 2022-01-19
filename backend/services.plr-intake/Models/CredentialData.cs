namespace PlrIntake.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(CredentialData))]
public class CredentialData
{
    [Key]
    public int Id { get; set; }

    public int PlrRecordId { get; set; }

    public PlrRecord? PlrRecord { get; set; }

    public string? Value { get; set; }
}
