namespace Pidp.Infrastructure.Services;

using Pidp.Infrastructure.Fhir;
using Serilog;

public class FhirService
{
    private static PidpConfiguration config = new PidpConfiguration();

    private static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri(config.FhirService.HostAddress),
    };

    public static async void createModel()
    {
        using StringContent jsonContent = FhirConstants.modelCreatePayload;
        using HttpResponseMessage response = await sharedClient.PutAsync(
            FhirConstants.modelCreateUrl + FhirConstants.modelName,
        jsonContent);

        Log.Information(response.ToString());
    }
}
