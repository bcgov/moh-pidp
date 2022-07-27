namespace Pidp.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Pidp.Models.Lookups;

[Table(nameof(PartyLicenceDeclaration))]
public class PartyLicenceDeclaration : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public int PartyId { get; set; }

    public Party? Party { get; set; }

    public CollegeCode? CollegeCode { get; set; }

    public College? College { get; set; }

    [RegularExpression(@"([a-zA-Z0-9]+)", ErrorMessage = "Licence Number should be alpha numeric characters")]
    public string? LicenceNumber { get; set; } = string.Empty;
}
