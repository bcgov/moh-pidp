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

    public static Endorsement FromCompletedRequest(EndorsementRequest request)
    {
        if (request.Status != EndorsementRequestStatus.Completed
            || request.ReceivingPartyId == null)
        {
            throw new InvalidOperationException();
        }

        return new Endorsement
        {
            Active = true,
            CreatedOn = request.StatusDate,
            EndorsementRelationships = new List<EndorsementRelationship>
            {
                new EndorsementRelationship { PartyId = request.RequestingPartyId },
                new EndorsementRelationship { PartyId = request.ReceivingPartyId.Value }
            }
        };
    }
}
