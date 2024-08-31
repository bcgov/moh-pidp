namespace Pidp.Features.ThirdPartyIntegrations;

using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using DomainResults.Common;
using DomainResults.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Fhir;
using Pidp.Infrastructure.Services;
using Pidp.Models;
using Serilog;

[Route("api/ext")]
public class ThirdPartyintegrationsController : PidpControllerBase
{
    private readonly PidpDbContext context;
    private static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("http://localhost:8080"),
    };

    public ThirdPartyintegrationsController(IPidpAuthorizationService authorizationService, PidpDbContext context) : base(authorizationService) {
        this.context = context;
    }

    [HttpGet("parties/{hpdid}/endorsements")]
    [Authorize(Roles = Roles.ViewEndorsements)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<EndorsementData.Model>>> GetEndorsemetData([FromServices] IQueryHandler<EndorsementData.Query, IDomainResult<List<EndorsementData.Model>>> handler,
                                                                                   [FromRoute] EndorsementData.Query query)
        => await handler.HandleAsync(query)
            .ToActionResultOfT();

    [HttpPost("fhir/message/save")]
    [AllowAnonymous]
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

    [HttpPost("fhir/model/create")]
    [AllowAnonymous]
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

    [HttpPost("fhir/model/save_data")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> PostSaveData([FromBody] object obj) {
        var jsonStr = JsonSerializer.Serialize(obj);
        var newJsonObj = jsonStr.Replace(FhirConstants.resourceType, FhirConstants.resourceTypeReplacer);
        var payload = JsonObject.Parse(newJsonObj);
        payload[FhirConstants.resourceType] = FhirConstants.modelName;
        StringContent stringContent = new StringContent(payload.ToString(), UnicodeEncoding.UTF8,  FhirConstants.postContentType);
        using HttpResponseMessage response = await sharedClient.PostAsync(
            FhirConstants.modelInsertDataEndpoint, stringContent
        );

        Log.Information(response.ToString());
        return this.Ok();
    }

}
