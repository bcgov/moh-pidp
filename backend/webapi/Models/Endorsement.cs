namespace Pidp.Models;

using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(Endorsement))]
public class Endorsement : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public bool Active { get; set; }

    public Instant CreatedOn { get; set; }

    public ICollection<EndorsementRelationship> EndorsementRelationships { get; set; } = new List<EndorsementRelationship>();
}
