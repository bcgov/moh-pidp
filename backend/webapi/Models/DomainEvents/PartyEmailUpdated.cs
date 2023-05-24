namespace Pidp.Models.DomainEvents;

public record PartyEmailUpdated(int PartyId, string NewEmail) : IDomainEvent { }
