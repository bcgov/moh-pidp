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

