namespace Pidp.Features.FhirMessages;

using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using DomainResults.Common;
using DomainResults.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Fhir;
using Pidp.Infrastructure.Services;
using Pidp.Models;
using Serilog;

[Route("api/fhir")]
public class FhirMessagesController : PidpControllerBase
{
    private readonly PidpDbContext context;
    private static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("http://localhost:8080"),
    };

    public FhirMessagesController(IPidpAuthorizationService authorizationService, PidpDbContext context) : base(authorizationService) {
        this.context = context;
    }

    [HttpPost("message")]
    [Authorize(Roles = Roles.FhirDistributionAccess)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> PostFHIRMessage([FromBody] JsonDocument obj)
    {
        var fhirmessage = new FhirMessage
        {
            MessageBody = obj,
        };
        this.context.FhirMessages.Add(fhirmessage);
        await this.context.SaveChangesAsync();
        return this.Ok();
    }

    [HttpPost("model/create")]
    [Authorize(Roles = Roles.FhirDistributionAccess)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> PostModel([FromBody] object obj)
    {
        using StringContent jsonContent = FhirConstants.modelCreatePayload;
        using HttpResponseMessage response = await sharedClient.PutAsync(
            FhirConstants.modelCreateUrl + FhirConstants.modelName,
        jsonContent);

        Log.Information(response.ToString());
        return this.Ok();
    }

    [HttpPost("model")]
    [Authorize(Roles = Roles.FhirDistributionAccess)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PostSaveData([FromServices] ICommandHandler<FhirMessages.Command, IDomainResult> handler, [FromBody] FhirMessages.Command command)
        => await handler.HandleAsync(command).ToActionResult();

}
