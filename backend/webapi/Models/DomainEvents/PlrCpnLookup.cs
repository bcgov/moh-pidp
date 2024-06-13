namespace Pidp.Models.DomainEvents;

using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models.Lookups;

public record PlrCpnLookupFound(int PartyId, IEnumerable<Guid> UserIds, string Cpn, PlrStandingsDigest StandingsDigest) : IDomainEvent
{
    public PlrCpnLookupFound(int partyId, Guid userId, string cpn, PlrStandingsDigest standingsDigest) : this(partyId, [userId], cpn, standingsDigest) { }
}

public record PlrCpnLookupNotFound(int PartyId, CollegeCode CollegeCode, string LicenceNumber) : IDomainEvent { }
