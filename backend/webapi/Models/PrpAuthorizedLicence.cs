namespace Pidp.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// College Licence Numbers from the College  of Physicians and Surgeons that have been pre-authorized to use the Provider Reporting Portal enrolment.
/// </summary>
[Table(nameof(PrpAuthorizedLicence))]
public class PrpAuthorizedLicence
{
    [Key]
    public int Id { get; set; }
    public string LicenceNumber { get; set; } = string.Empty;
    public bool Claimed { get; set; }
}
