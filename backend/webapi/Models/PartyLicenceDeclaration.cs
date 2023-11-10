namespace Pidp.Models;

using EntityFrameworkCore.Projectables;
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

    public string? LicenceNumber { get; set; }

    [Projectable]
    public bool IsUnlicensed => this.CollegeCode == null && this.LicenceNumber == null;
}
