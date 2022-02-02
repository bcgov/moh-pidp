namespace Pidp.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Pidp.Models.Lookups;

[Table(nameof(PartyCertification))]
public class PartyCertification : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public int PartyId { get; set; }

    public Party? Party { get; set; }

    public CollegeCode CollegeCode { get; set; }

    public College? College { get; set; }

    [RegularExpression(@"([a-zA-Z0-9]+)", ErrorMessage = "License Number should be alpha numeric characters")]
    public string LicenceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Internal Party Code, PLR's unique identifier for a certification
    /// </summary>
    public string? Ipc { get; set; }
}
