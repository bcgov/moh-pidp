namespace Pidp.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pidp.Models.Lookups;

public class PharmacyEnrolment : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(Pharmacy))]
    public int PharmacyId { get; set; }
    public Pharmacy Pharmacy { get; set; } = default!;

    public Guid Token { get; set; }

    public PharmacyRole Role { get; set; }

    public DateTime? EffectiveStartDate { get; set; }
    public DateTime? EffectiveEndDate { get; set; }
}