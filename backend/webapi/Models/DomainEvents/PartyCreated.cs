namespace Pidp.Models.DomainEvents;

public record PartyCreated(Guid UserId, string OpId) : IDomainEvent { }
