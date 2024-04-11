namespace Pidp.Infrastructure.HttpClients.Keycloak;

using System.Text.Json;

using Pidp.Infrastructure.HttpClients.Ldap;
using Pidp.Models.Lookups;

public class MohKeycloakEnrolment
{
    private static readonly List<MohKeycloakEnrolment> All = new();
    public static readonly MohKeycloakEnrolment DriverFitness = new("DMFT-WEBAPP", AccessTypeCode.DriverFitness, "DMFT_ENROLLED");
    public static readonly MohKeycloakEnrolment EdrdEforms = new("SAT-EFORMS", AccessTypeCode.EdrdEforms, "phsa_eforms_edrd");
    public static readonly MohKeycloakEnrolment ImmsBCEforms = new("SAT-EFORMS", AccessTypeCode.ImmsBCEforms, "phsa_eforms_imms");
    public static readonly MohKeycloakEnrolment PrescriptionRefillEforms = new("SAT-EFORMS", AccessTypeCode.PrescriptionRefillEforms, "phsa_eforms_rxrefill");
    public static readonly MohKeycloakEnrolment ProviderReportingPortal = new("PRP-SERVICE", AccessTypeCode.ProviderReportingPortal, "MSPQI", "PMP");
    public static readonly MohKeycloakEnrolment SAEforms = new("SAT-EFORMS", AccessTypeCode.SAEforms, "phsa_eforms_sat");

    public static readonly MohKeycloakEnrolment MoaLicenceStatus = new("LICENCE-STATUS", "MOA");
    public static readonly MohKeycloakEnrolment PractitionerLicenceStatus = new("LICENCE-STATUS", "PRACTITIONER");

    public IEnumerable<string> AccessRoles { get; private set; }
    public AccessTypeCode? AssocatedAccessRequest { get; private set; }
    public string ClientId { get; private set; }


    private MohKeycloakEnrolment(string clientId, params string[] accessRoles) : this(clientId, null, accessRoles) { }

    private MohKeycloakEnrolment(string clientId, AccessTypeCode? associatedAccessRequest, params string[] accessRoles)
    {
        this.AccessRoles = accessRoles;
        this.AssocatedAccessRequest = associatedAccessRequest;
        this.ClientId = clientId;

        All.Add(this);
    }

    public static MohKeycloakEnrolment? FromAssociatedAccessRequest(AccessTypeCode associatedEnrolment) => All.SingleOrDefault(enrolment => enrolment.AssocatedAccessRequest == associatedEnrolment);
}

/// <summary>
/// This is not the entire Keycloak Client Representation! See https://www.keycloak.org/docs-api/22.0.1/rest-api/index.html#ClientRepresentation.
/// </summary>
public class Client
{
    /// <summary>
    /// ID referenced in URIs and tokens
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// Guid-like unique identifier
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Display name
    /// </summary>
    public string? Name { get; set; }
}

public class Role
{
    public bool? ClientRole { get; set; }
    public bool? Composite { get; set; }
    public string? ContainerId { get; set; }
    public string? Description { get; set; }
    public string? Id { get; set; }
    public string? Name { get; set; }
}

/// <summary>
/// This is not the entire Keycloak User Representation! See https://www.keycloak.org/docs-api/22.0.1/rest-api/index.html#UserRepresentation.
/// </summary>
public class UserRepresentation
{
    public Dictionary<string, string[]> Attributes { get; set; } = new();
    public string? Email { get; set; }
    public bool? Enabled { get; set; }
    public string? FirstName { get; set; }
    public string? Id { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }

    internal void SetLdapOrgDetails(LdapLoginResponse.OrgDetails orgDetails) => this.SetAttribute("org_details", JsonSerializer.Serialize(orgDetails, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));

    public void SetCpn(string cpn) => this.SetAttribute("common_provider_number", cpn);

    public void SetPidpEmail(string pidpEmail) => this.SetAttribute("pidp_email", pidpEmail);

    public void SetOpId(string opId) => this.SetAttribute("opId", opId);

    /// <summary>
    /// Adds the given attributes to this User Representation. Overwrites any duplicate keys.
    /// </summary>
    public void SetAttributes(Dictionary<string, string[]> newAttributes)
    {
        foreach (var attribute in newAttributes)
        {
            this.Attributes[attribute.Key] = attribute.Value;
        }
    }

    private void SetAttribute(string key, string value) => this.Attributes[key] = new string[] { value };
}
