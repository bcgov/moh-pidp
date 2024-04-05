namespace Pidp.Models.DomainEvents;

public record BCProviderCreated(int PartyId, string FirstName, string LastName, string UserPrincipalName, string OpId) : IDomainEvent;
