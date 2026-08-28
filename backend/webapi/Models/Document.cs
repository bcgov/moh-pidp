namespace Pidp.Models;

using System.ComponentModel.DataAnnotations;

public class Document : BaseAuditable
{
    [Key]
    public Guid Id { get; set; }
    public byte[] Data { get; set; } = [];
    public string ContentType { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
}
