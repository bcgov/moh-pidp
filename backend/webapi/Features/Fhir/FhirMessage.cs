namespace Pidp.Features.FhirMessages;

using DomainResults.Mvc;
using DomainResults.Common;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Serilog;

using Pidp;
using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Fhir;
using Pidp.Models;
using Pidp.Infrastructure.HttpClients.Fhir;
using Pidp.Infrastructure.Services;

public class FhirMessages
{


    public class Command : ICommand<IDomainResult>
    {
        public string resourceType { get; set; }
        public object parameter { get; set; }
    }


    public class CommandHandler(PidpDbContext context, PidpConfiguration config) : ICommandHandler<Command, IDomainResult>
    {
        private readonly PidpDbContext context = context;
        private readonly PidpConfiguration config = config;

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var jsonStr = JsonSerializer.Serialize(command);
            var newJsonObj = jsonStr.Replace(FhirConstants.resourceType, FhirConstants.resourceTypeReplacer);
            var payload = JsonObject.Parse(newJsonObj);
            payload[FhirConstants.resourceType] = FhirConstants.modelName;

            var fhirClient = await new FhirService().ConstructFhirClient();
            var url = config.FhirService.HostAddress + FhirConstants.modelName;
            var response = await fhirClient.PostAsync(payload, url);

            Log.Information("Fhir POST API response : ");
            Log.Information(response.IsSuccess.ToString());

            // Save fhir messages to postgres database for data persistency.
            var jsonDocument = JsonDocument.Parse(jsonStr);
            var fhirMessage = new FhirMessage
            {
                MessageBody = jsonDocument,
            };
            this.context.FhirMessages.Add(fhirMessage);
            await this.context.SaveChangesAsync();
            return DomainResult.Success();
        }
    }

}
