namespace Pidp.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pidp.Models.Lookups;

public class PharmacyPartyRole : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(Party))]
    public int PartyId { get; set; }
    public Party Party { get; set; } = default!;

    [ForeignKey(nameof(Pharmacy))]
    public int PharmacyId { get; set; }
    public Pharmacy Pharmacy { get; set; } = default!;

    public PharmacyRole Role { get; set; }

    public DateTime? EffectiveStartDate { get; set; }
    public DateTime? EffectiveEndDate { get; set; }

    public DateTime? PrivacyTrainingAckDate { get; set; }
}