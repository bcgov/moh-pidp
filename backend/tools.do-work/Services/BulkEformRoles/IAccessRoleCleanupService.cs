namespace DoWork.Services.BulkEformRoles;

public interface IAccessRoleCleanupService
{
    /// <summary>
    /// Re-evaluates every holder of one Access Type against the live revocation policy for that card and
    /// revokes the ones who no longer qualify. Dry run unless --apply was given, in which case it acts
    /// only on the Parties named in the reviewed report passed to --from.
    /// </summary>
    Task CleanupAsync();
}
