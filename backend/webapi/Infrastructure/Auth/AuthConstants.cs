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

public static class ClaimValues
{
    public const string BCServicesCard = "bcsc";
    public const string Idir = "idir";
}

public static class Policies
{
    public const string BcscAuthentication = "bcsc-authentication-policy";
    public const string IdirAuthentication = "idir-authentication-policy";
}

public static class Resources
{
    public const string PidpApi = "PIDP-SERVICE";
    public const string SaEforms = "SAT-EFORMS";
}

public static class Roles
{
    // Placeholders
    public const string Admin = "ADMIN";
    public const string User = "USER";

    // External Systems
    public const string SaEforms = "phsa_eforms_sat";
}
