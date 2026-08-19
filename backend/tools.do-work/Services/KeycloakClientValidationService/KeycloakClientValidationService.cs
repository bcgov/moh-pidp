namespace DoWork.Services.KeycloakClientValidationService;

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pidp.Infrastructure.HttpClients.Keycloak;

public class KeycloakClientValidationService(
    IKeycloakAdministrationClient keycloakClient,
    ILogger<KeycloakClientValidationService> logger) : IKeycloakClientValidationService
{
    private readonly IKeycloakAdministrationClient keycloakClient = keycloakClient;
    private readonly ILogger<KeycloakClientValidationService> logger = logger;

    public async Task ValidateClientsAsync()
    {
        Console.WriteLine("Enter a comma-separated list of Client IDs to validate against Keycloak (default: SAT-EFORMS):");
        var input = Console.ReadLine();
        
        var clientsToValidate = string.IsNullOrWhiteSpace(input) 
            ? ["SAT-EFORMS"]
            : input.Split(',').Select(c => c.Trim()).ToArray();

        Console.WriteLine($"\nValidating {clientsToValidate.Length} client(s)...");

        foreach (var clientId in clientsToValidate)
        {
            var client = await this.keycloakClient.GetClient(clientId);

            if (client == null)
            {
                Console.WriteLine($"[MISSING] Client '{clientId}' DOES NOT exist in Keycloak.");
            }
            else
            {
                Console.WriteLine($"[FOUND]   Client '{clientId}' exists in Keycloak. (Internal ID: {client.Id})");
            }
        }
    }
}
