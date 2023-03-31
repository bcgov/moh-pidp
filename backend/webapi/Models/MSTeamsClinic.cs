namespace Pidp.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(MSTeamsClinic))]
public class MSTeamsClinic : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public MSTeamsClinicAddress? Address { get; set; }
}
