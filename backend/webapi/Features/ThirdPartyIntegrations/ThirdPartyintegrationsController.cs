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
            id = "FhirMessageTest9",
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
            url = "http://hl7.org/fhir/StructureDefinition/FhirMessageTest9",
            name = "FhirMessageTest9",
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
            description = "Base StructureDefinition for FhirMessageTest9 Resource",
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
            type = "FhirMessageTest9",
            baseDefinition = "http://hl7.org/fhir/StructureDefinition/DomainResource",
            derivation = "specialization",
            differential = new {
                element = new object[] {
                    new {
                        id = "FhirMessageTest9",
                        path = "FhirMessageTest9",
                        @short = "Resource for non-supported content",
                        definition = "FhirMessageTest9 is used for handling concepts not yet defined in FHIR, narrative-only resources that don't map to an existing resource, and custom resources not appropriate for inclusion in the FHIR specification.",
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
                        id = "FhirMessageTest9.resource_Type",
                        path = "FhirMessageTest9.resource_Type",
                        @short = "Type of the resource",
                        definition = "Defines the type of the resource",
                        min = 0,
                        max = "1",
                        type = new object[] {
                            new {
                                code = "string"
                            }
                        }
                    },
                    new {
                        id = "FhirMessageTest9.parameter",
                        path = "FhirMessageTest9.parameter",
                        @short = "parameter name",
                        definition = "Defines the name of the parameter",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/parameter"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.name",
                        path = "FhirMessageTest9.parameter.name",
                        @short = "parameter name",
                        definition = "Defines the name of the parameter",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.valueDateTime",
                        path = "FhirMessageTest9.parameter.valueDateTime",
                        @short = "value date time",
                        definition = "Defines the timestamp when the value was taken",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.valueString",
                        path = "FhirMessageTest9.parameter.valueString",
                        @short = "value string",
                        definition = "Defines the value string of the parameter",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource",
                        path = "FhirMessageTest9.parameter.resource",
                        @short = "resource",
                        definition = "Defines the value resource of the parameter",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/custom-resource"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.resource_Type",
                        path = "FhirMessageTest9.parameter.resource.resource_Type",
                        @short = "resource type",
                        definition = "Defines the type of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.id",
                        path = "FhirMessageTest9.parameter.resource.id",
                        @short = "resource id",
                        definition = "Defines the id of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.meta",
                        path = "FhirMessageTest9.parameter.resource.meta",
                        @short = "resource meta",
                        definition = "Defines the meta data of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/meta"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.meta.profile",
                        path = "FhirMessageTest9.parameter.resource.meta.profile",
                        @short = "resource meta profile",
                        definition = "Defines the resource meta profile",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.type",
                        path = "FhirMessageTest9.parameter.resource.type",
                        @short = "resource type",
                        definition = "Defines the type of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry",
                        path = "FhirMessageTest9.parameter.resource.entry",
                        @short = "resource entry",
                        definition = "Defines the entry data of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/entry"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.fullUrl",
                        path = "FhirMessageTest9.parameter.resource.entry.fullUrl",
                        @short = "resource entry fullUrl",
                        definition = "Defines the entry data fullUrl of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource",
                        path = "FhirMessageTest9.parameter.resource.entry.resource",
                        @short = "entry resource",
                        definition = "Defines the resource data of the entry",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/entry-resource"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.resource_Type",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.resource_Type",
                        @short = "resource entry resource type",
                        definition = "Defines the entry data resource type of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.id",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.id",
                        @short = "resource entry id",
                        definition = "Defines the entry data id of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.meta",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.meta",
                        @short = "resource entry meta",
                        definition = "Defines the entry data meta of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                  code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/entry-meta"
                            }
                    },
                     new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.meta.profile",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.meta.profile",
                        @short = "resource meta profile",
                        definition = "Defines the resource meta profile",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.practitioner",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.practitioner",
                        @short = "resource entry practitioner",
                        definition = "Defines the entry data practitioner of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/entry-practitioner"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.practitioner.identifier",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.practitioner.identifier",
                        @short = "resource entry practitioner identifier",
                        definition = "Defines the entry data practitioner identifier of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/entry-practitioner"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.practitioner.identifier.system",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.practitioner.identifier.system",
                        @short = "resource entry practitioner identifier system",
                        definition = "Defines the entry data practitioner identifier system of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.practitioner.identifier.value",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.practitioner.identifier.value",
                        @short = "resource entry practitioner identifier value",
                        definition = "Defines the entry data practitioner identifier value of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.code",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.code",
                        @short = "resource entry code",
                        definition = "Defines the entry data code of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/entry-code"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.code.coding",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.code.coding",
                        @short = "resource entry coding",
                        definition = "Defines the entry data coding of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/entry-coding"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.code.coding.system",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.code.coding.system",
                        @short = "resource entry coding system",
                        definition = "Defines the entry data coding system of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.code.coding.code",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.code.coding.code",
                        @short = "resource entry coding code",
                        definition = "Defines the entry data coding code of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource._extension",
                        path = "FhirMessageTest9.parameter.resource.entry.resource._extension",
                        @short = "resource entry _extension",
                        definition = "Defines the entry data _extension of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/entry-_extension"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource._extension.url",
                        path = "FhirMessageTest9.parameter.resource.entry.resource._extension.url",
                        @short = "resource entry _extension url",
                        definition = "Defines the entry data _extension url of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource._extension._extension",
                        path = "FhirMessageTest9.parameter.resource.entry.resource._extension._extension",
                        @short = "resource entry _extension",
                        definition = "Defines the entry data _extension of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/entry-resource-_extension"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource._extension._extension.url",
                        path = "FhirMessageTest9.parameter.resource.entry.resource._extension._extension.url",
                        @short = "resource entry _extension url",
                        definition = "Defines the entry data _extension url of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource._extension._extension.valueIdentifier",
                        path = "FhirMessageTest9.parameter.resource.entry.resource._extension._extension.valueIdentifier",
                        @short = "resource entry _extension valueIdentifier",
                        definition = "Defines the entry data _extension valueIdentifier of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/value-identifier"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource._extension._extension.valueIdentifier.assigner",
                        path = "FhirMessageTest9.parameter.resource.entry.resource._extension._extension.valueIdentifier.assigner",
                        @short = "resource entry _extension valueIdentifier assigner",
                        definition = "Defines the entry data _extension valueIdentifier assigner of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/value-identifier-assigner"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource._extension._extension.valueIdentifier.assigner.display",
                        path = "FhirMessageTest9.parameter.resource.entry.resource._extension._extension.valueIdentifier.assigner.display",
                        @short = "resource entry _extension valueIdentifier assigner display",
                        definition = "Defines the entry data _extension valueIdentifier assigner display of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource._extension._extension.valueCodeableConcept",
                        path = "FhirMessageTest9.parameter.resource.entry.resource._extension._extension.valueCodeableConcept",
                        @short = "resource entry _extension valueCodeableConcept assigner",
                        definition = "Defines the entry data _extension valueCodeableConcept of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/value-codeable-concept"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource._extension._extension.valueCodeableConcept.coding",
                        path = "FhirMessageTest9.parameter.resource.entry.resource._extension._extension.valueCodeableConcept.coding",
                        @short = "resource entry _extension valueCodeableConcept coding",
                        definition = "Defines the entry data _extension valueCodeableConcept coding of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/value-codeable-concept-coding"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource._extension._extension.valueCodeableConcept.coding.system",
                        path = "FhirMessageTest9.parameter.resource.entry.resource._extension._extension.valueCodeableConcept.coding.system",
                        @short = "resource entry _extension valueCodeableConcept coding system",
                        definition = "Defines the entry data _extension valueCodeableConcept coding system of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource._extension._extension.valueCodeableConcept.coding.code",
                        path = "FhirMessageTest9.parameter.resource.entry.resource._extension._extension.valueCodeableConcept.coding.code",
                        @short = "resource entry _extension valueCodeableConcept coding code",
                        definition = "Defines the entry data _extension valueCodeableConcept coding code of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource._extension._extension.valuePeriod",
                        path = "FhirMessageTest9.parameter.resource.entry.resource._extension._extension.valuePeriod",
                        @short = "resource entry _extension valuePeriod",
                        definition = "Defines the entry data _extension valuePeriod of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/value-period"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource._extension._extension.valuePeriod.start",
                        path = "FhirMessageTest9.parameter.resource.entry.resource._extension._extension.valuePeriod.start",
                        @short = "resource entry _extension valuePeriod start",
                        definition = "Defines the entry data _extension valuePeriod start of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource._extension.valueAddress",
                        path = "FhirMessageTest9.parameter.resource.entry.resource._extension.valueAddress",
                        @short = "resource  _extension valueAddress",
                        definition = "Defines the _extension valueAddress of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/value-address"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource._extension.valueAddress.country",
                        path = "FhirMessageTest9.parameter.resource.entry.resource._extension.valueAddress.country",
                        @short = "resource  _extension valueAddress country",
                        definition = "Defines the _extension valueAddress country of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource._extension.valueIdentifier",
                        path = "FhirMessageTest9.parameter.resource.entry.resource._extension.valueIdentifier",
                        @short = "resource  _extension valueIdentifier",
                        definition = "Defines the _extension valueIdentifier of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/value-identifier"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource._extension.valueIdentifier.assigner",
                        path = "FhirMessageTest9.parameter.resource.entry.resource._extension.valueIdentifier.assigner",
                        @short = "resource  _extension valueIdentifier assigner",
                        definition = "Defines the _extension valueIdentifier assigner of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/value-identifier-assigner"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource._extension.valueIdentifier.assigner.display",
                        path = "FhirMessageTest9.parameter.resource.entry.resource._extension.valueIdentifier.assigner.display",
                        @short = "resource  _extension valueIdentifier assigner display",
                        definition = "Defines the _extension valueIdentifier assigner display of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource._extension.valuePeriod",
                        path = "FhirMessageTest9.parameter.resource.entry.resource._extension.valuePeriod",
                        @short = "resource  _extension valuePeriod",
                        definition = "Defines the _extension valuePeriod of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/value-period"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource._extension.valuePeriod.start",
                        path = "FhirMessageTest9.parameter.resource.entry.resource._extension.valuePeriod.start",
                        @short = "resource  _extension valuePeriod start",
                        definition = "Defines the _extension valuePeriod start of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.identifier",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.identifier",
                        @short = "resource entry identifier",
                        definition = "Defines the entry data identifier of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/identifier"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.identifier._extension",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.identifier._extension",
                        @short = "resource entry identifier _extension",
                        definition = "Defines the entry data identifier _extension of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/identifier-_extension"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.identifier._extension.url",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.identifier._extension.url",
                        @short = "resource entry identifier _extension url",
                        definition = "Defines the entry data identifier _extension url of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.identifier._extension.valueIdentifier",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.identifier._extension.valueIdentifier",
                        @short = "resource entry identifier _extension valueIdentifier",
                        definition = "Defines the entry data identifier _extension valueIdentifier of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/identifier-_extension-valueIdentifier"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.identifier._extension.valueIdentifier.assigner",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.identifier._extension.valueIdentifier.assigner",
                        @short = "resource entry identifier _extension valueIdentifier assigner",
                        definition = "Defines the entry data identifier _extension valueIdentifier assigner of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/identifier-_extension-valueIdentifier-assigner"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.identifier._extension.valueIdentifier.assigner.display",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.identifier._extension.valueIdentifier.assigner.display",
                        @short = "resource entry identifier _extension valueIdentifier assigner display",
                        definition = "Defines the entry data identifier _extension valueIdentifier assigner display of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.identifier.system",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.identifier.system",
                        @short = "resource entry identifier _extension valueIdentifier system",
                        definition = "Defines the entry data identifier _extension valueIdentifier system",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.identifier.value",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.identifier.value",
                        @short = "resource entry identifier _extension valueIdentifier value",
                        definition = "Defines the entry data identifier _extension valueIdentifier value",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.identifier.period",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.identifier.period",
                        @short = "resource entry identifier _extension valueIdentifier period",
                        definition = "Defines the entry data identifier _extension valueIdentifier period",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/identifier-_extension-valueIdentifier-period"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.identifier.period.start",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.identifier.period.start",
                        @short = "resource entry identifier _extension valueIdentifier period start",
                        definition = "Defines the entry data identifier _extension valueIdentifier period start",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.active",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.active",
                        @short = "resource entry active status",
                        definition = "Defines the entry data active status",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.name",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.name",
                        @short = "resource entry name",
                        definition = "Defines the entry data name",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/entry-resource-name"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.name._extension",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.name._extension",
                        @short = "resource entry name extension",
                        definition = "Defines the entry data name extension",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                               profile ="http://example.org/fhir/StructureDefinition/entry-resource-name-extension"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.name._extension.url",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.name._extension.url",
                        @short = "resource entry name extension url",
                        definition = "Defines the entry data name extension url",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.name._extension.valueIdentifier",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.name._extension.valueIdentifier",
                        @short = "resource entry name extension valueIdentifier",
                        definition = "Defines the entry data name extension valueIdentifier",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/entry-resource-name-value-identifier"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.name._extension.valueIdentifier.assigner",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.name._extension.valueIdentifier.assigner",
                        @short = "resource entry name extension valueIdentifier assigner",
                        definition = "Defines the entry data name extension valueIdentifier assigner",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/entry-resource-name-value-identifier-assigner"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.name._extension.valueIdentifier.assigner.display",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.name._extension.valueIdentifier.assigner.display",
                        @short = "resource entry name extension valueIdentifier assigner display",
                        definition = "Defines the entry data name extension valueIdentifier assigner display",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.name.use",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.name.use",
                        @short = "resource entry name use",
                        definition = "Defines the entry data name use",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.name.family",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.name.family",
                        @short = "resource entry name family",
                        definition = "Defines the entry data name family",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.name.given",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.name.given",
                        @short = "resource entry name given",
                        definition = "Defines the entry data name given",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.name.prefix",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.name.prefix",
                        @short = "resource entry name prefix",
                        definition = "Defines the entry data name prefix",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.name.period",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.name.period",
                        @short = "resource entry name period",
                        definition = "Defines the entry data name period",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/entry-resource-name-period"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.name.period.start",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.name.period.start",
                        @short = "resource entry name period start",
                        definition = "Defines the entry data name period start",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.telecom",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.telecom",
                        @short = "resource entry telecom",
                        definition = "Defines the entry data telecom",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/entry-resource-telecom"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.telecom._extension",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.telecom._extension",
                        @short = "resource entry telecom extension",
                        definition = "Defines the entry data telecom extension",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/entry-resource-telecom-extension"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.telecom._extension.url",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.telecom._extension.url",
                        @short = "resource entry telecom extension url",
                        definition = "Defines the entry data telecom extension url",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.telecom._extension.valueIdentifier",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.telecom._extension.valueIdentifier",
                        @short = "resource entry telecom extension valueIdentifier",
                        definition = "Defines the entry telecom _extension valueIdentifier of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/telecom-value-identifier"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.telecom._extension.valueIdentifier.assigner",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.telecom._extension.valueIdentifier.assigner",
                        @short = "resource entry _extension valueIdentifier assigner",
                        definition = "Defines the entry telecom _extension valueIdentifier assigner of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/telecom-value-identifier-assigner"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.telecom._extension.valueIdentifier.assigner.display",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.telecom._extension.valueIdentifier.assigner.display",
                        @short = "resource entry _extension valueIdentifier assigner display",
                        definition = "Defines the entry telecom _extension valueIdentifier assigner display of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.telecom._extension.valueCodeableConcept",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.telecom._extension.valueCodeableConcept",
                        @short = "resource entry telecom _extensionvalueCodeableConcept assigner",
                        definition = "Defines the entry data telecom _extensionvalueCodeableConcept of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/telecom-value-codeable-concept"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.telecom._extension.valueCodeableConcept.coding",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.telecom._extension.valueCodeableConcept.coding",
                        @short = "resource entry telecom _extensionvalueCodeableConcept coding",
                        definition = "Defines the entry data telecom _extensionvalueCodeableConcept coding of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/telecom-value-codeable-concept-coding"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.telecom._extension.valueCodeableConcept.coding.system",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.telecom._extension.valueCodeableConcept.coding.system",
                        @short = "resource entry telecom _extensionvalueCodeableConcept coding system",
                        definition = "Defines the entry data telecom _extensionvalueCodeableConcept coding system of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.telecom._extension.valueCodeableConcept.coding.code",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.telecom._extension.valueCodeableConcept.coding.code",
                        @short = "resource entry telecom _extensionvalueCodeableConcept coding code",
                        definition = "Defines the entry data telecom _extensionvalueCodeableConcept coding code of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.telecom.system",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.telecom.system",
                        @short = "resource entry identifier  valueIdentifier system",
                        definition = "Defines the entry data identifier valueIdentifier system",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.telecom.value",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.telecom.value",
                        @short = "resource entry identifier valueIdentifier value",
                        definition = "Defines the entry data identifier valueIdentifier value",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.telecom.period",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.telecom.period",
                        @short = "resource entry identifier valueIdentifier period",
                        definition = "Defines the entry data identifier valueIdentifier period",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/telecom-identifier-valueIdentifier-period"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.telecom.period.start",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.telecom.period.start",
                        @short = "resource entry identifier valueIdentifier period start",
                        definition = "Defines the entry data identifier valueIdentifier period start",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.address",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.address",
                        @short = "resource entry address",
                        definition = "Defines the entry data address",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/resource-entry-address"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.address._extension",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.address._extension",
                        @short = "resource entry address _extension",
                        definition = "Defines the entry data address _extension",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/resource-entry-address-_extension"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.address._extension.url",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.address._extension.url",
                        @short = "resource entry address _extension url",
                        definition = "Defines the entry data address _extension url",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.address._extension.valueCodeableConcept",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.address._extension.valueCodeableConcept",
                        @short = "resource entry _extension address valueCodeableConcept",
                        definition = "Defines the entry data _extension address valueCodeableConcept",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/value-codeable-concept"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.address._extension.valueCodeableConcept.coding",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.address._extension.valueCodeableConcept.coding",
                        @short = "resource entry _extension address valueCodeableConcept coding",
                        definition = "Defines the entry data _extension address valueCodeableConcept coding of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/value-codeable-concept-coding"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.address._extension.valueCodeableConcept.coding.system",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.address._extension.valueCodeableConcept.coding.system",
                        @short = "resource entry _extension address valueCodeableConcept coding system",
                        definition = "Defines the entry data _extension address valueCodeableConcept coding system of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.address._extension.valueCodeableConcept.coding.code",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.address._extension.valueCodeableConcept.coding.code",
                        @short = "resource entry _extension address valueCodeableConcept coding code",
                        definition = "Defines the entry data _extension address valueCodeableConcept coding code of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.address._extension.valueIdentifier",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.address._extension.valueIdentifier",
                        @short = "resource entry address _extension valueIdentifier",
                        definition = "Defines the entry data _extension valueIdentifier of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/address-value-identifier"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.address._extension.valueIdentifier.assigner",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.address._extension.valueIdentifier.assigner",
                        @short = "resource entry address _extension valueIdentifier assigner",
                        definition = "Defines the entry data address  _extension valueIdentifier assigner of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/address-value-identifier-assigner"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.address._extension.valueIdentifier.assigner.display",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.address._extension.valueIdentifier.assigner.display",
                        @short = "resource entry address _extension valueIdentifier assigner display",
                        definition = "Defines the entry data address _extension valueIdentifier assigner display of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.address.type",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.address.type",
                        @short = "resource entry address type",
                        definition = "Defines the entry data address type",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.address.text",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.address.text",
                        @short = "resource entry address text",
                        definition = "Defines the entry data address text",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.address.line",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.address.line",
                        @short = "resource entry address line",
                        definition = "Defines the entry data address line",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.address.city",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.address.city",
                        @short = "resource entry address city",
                        definition = "Defines the entry data address city",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.address.state",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.address.state",
                        @short = "resource entry address state",
                        definition = "Defines the entry data address state",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.address.postalCode",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.address.postalCode",
                        @short = "resource entry address postalCode",
                        definition = "Defines the entry data address postalCode",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.address.country",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.address.country",
                        @short = "resource entry address country",
                        definition = "Defines the entry data address country",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.address.period",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.address.period",
                        @short = "resource entry address period",
                        definition = "Defines the entry data address period",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/address-period"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.address.period.start",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.address.period.start",
                        @short = "resource entry address start",
                        definition = "Defines the entry data address start",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.gender",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.gender",
                        @short = "resource entry gender",
                        definition = "Defines the entry data gender",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.birthDate",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.birthDate",
                        @short = "resource entry birthDate",
                        definition = "Defines the entry data birthDate",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/entry-birth-date"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.birthDate._extension",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.birthDate._extension",
                        @short = "resource entry birthDate extension",
                        definition = "Defines the entry data birthDate extension",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/entry-birth-date-extension"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.birthDate._extension.url",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.birthDate._extension.url",
                        @short = "resource entry birthDate extension url",
                        definition = "Defines the entry data birthDate extension url",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageTest9.parameter.resource.entry.resource.birthDate._extension.valueDateTime",
                        path = "FhirMessageTest9.parameter.resource.entry.resource.birthDate._extension.valueDateTime",
                        @short = "resource entry birthDate extension valueDateTime",
                        definition = "Defines the entry data birthDate extension valueDateTime",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },

                }
            }
        }),
        Encoding.UTF8,
        "application/json");

        Console.WriteLine("StringContent : ", jsonContent);
        // sharedClient.
        using HttpResponseMessage response = await sharedClient.PutAsync(
            "/administration/StructureDefinition/FhirMessageTest9",
        jsonContent);

        // response.EnsureSuccessStatusCode()
        //     .WriteRequestToConsole();

        // var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine(response);
        Console.WriteLine("PUT API Succeeded.");
        return this.Ok();
    }

}
