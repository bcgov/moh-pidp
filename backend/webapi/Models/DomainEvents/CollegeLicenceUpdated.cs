namespace Pidp.Models.DomainEvents;
public record CollegeLicenceUpdated(int PartyId, Guid UserId) : IDomainEvent;
