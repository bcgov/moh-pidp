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
        // HttpRequestHeaders httprequestheaders = new HttpRequestHeaders().Add("","");
        using StringContent jsonContent = new(
        JsonSerializer.Serialize(new
        {
            resourceType = "StructureDefinition",
            id = "FhirMessageTest3",
            meta = new {
                lastUpdated = "2019-10-24T11:53:00+11:00"
            },
            extension = new object[] {
             new {
                url = "http://hl7.org/fhir/StructureDefinition/structuredefinition-fmm",
                valueInteger = 1
                },
                new {
                url = "http://hl7.org/fhir/StructureDefinition/strucfturedefinition-wg",
                valueCode = "fhir"
                }
            },
            url = "http://hl7.org/fhir/StructureDefinition/FhirMessageTest3",
            name = "FhirMessageTest3",
            status = "draft",
            date = "2019-10-24T11:53:00+11:00",
            publisher = "Health Level Seven International (FHIR Infrastructure)",
            contact = new object[] {
                new {
                    telecom = new object[] {
                        new {
                            system = "url",
                            value = "http://hl7.org/fhir"
                        }
                    }
                },
                new {
                    telecom = new object [] {
                        new {
                            system = "url",
                            value = "http://www.hl7.org/Special/committees/fiwg/index.cfm"
                        }
                    }
                }
            },
            description = "Base StructureDefinition for FhirMessageTest3 Resource",
            purpose = "Need some way to safely (without breaking interoperability) allow implementers to exchange content not supported by the initial set of declared resources.",
            mapping = new object[] {
                new {
                    identity = "rim",
                    uri = "http://hl7.org/v3",
                    name = "RIM Mapping"
                },
                new {
                    identity = "w5",
                    uri ="http://hl7.org/fhir/w5",
                    name = "W5 Mapping"
                }
            },
            kind = "resource",
            @abstract = false,
            type = "FhirMessageTest3",
            baseDefinition = "http://hl7.org/fhir/StructureDefinition/DomainResource",
            derivation = "specialization",
            differential = new {
                element = new object[] {
                    new {
                        id = "FhirMessageTest3",
                        path = "FhirMessageTest3",
                        @short = "Resource for non-supported content",
                        definition = "FhirMessageTest3 is used for handling concepts not yet defined in FHIR, narrative-only resources that don't map to an existing resource, and custom resources not appropriate for inclusion in the FHIR specification.",
                        alias = new object[] {
                            "Z-resource",
                            "Extension-resource",
                            "Custom-resource"
                        },
                        min = 0,
                        max = "*",
                        mapping = new [] {
                            new {
                                identity = "rim",
                                map = "Act, Entity or Role"
                            },
                            new {
                                identity = "w5",
                                map = "infrastructure.structure"
                            }
                        }
                    },
                    new {
                        id = "FhirMessageTest3.resourceType",
                        path = "FhirMessageTest3.resourceType",
                        @short = "Type of the resource",
                        definition = "Defines the type of the resource",
                        min = 0,
                        max = "1",
                        type = new object[] {
                            new {
                                code = "string"
                            }
                        }
                    }
                }
            }
        }),
        Encoding.UTF8,
        "application/json");

        Console.WriteLine("StringContent : ", jsonContent);
        // sharedClient.
        using HttpResponseMessage response = await sharedClient.PutAsync(
            "/administration/StructureDefinition/FhirMessageTest3",
        jsonContent);

        // response.EnsureSuccessStatusCode()
        //     .WriteRequestToConsole();

        // var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine(response);
        Console.WriteLine("PUT API Succeeded.");
        return this.Ok();
    }

}
