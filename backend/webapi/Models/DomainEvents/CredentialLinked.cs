namespace Pidp.Models.DomainEvents;

public record CredentialLinked(Credential Credential) : IDomainEvent;
