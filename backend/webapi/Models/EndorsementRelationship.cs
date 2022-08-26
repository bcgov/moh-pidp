namespace Pidp.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(EndorsementRelationship))]
public class EndorsementRelationship : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public int PartyId { get; set; }

    public Party? Party { get; set; }

    public int EndorsementId { get; set; }

    public Endorsement? Endorsement { get; set; }
}
