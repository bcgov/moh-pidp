namespace Pidp.Models;

using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Models.Lookups;

[Table(nameof(BusinessEvent))]
public abstract class BusinessEvent : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public LogLevel Severity { get; set; }

    public Instant RecordedOn { get; set; }
}

public abstract class PartyBusinessEvent : BusinessEvent
{
    [Column(nameof(PartyId))]
    public int PartyId { get; set; }
    public Party? Party { get; set; }
}

public class PartyNotInPlr : PartyBusinessEvent
{
    public static PartyNotInPlr Create(int partyId, CollegeCode collegeCode, string licenceNumber, Instant recordedOn)
    {
        return new PartyNotInPlr
        {
            PartyId = partyId,
            Description = $"Party declared the College Licence [Collge Code: {collegeCode}, Licence Number: {licenceNumber}] but was not found in PLR.",
            Severity = LogLevel.Information,
            RecordedOn = recordedOn
        };
    }
}

public class LicenceStatusRoleAssigned : PartyBusinessEvent
{
    public static LicenceStatusRoleAssigned Create(int partyId, MohKeycloakEnrolment enrolmentAssigned, Instant recordedOn)
    {
        return new LicenceStatusRoleAssigned
        {
            PartyId = partyId,
            Description = $"Party was assigned the {enrolmentAssigned.AccessRoles.Single()} role.",
            Severity = LogLevel.Information,
            RecordedOn = recordedOn
        };
    }
}

public class LicenceStatusRoleUnassigned : PartyBusinessEvent
{
    public static LicenceStatusRoleUnassigned Create(int partyId, MohKeycloakEnrolment enrolmentAssigned, Instant recordedOn)
    {
        return new LicenceStatusRoleUnassigned
        {
            PartyId = partyId,
            Description = $"Party was unassigned the {enrolmentAssigned.AccessRoles.Single()} role.",
            Severity = LogLevel.Information,
            RecordedOn = recordedOn
        };
    }
}

public class BCProviderPasswordReset : PartyBusinessEvent
{
    public static BCProviderPasswordReset Create(int partyId, string userPrincipalName, Instant recordedOn)
    {
        return new BCProviderPasswordReset
        {
            PartyId = partyId,
            Description = $"Party with User Principal Name {userPrincipalName} reset their BCProvider password.",
            Severity = LogLevel.Information,
            RecordedOn = recordedOn
        };
    }
}

public class AccountLinkingSuccess : PartyBusinessEvent
{
    public static AccountLinkingSuccess Create(int partyId, Instant recordedOn)
    {
        return new AccountLinkingSuccess
        {
            PartyId = partyId,
            Description = $"Party successfully linked their account.",
            Severity = LogLevel.Information,
            RecordedOn = recordedOn
        };
    }
}

public class AccountLinkingFailure : PartyBusinessEvent
{
    public class LinkTicketNotFound : BusinessEvent { }
    public static LinkTicketNotFound CreateTicketNotFound(Guid userId, Guid credentialLinkToken, Instant recordedOn) => new()
    {
        Description = $"No unclaimed Credential Link Ticket found. User Id: {userId}, Credential Link Token: {credentialLinkToken}",
        Severity = LogLevel.Error,
        RecordedOn = recordedOn
    };

    public static AccountLinkingFailure CreateCredentialAlreadyLinked(int partyId, int credentialId, int ticketId, Instant recordedOn) => CreateInternal(partyId, $"Credential {credentialId} is already linked. Ticket ID {ticketId}", recordedOn);
    public static AccountLinkingFailure CreateCredentialExists(int partyId, int credentialId, int ticketId, Instant recordedOn) => CreateInternal(partyId, $"Credential {credentialId} already exists on another Party. Ticket ID {ticketId}", recordedOn);
    public static AccountLinkingFailure CreateTicketExpired(int partyId, int ticketId, Instant recordedOn) => CreateInternal(partyId, $"Ticket {ticketId} expired", recordedOn);
    public static AccountLinkingFailure CreateWrongIdentityProvider(int partyId, int ticketId, string? actualIdp, Instant recordedOn) => CreateInternal(partyId, $"New Credential's Identity Provider {actualIdp} does not match Link Ticket {ticketId} expected IDP", recordedOn);

    private static AccountLinkingFailure CreateInternal(int partyId, string failureReason, Instant recordedOn)
    {
        return new AccountLinkingFailure
        {
            PartyId = partyId,
            Description = $"Party failed to link their account. Reason: {failureReason}.",
            Severity = LogLevel.Error,
            RecordedOn = recordedOn
        };
    }
}

public class CollegeLicenceSearchError : PartyBusinessEvent
{
    public static CollegeLicenceSearchError Create(int partyId, CollegeCode? collegeCode, string? licenceNumber, Instant recordedOn)
    {
        return new CollegeLicenceSearchError
        {
            PartyId = partyId,
            Description = $"CollegeLicenceSearch Error occured while searching for the CollegeCode {collegeCode}, LicenceNumber {licenceNumber}",
            Severity = LogLevel.Error,
            RecordedOn = recordedOn
        };
    }
}
