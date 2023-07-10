namespace Pidp.Models.DomainEvents;

public record PartyEmailUpdated(int PartyId, Guid UserId, string NewEmail) : IDomainEvent { }
