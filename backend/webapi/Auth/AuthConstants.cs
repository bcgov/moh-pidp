namespace Pidp.Auth;

public static class AuthConstants
{
    public const string Audience = "PIDP-SERVICE";
    public const string BCServicesCard = "bcsc";
}

public static class Claims
{
    public const string PreferredUsername = "preferred_username";
    public const string GivenName = "given_name";
    public const string GivenNames = "given_names";
    public const string FamilyName = "family_name";
    public const string Address = "address";
    public const string Birthdate = "birthdate";
    public const string Email = "email";
    public const string Subject = "sub";

    public const string ResourceAccess = "resource_access";
    public const string AssuranceLevel = "identity_assurance_level";
    public const string IdentityProvider = "identity_provider";
}

public static class Roles
{
    // Placeholders
    public const string Admin = "ADMIN";
    public const string User = "USER";
}
