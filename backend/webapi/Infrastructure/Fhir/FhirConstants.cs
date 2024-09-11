namespace Pidp.Infrastructure.Fhir;

public class FhirConstants {
    public static object modelCreatePayload = new
        {
            resourceType = "StructureDefinition",
            id = "FhirMessageBody",
            meta = new
            {
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
            url = "http://hl7.org/fhir/StructureDefinition/FhirMessageBody",
            name = "FhirMessageBody",
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
            description = "Base StructureDefinition for FhirMessageBody Resource",
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
            type = "FhirMessageBody",
            baseDefinition = "http://hl7.org/fhir/StructureDefinition/DomainResource",
            derivation = "specialization",
            differential = new
            {
                element = new object[] {
                    new {
                        id = "FhirMessageBody",
                        path = "FhirMessageBody",
                        @short = "Resource for non-supported content",
                        definition = "FhirMessageBody is used for handling concepts not yet defined in FHIR, narrative-only resources that don't map to an existing resource, and custom resources not appropriate for inclusion in the FHIR specification.",
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
                        id = "FhirMessageBody.resource_Type",
                        path = "FhirMessageBody.resource_Type",
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
                        id = "FhirMessageBody.parameter",
                        path = "FhirMessageBody.parameter",
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
                        id = "FhirMessageBody.parameter.name",
                        path = "FhirMessageBody.parameter.name",
                        @short = "parameter name",
                        definition = "Defines the name of the parameter",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.valueDateTime",
                        path = "FhirMessageBody.parameter.valueDateTime",
                        @short = "value date time",
                        definition = "Defines the timestamp when the value was taken",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.valueString",
                        path = "FhirMessageBody.parameter.valueString",
                        @short = "value string",
                        definition = "Defines the value string of the parameter",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource",
                        path = "FhirMessageBody.parameter.resource",
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
                        id = "FhirMessageBody.parameter.resource.resource_Type",
                        path = "FhirMessageBody.parameter.resource.resource_Type",
                        @short = "resource type",
                        definition = "Defines the type of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.id",
                        path = "FhirMessageBody.parameter.resource.id",
                        @short = "resource id",
                        definition = "Defines the id of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.meta",
                        path = "FhirMessageBody.parameter.resource.meta",
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
                        id = "FhirMessageBody.parameter.resource.meta.profile",
                        path = "FhirMessageBody.parameter.resource.meta.profile",
                        @short = "resource meta profile",
                        definition = "Defines the resource meta profile",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.type",
                        path = "FhirMessageBody.parameter.resource.type",
                        @short = "resource type",
                        definition = "Defines the type of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry",
                        path = "FhirMessageBody.parameter.resource.entry",
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
                        id = "FhirMessageBody.parameter.resource.entry.fullUrl",
                        path = "FhirMessageBody.parameter.resource.entry.fullUrl",
                        @short = "resource entry fullUrl",
                        definition = "Defines the entry data fullUrl of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource",
                        path = "FhirMessageBody.parameter.resource.entry.resource",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.resource_Type",
                        path = "FhirMessageBody.parameter.resource.entry.resource.resource_Type",
                        @short = "resource entry resource type",
                        definition = "Defines the entry data resource type of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.id",
                        path = "FhirMessageBody.parameter.resource.entry.resource.id",
                        @short = "resource entry id",
                        definition = "Defines the entry data id of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.meta",
                        path = "FhirMessageBody.parameter.resource.entry.resource.meta",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.meta.profile",
                        path = "FhirMessageBody.parameter.resource.entry.resource.meta.profile",
                        @short = "resource meta profile",
                        definition = "Defines the resource meta profile",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.contained",
                        path = "FhirMessageBody.parameter.resource.entry.resource.contained",
                        @short = "resource entry contained",
                        definition = "Defines the resource entry contained",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/resource-entry-contained"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.contained.resource_Type",
                        path = "FhirMessageBody.parameter.resource.entry.resource.contained.resource_Type",
                        @short = "resource entry contained resource_Type",
                        definition = "Defines the resource entry contained resource_Type",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.contained.id",
                        path = "FhirMessageBody.parameter.resource.entry.resource.contained.id",
                        @short = "resource entry contained id",
                        definition = "Defines the resource entry contained id",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.contained.meta",
                        path = "FhirMessageBody.parameter.resource.entry.resource.contained.meta",
                        @short = "resource entry contained meta",
                        definition = "Defines the resource entry contained meta",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/resource-contained-meta"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.contained.meta.profile",
                        path = "FhirMessageBody.parameter.resource.entry.resource.contained.meta.profile",
                        @short = "resource entry contained meta profile",
                        definition = "Defines the resource entry contained meta profile",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.contained.name",
                        path = "FhirMessageBody.parameter.resource.entry.resource.contained.name",
                        @short = "resource entry contained name",
                        definition = "Defines the resource entry contained name",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.contained.address",
                        path = "FhirMessageBody.parameter.resource.entry.resource.contained.address",
                        @short = "resource entry contained address",
                        definition = "Defines the resource entry contained address",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/resource-entry-contained-address"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.contained.address.city",
                        path = "FhirMessageBody.parameter.resource.entry.resource.contained.address.city",
                        @short = "resource entry contained address city",
                        definition = "Defines the resource entry contained address city",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string",
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.contained.address.state",
                        path = "FhirMessageBody.parameter.resource.entry.resource.contained.address.state",
                        @short = "resource entry contained address state",
                        definition = "Defines the resource entry contained address state",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string",
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.contained.address.country",
                        path = "FhirMessageBody.parameter.resource.entry.resource.contained.address.country",
                        @short = "resource entry contained address country",
                        definition = "Defines the resource entry contained address country",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string",
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.practitioner",
                        path = "FhirMessageBody.parameter.resource.entry.resource.practitioner",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.practitioner.identifier",
                        path = "FhirMessageBody.parameter.resource.entry.resource.practitioner.identifier",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.practitioner.identifier.system",
                        path = "FhirMessageBody.parameter.resource.entry.resource.practitioner.identifier.system",
                        @short = "resource entry practitioner identifier system",
                        definition = "Defines the entry data practitioner identifier system of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.practitioner.identifier.value",
                        path = "FhirMessageBody.parameter.resource.entry.resource.practitioner.identifier.value",
                        @short = "resource entry practitioner identifier value",
                        definition = "Defines the entry data practitioner identifier value of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.code",
                        path = "FhirMessageBody.parameter.resource.entry.resource.code",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.code.coding",
                        path = "FhirMessageBody.parameter.resource.entry.resource.code.coding",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.code.coding.system",
                        path = "FhirMessageBody.parameter.resource.entry.resource.code.coding.system",
                        @short = "resource entry coding system",
                        definition = "Defines the entry data coding system of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.code.coding.code",
                        path = "FhirMessageBody.parameter.resource.entry.resource.code.coding.code",
                        @short = "resource entry coding code",
                        definition = "Defines the entry data coding code of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.specialty",
                        path = "FhirMessageBody.parameter.resource.entry.resource.specialty",
                        @short = "resource entry specialty",
                        definition = "Defines the entry data specialty of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/resource-speciality"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.specialty._extension",
                        path = "FhirMessageBody.parameter.resource.entry.resource.specialty._extension",
                        @short = "resource entry specialty extension",
                        definition = "Defines the entry data specialty extension of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/resource-speciality-extension"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.specialty._extension.url",
                        path = "FhirMessageBody.parameter.resource.entry.resource.specialty._extension.url",
                        @short = "resource entry specialty extension url",
                        definition = "Defines the entry data specialty extension url of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.specialty._extension.valueString",
                        path = "FhirMessageBody.parameter.resource.entry.resource.specialty._extension.valueString",
                        @short = "resource entry specialty extension valueString",
                        definition = "Defines the entry data specialty extension valueString of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.specialty._extension.valuePeriod",
                        path = "FhirMessageBody.parameter.resource.entry.resource.specialty._extension.valuePeriod",
                        @short = "resource entry speciality _extension valuePeriod",
                        definition = "Defines the entry speciality _extension valuePeriod of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/speciality-value-period"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.specialty._extension.valuePeriod.start",
                        path = "FhirMessageBody.parameter.resource.entry.resource.specialty._extension.valuePeriod.start",
                        @short = "resource entry _extension specialty valuePeriod start",
                        definition = "Defines the entry data _extension specialty valuePeriod start of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.specialty._extension.valueIdentifier",
                        path = "FhirMessageBody.parameter.resource.entry.resource.specialty._extension.valueIdentifier",
                        @short = "resource entry specialty _extension valueIdentifier",
                        definition = "Defines the entry data specialty _extension valueIdentifier of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/value-identifier"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.specialty._extension.valueIdentifier.assigner",
                        path = "FhirMessageBody.parameter.resource.entry.resource.specialty._extension.valueIdentifier.assigner",
                        @short = "resource entry specialty _extension valueIdentifier assigner",
                        definition = "Defines the entry data specialty _extension valueIdentifier assigner of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/value-identifier-assigner"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.specialty._extension.valueIdentifier.assigner.display",
                        path = "FhirMessageBody.parameter.resource.entry.resource.specialty._extension.valueIdentifier.assigner.display",
                        @short = "resource entry _extension specialty valueIdentifier assigner display",
                        definition = "Defines the entry data specialty _extension valueIdentifier assigner display of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.specialty.coding",
                        path = "FhirMessageBody.parameter.resource.entry.resource.specialty.coding",
                        @short = "resource entry specialty valueCodeableConcept coding",
                        definition = "Defines the entry data specialty valueCodeableConcept coding of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/specialty-coding"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.specialty.coding.system",
                        path = "FhirMessageBody.parameter.resource.entry.resource.specialty.coding.system",
                        @short = "resource entry specialty valueCodeableConcept coding system",
                        definition = "Defines the entry data specialty valueCodeableConcept coding system of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.specialty.coding.code",
                        path = "FhirMessageBody.parameter.resource.entry.resource.specialty.coding.code",
                        @short = "resource entry specialty valueCodeableConcept coding code",
                        definition = "Defines the entry data specialty valueCodeableConcept coding code of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource._extension",
                        path = "FhirMessageBody.parameter.resource.entry.resource._extension",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource._extension.url",
                        path = "FhirMessageBody.parameter.resource.entry.resource._extension.url",
                        @short = "resource entry _extension url",
                        definition = "Defines the entry data _extension url of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource._extension._extension",
                        path = "FhirMessageBody.parameter.resource.entry.resource._extension._extension",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource._extension._extension.url",
                        path = "FhirMessageBody.parameter.resource.entry.resource._extension._extension.url",
                        @short = "resource entry _extension url",
                        definition = "Defines the entry data _extension url of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource._extension._extension.valueIdentifier",
                        path = "FhirMessageBody.parameter.resource.entry.resource._extension._extension.valueIdentifier",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource._extension._extension.valueIdentifier.assigner",
                        path = "FhirMessageBody.parameter.resource.entry.resource._extension._extension.valueIdentifier.assigner",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource._extension._extension.valueIdentifier.assigner.display",
                        path = "FhirMessageBody.parameter.resource.entry.resource._extension._extension.valueIdentifier.assigner.display",
                        @short = "resource entry _extension valueIdentifier assigner display",
                        definition = "Defines the entry data _extension valueIdentifier assigner display of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource._extension._extension.valueCodeableConcept",
                        path = "FhirMessageBody.parameter.resource.entry.resource._extension._extension.valueCodeableConcept",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource._extension._extension.valueCodeableConcept.coding",
                        path = "FhirMessageBody.parameter.resource.entry.resource._extension._extension.valueCodeableConcept.coding",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource._extension._extension.valueCodeableConcept.coding.system",
                        path = "FhirMessageBody.parameter.resource.entry.resource._extension._extension.valueCodeableConcept.coding.system",
                        @short = "resource entry _extension valueCodeableConcept coding system",
                        definition = "Defines the entry data _extension valueCodeableConcept coding system of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource._extension._extension.valueCodeableConcept.coding.code",
                        path = "FhirMessageBody.parameter.resource.entry.resource._extension._extension.valueCodeableConcept.coding.code",
                        @short = "resource entry _extension valueCodeableConcept coding code",
                        definition = "Defines the entry data _extension valueCodeableConcept coding code of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource._extension._extension.valuePeriod",
                        path = "FhirMessageBody.parameter.resource.entry.resource._extension._extension.valuePeriod",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource._extension._extension.valuePeriod.start",
                        path = "FhirMessageBody.parameter.resource.entry.resource._extension._extension.valuePeriod.start",
                        @short = "resource entry _extension valuePeriod start",
                        definition = "Defines the entry data _extension valuePeriod start of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource._extension.valueAddress",
                        path = "FhirMessageBody.parameter.resource.entry.resource._extension.valueAddress",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource._extension.valueAddress.country",
                        path = "FhirMessageBody.parameter.resource.entry.resource._extension.valueAddress.country",
                        @short = "resource  _extension valueAddress country",
                        definition = "Defines the _extension valueAddress country of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource._extension.valueIdentifier",
                        path = "FhirMessageBody.parameter.resource.entry.resource._extension.valueIdentifier",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource._extension.valueIdentifier.assigner",
                        path = "FhirMessageBody.parameter.resource.entry.resource._extension.valueIdentifier.assigner",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource._extension.valueIdentifier.assigner.display",
                        path = "FhirMessageBody.parameter.resource.entry.resource._extension.valueIdentifier.assigner.display",
                        @short = "resource  _extension valueIdentifier assigner display",
                        definition = "Defines the _extension valueIdentifier assigner display of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource._extension.valuePeriod",
                        path = "FhirMessageBody.parameter.resource.entry.resource._extension.valuePeriod",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource._extension.valuePeriod.start",
                        path = "FhirMessageBody.parameter.resource.entry.resource._extension.valuePeriod.start",
                        @short = "resource  _extension valuePeriod start",
                        definition = "Defines the _extension valuePeriod start of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.identifier",
                        path = "FhirMessageBody.parameter.resource.entry.resource.identifier",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.identifier._extension",
                        path = "FhirMessageBody.parameter.resource.entry.resource.identifier._extension",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.identifier._extension.url",
                        path = "FhirMessageBody.parameter.resource.entry.resource.identifier._extension.url",
                        @short = "resource entry identifier _extension url",
                        definition = "Defines the entry data identifier _extension url of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.identifier._extension.valueIdentifier",
                        path = "FhirMessageBody.parameter.resource.entry.resource.identifier._extension.valueIdentifier",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.identifier._extension.valueIdentifier.assigner",
                        path = "FhirMessageBody.parameter.resource.entry.resource.identifier._extension.valueIdentifier.assigner",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.identifier._extension.valueIdentifier.assigner.display",
                        path = "FhirMessageBody.parameter.resource.entry.resource.identifier._extension.valueIdentifier.assigner.display",
                        @short = "resource entry identifier _extension valueIdentifier assigner display",
                        definition = "Defines the entry data identifier _extension valueIdentifier assigner display of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.identifier.system",
                        path = "FhirMessageBody.parameter.resource.entry.resource.identifier.system",
                        @short = "resource entry identifier _extension valueIdentifier system",
                        definition = "Defines the entry data identifier _extension valueIdentifier system",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.identifier.value",
                        path = "FhirMessageBody.parameter.resource.entry.resource.identifier.value",
                        @short = "resource entry identifier _extension valueIdentifier value",
                        definition = "Defines the entry data identifier _extension valueIdentifier value",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.identifier.period",
                        path = "FhirMessageBody.parameter.resource.entry.resource.identifier.period",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.identifier.period.start",
                        path = "FhirMessageBody.parameter.resource.entry.resource.identifier.period.start",
                        @short = "resource entry identifier _extension valueIdentifier period start",
                        definition = "Defines the entry data identifier _extension valueIdentifier period start",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.active",
                        path = "FhirMessageBody.parameter.resource.entry.resource.active",
                        @short = "resource entry active status",
                        definition = "Defines the entry data active status",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.name",
                        path = "FhirMessageBody.parameter.resource.entry.resource.name",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.name._extension",
                        path = "FhirMessageBody.parameter.resource.entry.resource.name._extension",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.name._extension.url",
                        path = "FhirMessageBody.parameter.resource.entry.resource.name._extension.url",
                        @short = "resource entry name extension url",
                        definition = "Defines the entry data name extension url",
                        min = 0,
                        max = "*",
                        type = new {
                               code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.name._extension.valueIdentifier",
                        path = "FhirMessageBody.parameter.resource.entry.resource.name._extension.valueIdentifier",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.name._extension.valueIdentifier.assigner",
                        path = "FhirMessageBody.parameter.resource.entry.resource.name._extension.valueIdentifier.assigner",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.name._extension.valueIdentifier.assigner.display",
                        path = "FhirMessageBody.parameter.resource.entry.resource.name._extension.valueIdentifier.assigner.display",
                        @short = "resource entry name extension valueIdentifier assigner display",
                        definition = "Defines the entry data name extension valueIdentifier assigner display",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.name.use",
                        path = "FhirMessageBody.parameter.resource.entry.resource.name.use",
                        @short = "resource entry name use",
                        definition = "Defines the entry data name use",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.name.family",
                        path = "FhirMessageBody.parameter.resource.entry.resource.name.family",
                        @short = "resource entry name family",
                        definition = "Defines the entry data name family",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.name.given",
                        path = "FhirMessageBody.parameter.resource.entry.resource.name.given",
                        @short = "resource entry name given",
                        definition = "Defines the entry data name given",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.name.prefix",
                        path = "FhirMessageBody.parameter.resource.entry.resource.name.prefix",
                        @short = "resource entry name prefix",
                        definition = "Defines the entry data name prefix",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.name.period",
                        path = "FhirMessageBody.parameter.resource.entry.resource.name.period",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.name.period.start",
                        path = "FhirMessageBody.parameter.resource.entry.resource.name.period.start",
                        @short = "resource entry name period start",
                        definition = "Defines the entry data name period start",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.telecom",
                        path = "FhirMessageBody.parameter.resource.entry.resource.telecom",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.telecom._extension",
                        path = "FhirMessageBody.parameter.resource.entry.resource.telecom._extension",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.telecom._extension.url",
                        path = "FhirMessageBody.parameter.resource.entry.resource.telecom._extension.url",
                        @short = "resource entry telecom extension url",
                        definition = "Defines the entry data telecom extension url",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.telecom._extension.valueIdentifier",
                        path = "FhirMessageBody.parameter.resource.entry.resource.telecom._extension.valueIdentifier",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.telecom._extension.valueIdentifier.assigner",
                        path = "FhirMessageBody.parameter.resource.entry.resource.telecom._extension.valueIdentifier.assigner",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.telecom._extension.valueIdentifier.assigner.display",
                        path = "FhirMessageBody.parameter.resource.entry.resource.telecom._extension.valueIdentifier.assigner.display",
                        @short = "resource entry _extension valueIdentifier assigner display",
                        definition = "Defines the entry telecom _extension valueIdentifier assigner display of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.telecom._extension.valueCodeableConcept",
                        path = "FhirMessageBody.parameter.resource.entry.resource.telecom._extension.valueCodeableConcept",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.telecom._extension.valueCodeableConcept.coding",
                        path = "FhirMessageBody.parameter.resource.entry.resource.telecom._extension.valueCodeableConcept.coding",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.telecom._extension.valueCodeableConcept.coding.system",
                        path = "FhirMessageBody.parameter.resource.entry.resource.telecom._extension.valueCodeableConcept.coding.system",
                        @short = "resource entry telecom _extensionvalueCodeableConcept coding system",
                        definition = "Defines the entry data telecom _extensionvalueCodeableConcept coding system of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.telecom._extension.valueCodeableConcept.coding.code",
                        path = "FhirMessageBody.parameter.resource.entry.resource.telecom._extension.valueCodeableConcept.coding.code",
                        @short = "resource entry telecom _extensionvalueCodeableConcept coding code",
                        definition = "Defines the entry data telecom _extensionvalueCodeableConcept coding code of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.telecom.system",
                        path = "FhirMessageBody.parameter.resource.entry.resource.telecom.system",
                        @short = "resource entry identifier  valueIdentifier system",
                        definition = "Defines the entry data identifier valueIdentifier system",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.telecom.value",
                        path = "FhirMessageBody.parameter.resource.entry.resource.telecom.value",
                        @short = "resource entry identifier valueIdentifier value",
                        definition = "Defines the entry data identifier valueIdentifier value",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.telecom.period",
                        path = "FhirMessageBody.parameter.resource.entry.resource.telecom.period",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.telecom.period.start",
                        path = "FhirMessageBody.parameter.resource.entry.resource.telecom.period.start",
                        @short = "resource entry identifier valueIdentifier period start",
                        definition = "Defines the entry data identifier valueIdentifier period start",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.address",
                        path = "FhirMessageBody.parameter.resource.entry.resource.address",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.address._extension",
                        path = "FhirMessageBody.parameter.resource.entry.resource.address._extension",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.address._extension.url",
                        path = "FhirMessageBody.parameter.resource.entry.resource.address._extension.url",
                        @short = "resource entry address _extension url",
                        definition = "Defines the entry data address _extension url",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.address._extension.valueCodeableConcept",
                        path = "FhirMessageBody.parameter.resource.entry.resource.address._extension.valueCodeableConcept",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.address._extension.valueCodeableConcept.coding",
                        path = "FhirMessageBody.parameter.resource.entry.resource.address._extension.valueCodeableConcept.coding",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.address._extension.valueCodeableConcept.coding.system",
                        path = "FhirMessageBody.parameter.resource.entry.resource.address._extension.valueCodeableConcept.coding.system",
                        @short = "resource entry _extension address valueCodeableConcept coding system",
                        definition = "Defines the entry data _extension address valueCodeableConcept coding system of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.address._extension.valueCodeableConcept.coding.code",
                        path = "FhirMessageBody.parameter.resource.entry.resource.address._extension.valueCodeableConcept.coding.code",
                        @short = "resource entry _extension address valueCodeableConcept coding code",
                        definition = "Defines the entry data _extension address valueCodeableConcept coding code of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.address._extension.valueIdentifier",
                        path = "FhirMessageBody.parameter.resource.entry.resource.address._extension.valueIdentifier",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.address._extension.valueIdentifier.assigner",
                        path = "FhirMessageBody.parameter.resource.entry.resource.address._extension.valueIdentifier.assigner",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.address._extension.valueIdentifier.assigner.display",
                        path = "FhirMessageBody.parameter.resource.entry.resource.address._extension.valueIdentifier.assigner.display",
                        @short = "resource entry address _extension valueIdentifier assigner display",
                        definition = "Defines the entry data address _extension valueIdentifier assigner display of the resource",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.address.type",
                        path = "FhirMessageBody.parameter.resource.entry.resource.address.type",
                        @short = "resource entry address type",
                        definition = "Defines the entry data address type",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.address.text",
                        path = "FhirMessageBody.parameter.resource.entry.resource.address.text",
                        @short = "resource entry address text",
                        definition = "Defines the entry data address text",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.address.line",
                        path = "FhirMessageBody.parameter.resource.entry.resource.address.line",
                        @short = "resource entry address line",
                        definition = "Defines the entry data address line",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.address.city",
                        path = "FhirMessageBody.parameter.resource.entry.resource.address.city",
                        @short = "resource entry address city",
                        definition = "Defines the entry data address city",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.address.state",
                        path = "FhirMessageBody.parameter.resource.entry.resource.address.state",
                        @short = "resource entry address state",
                        definition = "Defines the entry data address state",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.address.postalCode",
                        path = "FhirMessageBody.parameter.resource.entry.resource.address.postalCode",
                        @short = "resource entry address postalCode",
                        definition = "Defines the entry data address postalCode",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.address.country",
                        path = "FhirMessageBody.parameter.resource.entry.resource.address.country",
                        @short = "resource entry address country",
                        definition = "Defines the entry data address country",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.address.period",
                        path = "FhirMessageBody.parameter.resource.entry.resource.address.period",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.address.period.start",
                        path = "FhirMessageBody.parameter.resource.entry.resource.address.period.start",
                        @short = "resource entry address start",
                        definition = "Defines the entry data address start",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.gender",
                        path = "FhirMessageBody.parameter.resource.entry.resource.gender",
                        @short = "resource entry gender",
                        definition = "Defines the entry data gender",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.birthDate",
                        path = "FhirMessageBody.parameter.resource.entry.resource.birthDate",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.birthDate._extension",
                        path = "FhirMessageBody.parameter.resource.entry.resource.birthDate._extension",
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
                        id = "FhirMessageBody.parameter.resource.entry.resource.birthDate._extension.url",
                        path = "FhirMessageBody.parameter.resource.entry.resource.birthDate._extension.url",
                        @short = "resource entry birthDate extension url",
                        definition = "Defines the entry data birthDate extension url",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.birthDate._extension.valueDateTime",
                        path = "FhirMessageBody.parameter.resource.entry.resource.birthDate._extension.valueDateTime",
                        @short = "resource entry birthDate extension valueDateTime",
                        definition = "Defines the entry data birthDate extension valueDateTime",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.qualification",
                        path = "FhirMessageBody.parameter.resource.entry.resource.qualification",
                        @short = "resource entry qualification",
                        definition = "Defines the entry data qualification",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/resource-entry-qualification"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.qualification._extension",
                        path = "FhirMessageBody.parameter.resource.entry.resource.qualification._extension",
                        @short = "resource entry qualification _extension",
                        definition = "Defines the entry data qualification _extension",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/resource-entry-qualification-_extension"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.qualification._extension.url",
                        path = "FhirMessageBody.parameter.resource.entry.resource.qualification._extension.url",
                        @short = "resource entry qualification _extension url",
                        definition = "Defines the entry data qualification _extension url",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string",
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.qualification._extension.valueIdentifier",
                        path = "FhirMessageBody.parameter.resource.entry.resource.qualification._extension.valueIdentifier",
                        @short = "resource entry qualification _extension valueIdentifier",
                        definition = "Defines the entry data qualification _extension valueIdentifier",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/resource-entry-qualification-_extension-valueidentifier"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.qualification._extension.valueIdentifier.assigner",
                        path = "FhirMessageBody.parameter.resource.entry.resource.qualification._extension.valueIdentifier.assigner",
                        @short = "resource entry qualification _extension valueIdentifier assigner",
                        definition = "Defines the entry data qualification _extension valueIdentifier assigner",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/resource-entry-qualification-_extension-valueidentifier-assigner"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.qualification._extension.valueIdentifier.assigner.display",
                        path = "FhirMessageBody.parameter.resource.entry.resource.qualification._extension.valueIdentifier.assigner.display",
                        @short = "resource entry qualification _extension valueIdentifier assigner display",
                        definition = "Defines the entry data qualification _extension valueIdentifier assigner display",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.qualification._extension._extension",
                        path = "FhirMessageBody.parameter.resource.entry.resource.qualification._extension._extension",
                        @short = "resource entry qualification _extension _extension",
                        definition = "Defines the entry data qualification _extension _extension",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/resource-entry-qualification-_extension-_extension"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.qualification._extension._extension.url",
                        path = "FhirMessageBody.parameter.resource.entry.resource.qualification._extension._extension.url",
                        @short = "resource entry qualification _extension _extension url",
                        definition = "Defines the entry data qualification _extension _extension url",
                        min = 0,
                        max = "*",
                        type = new {
                                 code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.qualification._extension._extension.valueString",
                        path = "FhirMessageBody.parameter.resource.entry.resource.qualification._extension._extension.valueString",
                        @short = "resource entry qualification _extension _extension valueString",
                        definition = "Defines the entry data qualification _extension _extension valueString",
                        min = 0,
                        max = "*",
                        type = new {
                                 code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.qualification._extension._extension.valueBoolean",
                        path = "FhirMessageBody.parameter.resource.entry.resource.qualification._extension._extension.valueBoolean",
                        @short = "resource entry qualification _extension _extension valueBoolean",
                        definition = "Defines the entry data qualification _extension _extension valueBoolean",
                        min = 0,
                        max = "*",
                        type = new {
                                 code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.qualification._extension._extension.valueDate",
                        path = "FhirMessageBody.parameter.resource.entry.resource.qualification._extension._extension.valueDate",
                        @short = "resource entry qualification _extension _extension valueDate",
                        definition = "Defines the entry data qualification _extension _extension valueDate",
                        min = 0,
                        max = "*",
                        type = new {
                                 code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.qualification.code",
                        path = "FhirMessageBody.parameter.resource.entry.resource.qualification.code",
                        @short = "resource entry qualification code",
                        definition = "Defines the entry data qualification code",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/resource-entry-qualification-code"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.qualification.code.coding",
                        path = "FhirMessageBody.parameter.resource.entry.resource.qualification.code.coding",
                        @short = "resource entry qualification code coding",
                        definition = "Defines the entry data qualification code coding",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/resource-entry-qualification-code-coding"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.qualification.code.coding.system",
                        path = "FhirMessageBody.parameter.resource.entry.resource.qualification.code.coding.system",
                        @short = "resource entry qualification code coding system",
                        definition = "Defines the entry data qualification code coding system",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string",
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.qualification.code.coding.code",
                        path = "FhirMessageBody.parameter.resource.entry.resource.qualification.code.coding.code",
                        @short = "resource entry qualification code coding code",
                        definition = "Defines the entry data qualification code coding code",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string",
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.qualification.period",
                        path = "FhirMessageBody.parameter.resource.entry.resource.qualification.period",
                        @short = "resource entry qualification period",
                        definition = "Defines the entry data qualification period",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/resource-entry-qualification-period"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.qualification.period.start",
                        path = "FhirMessageBody.parameter.resource.entry.resource.qualification.period.start",
                        @short = "resource entry qualification period start",
                        definition = "Defines the entry data qualification period start",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.qualification.issuer",
                        path = "FhirMessageBody.parameter.resource.entry.resource.qualification.issuer",
                        @short = "resource entry qualification issuer",
                        definition = "Defines the entry data qualification issuer",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "BackboneElement",
                                profile ="http://example.org/fhir/StructureDefinition/resource-entry-qualification-issuer"
                            }
                    },
                    new {
                        id = "FhirMessageBody.parameter.resource.entry.resource.qualification.issuer.reference",
                        path = "FhirMessageBody.parameter.resource.entry.resource.qualification.issuer.reference",
                        @short = "resource entry qualification issuer reference",
                        definition = "Defines the entry data qualification issuer reference",
                        min = 0,
                        max = "*",
                        type = new {
                                code = "string",
                            }
                    },
                }
            }
        };

    public static String modelCreateUrl = "administration/StructureDefinition/";
    public static String modelName = "FhirMessageBody";
    public static String resourceType = "resourceType";
    public static String resourceTypeReplacer = "resource_Type";
    public static String postContentType = "application/json";
    public static String modelInsertDataEndpoint = "/" + modelName;
}
