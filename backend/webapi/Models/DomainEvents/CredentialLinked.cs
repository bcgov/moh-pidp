namespace Pidp.Models.DomainEvents;

using System.Security.Claims;

public record CredentialLinked(Credential Credential, ClaimsPrincipal User) : IDomainEvent;
