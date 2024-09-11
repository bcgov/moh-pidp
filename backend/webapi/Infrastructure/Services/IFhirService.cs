namespace Pidp.Infrastructure.Services;

using Pidp.Infrastructure.HttpClients.Fhir;

public interface IFhirService
{
    Task<FhirClient> ConstructFhirClient();
}
