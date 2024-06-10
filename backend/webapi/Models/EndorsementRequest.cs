namespace Pidp.Models;

using DomainResults.Common;
using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Pidp.Features.EndorsementRequests;

public enum EndorsementRequestStatus
{
    Created = 1,
    Received,
    Approved,
    Declined,
    Completed,
    Cancelled,
    Expired
}

[Table(nameof(EndorsementRequest))]
public class EndorsementRequest : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public int RequestingPartyId { get; set; }

    public Party? RequestingParty { get; set; }

    public int? ReceivingPartyId { get; set; }

    public Party? ReceivingParty { get; set; }

    public Guid Token { get; set; }

    public string RecipientEmail { get; set; } = string.Empty;

    public string? AdditionalInformation { get; set; }

    public EndorsementRequestStatus Status { get; set; }

    public Instant StatusDate { get; set; }

    public IDomainResult Handle(Approve.Command command, IClock clock)
    {
        if (this.ActionableByReciever(command.PartyId))
        {
            this.Status = EndorsementRequestStatus.Approved;
        }
        else if (this.ActionableByRequester(command.PartyId))
        {
            this.Status = EndorsementRequestStatus.Completed;
        }
        else
        {
            return DomainResult.Unauthorized();
        }

        this.StatusDate = clock.GetCurrentInstant();
        return DomainResult.Success();
    }

    public IDomainResult Handle(Decline.Command command, IClock clock)
    {
        if (this.ActionableByReciever(command.PartyId))
        {
            this.Status = EndorsementRequestStatus.Declined;
        }
        else if (this.ActionableByRequester(command.PartyId))
        {
            this.Status = EndorsementRequestStatus.Cancelled;
        }
        else
        {
            return DomainResult.Unauthorized();
        }

        this.StatusDate = clock.GetCurrentInstant();
        return DomainResult.Success();
    }

    private bool ActionableByReciever(int partyId) => this.ReceivingPartyId == partyId && this.Status == EndorsementRequestStatus.Received;
    private bool ActionableByRequester(int partyId) => this.RequestingPartyId == partyId && this.Status == EndorsementRequestStatus.Approved;
}
