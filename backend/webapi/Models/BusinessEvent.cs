namespace Pidp.Models;

using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public int PartyId { get; set; }
    public Party? Party { get; set; }

    public static LicenceStatusRoleAssigned Create(int partyId, string roleAssigned, Instant recordedOn)
    {
        return new LicenceStatusRoleAssigned
        {
            PartyId = partyId,
            Description = $"Party was assigned the {roleAssigned} role.",
            Severity = LogLevel.Information,
            RecordedOn = recordedOn
        };
    }
}
