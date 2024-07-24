namespace Pidp.Models;

using System.Text.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table(nameof(FhirMessage))]
public class FhirMessage : IDisposable
{
    public FhirMessage()
    {
    }
    [Key]
    public int Id { get; set; }
    public JsonDocument? MessageBody { get; set; }

    public void Dispose() => MessageBody?.Dispose();
}
