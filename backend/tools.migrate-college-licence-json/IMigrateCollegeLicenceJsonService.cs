namespace MigrateCollegeLicenceJson;

public interface IMigrateCollegeLicenceJsonService
{
    /// <summary>
    /// Finds the college licence information for all parties,
    /// constructs a JSON object and updates the party's keycloak
    /// college_licence_info attribute with the JSON object.
    /// </summary>
    /// <returns></returns>
    public Task MigrateCollegeLicenceJsonAsync();
}
