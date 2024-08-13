namespace Pidp.Features.ThirdPartyIntegrations;

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using DomainResults.Common;
using DomainResults.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Extensibility;
using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;
using Pidp.Models;

[Route("api/ext")]
public class ThirdPartyintegrationsController : PidpControllerBase
{
    private readonly PidpDbContext context;
    private static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("https://firely.server:4080/"),
    };

    static async Task PutAsync(HttpClient httpClient, object obj)
    {
        using StringContent jsonContent = new(
        JsonSerializer.Serialize(new
        {
            obj
        }),
        Encoding.UTF8,
        "application/fhir+json");

    using HttpResponseMessage response = await httpClient.PutAsync(
        "/administration/StructureDefinition/Test2",
        jsonContent);

    // response.EnsureSuccessStatusCode()
    //     .WriteRequestToConsole();

    var jsonResponse = await response.Content.ReadAsStringAsync();
    Console.WriteLine($"{jsonResponse}\n");

    }
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
        // HttpRequestHeaders httprequestheaders = new HttpRequestHeaders().Add("","");
        using StringContent jsonContent = new(
        JsonSerializer.Serialize(new
        {
            obj
        }),
        Encoding.UTF8,
        "application/json");

        Console.WriteLine("StringContent : ", jsonContent);
        // sharedClient.
        using HttpResponseMessage response = await sharedClient.PutAsync(
            "/administration/StructureDefinition/Test2",
        jsonContent);

        // response.EnsureSuccessStatusCode()
        //     .WriteRequestToConsole();

        // var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine(response);
        Console.WriteLine("PUT API Succeeded.");
        return this.Ok();
    }

}
