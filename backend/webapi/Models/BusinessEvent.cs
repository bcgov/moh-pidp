namespace Pidp.Models;

using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Pidp.Infrastructure.HttpClients.Keycloak;

[Table(nameof(BusinessEvent))]
public abstract class BusinessEvent : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public LogLevel Severity { get; set; }

    public Instant RecordedOn { get; set; }
}

public class PartyNotInPlr : BusinessEvent
{
    // Normally EF would map this column to "PartyNotInPlr_PartyId" but we would like all derived types to share the column where applicable.
    [Column(nameof(PartyId))]
    public int PartyId { get; set; }
    public Party? Party { get; set; }

    public static PartyNotInPlr Create(int partyId, Instant recordedOn)
    {
        return new PartyNotInPlr
        {
            PartyId = partyId,
            Description = "Party declared a College Licence but was not found in PLR.",
            Severity = LogLevel.Information,
            RecordedOn = recordedOn
        };
    }
}

public class LicenceStatusRoleAssigned : BusinessEvent
{
    [Column(nameof(PartyId))]
    public int PartyId { get; set; }
    public Party? Party { get; set; }

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

public class LicenceStatusRoleUnassigned : BusinessEvent
{
    [Column(nameof(PartyId))]
    public int PartyId { get; set; }
    public Party? Party { get; set; }

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

