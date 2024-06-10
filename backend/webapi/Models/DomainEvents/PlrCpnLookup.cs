namespace Pidp.Models.DomainEvents;

using Pidp.Infrastructure.HttpClients.Plr;

public record PlrCpnLookupFound(int PartyId, IEnumerable<Guid> UserIds, string Cpn, PlrStandingsDigest StandingsDigest) : IDomainEvent
{
    public PlrCpnLookupFound(int partyId, Guid userId, string cpn, PlrStandingsDigest standingsDigest) : this(partyId, new[] { userId }, cpn, standingsDigest) { }
}

public record PlrCpnLookupNotFound(int PartyId) : IDomainEvent { }
