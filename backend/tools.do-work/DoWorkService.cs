namespace DoWork;

using DoWork.Services.BCProviderAttributeService;

/// <summary>
/// Modify this file with custom scripts / helper services.
/// Remember to check that any dependencies you need (like the Keycloak or BC Provider client) are registered in the Program.cs file and the nessisary environment variables have been added or modified in appsettings.json.
/// </summary>
public class DoWorkService(IBCProviderAttributeService service) : IDoWorkService
{
    private readonly IBCProviderAttributeService bcProviderAttributeService = service;
    public async Task DoWorkAsync() => await this.bcProviderAttributeService.UpdateBCProviderAttributesAsync();
}
