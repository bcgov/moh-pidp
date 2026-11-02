namespace DoWork.Services.BulkEformRoles;

public interface IInfantRsvBulkGrantService
{
    /// <summary>
    /// One-time bulk grant of the Infant RSV eForm role to existing Immunization Entry / Special Authority
    /// eForm users who meet the Infant RSV eligibility criteria. Safe to re-run: Parties who already hold
    /// the enrolment are filtered out before any work is done.
    /// </summary>
    Task GrantAsync();
}
