namespace Pidp.Extensions;

using Pidp.Data;
using Pidp.Models;

public static class PidpDbContextExtensions
{
    /// <summary>
    /// Builds the IQueryable for all of a given Party's Active Endorsement Relationships.
    /// </summary>
    public static IQueryable<EndorsementRelationship> ActiveEndorsementRelationships(this PidpDbContext context, int partyId)
    {
        return context.Endorsements
            .Where(endorsement => endorsement.Active
                && endorsement.EndorsementRelationships.Any(relationship => relationship.PartyId == partyId))
            .SelectMany(endorsement => endorsement.EndorsementRelationships)
            .Where(relationship => relationship.PartyId != partyId);
    }

    /// <summary>
    /// Builds the IQueryable for all Parties in an active Endorsement Relationship with the given Party.
    /// Removes duplicates if the Party is endorsed by the same Party multiple times.
    /// </summary>
    public static IQueryable<Party> ActiveEndorsingParties(this PidpDbContext context, int partyId)
    {
        return context.ActiveEndorsementRelationships(partyId)
            .Select(relationship => relationship.Party!)
            .Distinct();
    }
}
