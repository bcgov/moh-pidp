namespace Pidp.Infrastructure.HttpClients.Fhir;

using DomainResults.Common;

public interface IFhirClient
{
    Task<IDomainResult> PostAsync(object payload, string url);
    Task<IDomainResult> PutAsync(object payload, string url);
}
