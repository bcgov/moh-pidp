namespace Pidp.Models.DomainEvents;

using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;

public record PlrCpnLookupFound(int PartyId, IEnumerable<Guid> UserIds, string Cpn, PlrStandingsDigest StandingsDigest, IEnumerable<CollegeLicenceInformation> CollegeLicenceInformation) : IDomainEvent
{
    public PlrCpnLookupFound(int partyId, Guid userId, string cpn, PlrStandingsDigest standingsDigest, IEnumerable<CollegeLicenceInformation> collegeLicenceInformation) : this(partyId, new[] { userId }, cpn, standingsDigest, collegeLicenceInformation) { }
}

public record PlrCpnLookupNotFound(int PartyId) : IDomainEvent { }
