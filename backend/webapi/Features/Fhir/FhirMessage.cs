namespace Pidp.Features.FhirMessages;

using DomainResults.Mvc;
using DomainResults.Common;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Serilog;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Fhir;
using Pidp.Models;


public class FhirMessages
{
    private readonly PidpDbContext context;
    private static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("http://firely-server:4080"),
    };

    public class Command : ICommand<IDomainResult>
    {
        public string resourceType { get; set; }
        public object parameter { get; set; }
    }


    public class CommandHandler(PidpDbContext context) :  ICommandHandler<Command, IDomainResult>
    {
        private readonly PidpDbContext context = context;

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var jsonStr = JsonSerializer.Serialize(command);
            var newJsonObj = jsonStr.Replace(FhirConstants.resourceType, FhirConstants.resourceTypeReplacer);
            var payload = JsonObject.Parse(newJsonObj);
            payload[FhirConstants.resourceType] = FhirConstants.modelName;
            StringContent stringContent = new StringContent(payload.ToString(), UnicodeEncoding.UTF8,  FhirConstants.postContentType);

            using HttpResponseMessage response = await sharedClient.PostAsync(
                FhirConstants.modelInsertDataEndpoint, stringContent
            );

            Log.Information(response.ToString());

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
