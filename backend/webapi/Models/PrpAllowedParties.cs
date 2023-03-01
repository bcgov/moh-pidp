namespace Pidp.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// A list of Parties that have been pre-approved to use the Provider Reporting Portal enrolment. Found using Licence Number (from the college of Physicians and Surgeons).
/// </summary>
[Table(nameof(PrpAllowedParty))]
public class PrpAllowedParty
{
    [Key]
    public int Id { get; set; }
    public string LicenceNumber { get; set; } = string.Empty;
    public bool Claimed { get; set; }
}
