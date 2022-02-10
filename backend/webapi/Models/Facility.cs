namespace Pidp.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(Facility))]
public class Facility : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public int PartyId { get; set; }

    public Party? Party { get; set; }

    public string Name { get; set; } = string.Empty;

    public FacilityAddress? PhysicalAddress { get; set; }
}
