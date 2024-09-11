namespace Pidp.Infrastructure.Services;

using Pidp.Infrastructure.HttpClients.Fhir;

public class FhirService : IFhirService
{
    public FhirService()
    {
    }

    public async Task<FhirClient> ConstructFhirClient()
    {
        var client = new HttpClient();
        var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<FhirClient>();
        var fhirClient = new FhirClient(client, logger);
        return fhirClient;
    }
}
