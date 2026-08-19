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

public class AccessRequestRevoked : PartyBusinessEvent
{
    public static AccessRequestRevoked Create(int partyId, AccessTypeCode accessTypeCode, MohKeycloakEnrolment? enrolment, string? cpn, IEnumerable<Guid> userIds, string reason, string? trigger, Instant recordedOn)
    {
        return new AccessRequestRevoked
        {
            PartyId = partyId,
            Description = $"Party's {accessTypeCode} Access Request was deleted and {FormatRoles(enrolment)} unassigned."
                + $" Reason: {reason}."
                + $" Trigger: {trigger ?? "not recorded"}."
                + $" CPN: {cpn ?? "none (endorsement-based access)"}."
                + $" Keycloak User Ids: {FormatUserIds(userIds)}.",
            Severity = LogLevel.Information,
            RecordedOn = recordedOn
        };
    }

    public static AccessRequestRevoked CreateFailure(int partyId, AccessTypeCode accessTypeCode, MohKeycloakEnrolment? enrolment, string? cpn, IEnumerable<Guid> userIds, string reason, string? trigger, Instant recordedOn)
    {
        return new AccessRequestRevoked
        {
            PartyId = partyId,
            Description = $"Party was no longer entitled to their {accessTypeCode} Access Request but {FormatRoles(enrolment)} could not be unassigned; the Access Request was left in place to be retried."
                + $" Reason: {reason}."
                + $" Trigger: {trigger ?? "not recorded"}."
                + $" CPN: {cpn ?? "none (endorsement-based access)"}."
                + $" Keycloak User Ids: {FormatUserIds(userIds)}.",
            Severity = LogLevel.Error,
            RecordedOn = recordedOn
        };
    }

    private static string FormatRoles(MohKeycloakEnrolment? enrolment) => enrolment == null
        ? "no associated Keycloak role was"
        : $"the {string.Join(", ", enrolment.AccessRoles)} role(s) in Client {enrolment.ClientId} were";

    private static string FormatUserIds(IEnumerable<Guid> userIds) => userIds.Any()
        ? string.Join(", ", userIds)
        : "none";
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

public class DemographicsUpdated : PartyBusinessEvent
{
    public static DemographicsUpdated Create(int partyId, Instant recordedOn)
    {
        return new DemographicsUpdated
        {
            PartyId = partyId,
            Description = "Party's demographics were updated.",
            Severity = LogLevel.Information,
            RecordedOn = recordedOn
        };
    }
}

public class AccessRequestSubmitted : PartyBusinessEvent
{
    public static AccessRequestSubmitted Create(int partyId, string accessType, Instant recordedOn)
    {
        return new AccessRequestSubmitted
        {
            PartyId = partyId,
            Description = $"Party submitted an access request for {accessType}.",
            Severity = LogLevel.Information,
            RecordedOn = recordedOn
        };
    }
}

public class AccessRequestFailed : PartyBusinessEvent
{
    public static AccessRequestFailed Create(int partyId, string accessType, Instant recordedOn)
    {
        return new AccessRequestFailed
        {
            PartyId = partyId,
            Description = $"Party's access request for {accessType} failed due to unmet prerequisites.",
            Severity = LogLevel.Warning,
            RecordedOn = recordedOn
        };
    }
}

public class EndorsementRequestCreated : PartyBusinessEvent
{
    public static EndorsementRequestCreated Create(int partyId, string recipientEmail, Instant recordedOn)
    {
        return new EndorsementRequestCreated
        {
            PartyId = partyId,
            Description = $"Party sent an endorsement request to {recipientEmail}.",
            Severity = LogLevel.Information,
            RecordedOn = recordedOn
        };
    }
}

public class EndorsementApproved : PartyBusinessEvent
{
    public static EndorsementApproved Create(int partyId, int endorsementRequestId, Instant recordedOn)
    {
        return new EndorsementApproved
        {
            PartyId = partyId,
            Description = $"Party approved endorsement request ID {endorsementRequestId}.",
            Severity = LogLevel.Information,
            RecordedOn = recordedOn
        };
    }
}

public class EndorsementDenied : PartyBusinessEvent
{
    public static EndorsementDenied Create(int partyId, int endorsementRequestId, Instant recordedOn)
    {
        return new EndorsementDenied
        {
            PartyId = partyId,
            Description = $"Party declined endorsement request ID {endorsementRequestId}.",
            Severity = LogLevel.Information,
            RecordedOn = recordedOn
        };
    }
}

public class EndorsementCancelled : PartyBusinessEvent
{
    public static EndorsementCancelled Create(int partyId, int endorsementId, Instant recordedOn)
    {
        return new EndorsementCancelled
        {
            PartyId = partyId,
            Description = $"Party cancelled active endorsement ID {endorsementId}.",
            Severity = LogLevel.Information,
            RecordedOn = recordedOn
        };
    }
}

public class PharmacyAdded : PartyBusinessEvent
{
    public static PharmacyAdded Create(int partyId, string pharmacyName, Instant recordedOn)
    {
        return new PharmacyAdded
        {
            PartyId = partyId,
            Description = $"Party added a new pharmacy: {pharmacyName}.",
            Severity = LogLevel.Information,
            RecordedOn = recordedOn
        };
    }
}

public class PharmacyUpdated : PartyBusinessEvent
{
    public static PharmacyUpdated Create(int partyId, string pharmacyName, Instant recordedOn)
    {
        return new PharmacyUpdated
        {
            PartyId = partyId,
            Description = $"Party updated details for pharmacy: {pharmacyName}.",
            Severity = LogLevel.Information,
            RecordedOn = recordedOn
        };
    }
}

public class PharmacyStaffChanged : PartyBusinessEvent
{
    public static PharmacyStaffChanged Create(int partyId, string pharmacyName, Instant recordedOn)
    {
        return new PharmacyStaffChanged
        {
            PartyId = partyId,
            Description = $"Party modified the staffing at pharmacy: {pharmacyName}.",
            Severity = LogLevel.Information,
            RecordedOn = recordedOn
        };
    }
}

public class PlrStatusUpdated : PartyBusinessEvent
{
    public static PlrStatusUpdated Create(int partyId, string providerRoleType, Instant recordedOn)
    {
        return new PlrStatusUpdated
        {
            PartyId = partyId,
            Description = $"Party's PLR standing was updated for Role Type: {providerRoleType}.",
            Severity = LogLevel.Information,
            RecordedOn = recordedOn
        };
    }
}

public class BCProviderAttributesUpdated : PartyBusinessEvent
{
    public static BCProviderAttributesUpdated Create(int partyId, string details, Instant recordedOn)
    {
        return new BCProviderAttributesUpdated
        {
            PartyId = partyId,
            Description = $"BCProvider attributes updated: {details}.",
            Severity = LogLevel.Information,
            RecordedOn = recordedOn
        };
    }
}
