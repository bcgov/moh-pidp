namespace Pidp.Models.DomainEvents;

public record PlrCpnLookupFound(int PartyId, IEnumerable<Guid> UserIds, string Cpn) : IDomainEvent
{
    public PlrCpnLookupFound(int partyId, Guid userId, string cpn) : this(partyId, new[] { userId }, cpn) { }
}

public record PlrCpnLookupNotFound(int PartyId) : IDomainEvent { }
