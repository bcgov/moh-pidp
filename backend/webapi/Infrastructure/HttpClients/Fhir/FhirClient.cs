namespace Pidp.Infrastructure.HttpClients.Fhir;

using DomainResults.Common;
using Pidp.Infrastructure.Fhir;
using Serilog;

public class FhirClient : BaseClient, IFhirClient
{

    public FhirClient(HttpClient httpClient, ILogger<FhirClient> logger) : base(httpClient, logger) {
    }

    public async Task<IDomainResult> PostAsync(object payload, string url) {
        var response = await this.PostAsync(url, payload);
        return response;
    }
    public async Task<IDomainResult> PutAsync(object payload, string url) {
        string endpoint = url + FhirConstants.modelCreateUrl + FhirConstants.modelName;
        Log.Information(endpoint);
        var response = await this.PutAsync(endpoint, payload);
        return response;
    }

}
