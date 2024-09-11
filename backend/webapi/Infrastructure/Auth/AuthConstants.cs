namespace Pidp.Infrastructure.Auth;

public static class Claims
{
    public const string Address = "address";
    public const string AssuranceLevel = "identity_assurance_level";
    public const string Birthdate = "birthdate";
    public const string Email = "email";
    public const string FamilyName = "family_name";
    public const string GivenName = "given_name";
    public const string GivenNames = "given_names";
    public const string IdentityProvider = "identity_provider";
    public const string PreferredUsername = "preferred_username";
    public const string ResourceAccess = "resource_access";
    public const string Subject = "sub";
}

public static class IdentityProviders
{
    public const string BCServicesCard = "bcsc";
    public const string BCProvider = "bcprovider_aad";
    public const string Idir = "idir";
    public const string Phsa = "phsa";
}

public static class Policies
{
    public const string BcscAuthentication = "bcsc-authentication-policy";
    public const string BCProviderAuthentication = "bc-provider-authentication-policy";
    public const string HighAssuranceIdentityProvider = "high-assurance-idp-policy";
    public const string IdirAuthentication = "idir-authentication-policy";
    public const string AnyPartyIdentityProvider = "party-idp-policy";
}

public static class Clients
{
    public const string PidpApi = "PIDP-SERVICE";
}

public static class Roles
{
    // PIdP Role Placeholders
    public const string Admin = "ADMIN";
    public const string User = "USER";

    // Service account roles for external access
    public const string FhirDistributionAccess = "fhir_distribution_access";
    public const string ViewEndorsements = "view_endorsement_data";
}
