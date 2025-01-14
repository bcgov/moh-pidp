namespace Pidp.Models.DomainEvents;

public record PartyPhoneUpdated(int PartyId, IEnumerable<Guid> UserIds, string NewPhone) : IDomainEvent { }
