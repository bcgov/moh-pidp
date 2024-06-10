namespace Pidp.Models.DomainEvents;

/// <summary>
/// The standing of one of the Party's Endorsments has changed, so some data will need to be updated (i.e. BC Provider).
/// Can happen for number for reasons:
/// 1. An existing Endorsing Party was just found in PLR with a licence in good standing.
/// 2. An existing Endorsing Party's licence status changed.
/// 3. An existing Endorsement was cancelled.
/// 4. A new Licenced Party Just Endorsed the Party.
/// </summary>
public record EndorsementStandingUpdated(int PartyId) : IDomainEvent;
