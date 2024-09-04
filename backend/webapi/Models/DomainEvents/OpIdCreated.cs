namespace Pidp.Models.DomainEvents;

public record OpIdCreated(Guid UserId, string OpId) : IDomainEvent { }
