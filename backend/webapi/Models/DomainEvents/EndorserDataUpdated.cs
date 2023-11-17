namespace Pidp.Models.DomainEvents;

public record EndorserDataUpdated(int PartyId) : IDomainEvent;
