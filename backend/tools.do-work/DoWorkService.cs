namespace DoWork;

/// <summary>
/// Modify this file with custom scripts / helper services.
/// Remember to check that any dependencies you need (like the Keycloak or BC Provider client) are registered in the Program.cs file and the nessisary environment variables have been added or modified in appsettings.json.
/// </summary>
public class DoWorkService() : IDoWorkService
{
    public async Task DoWorkAsync()
    {
        var placeholder = await Task.FromResult("This is a placeholder for custom work.");
    }
}
