namespace Pidp.Infrastructure.HttpClients.Plr;

using Pidp.Extensions;

public class IdentifierType
{
    public static readonly IdentifierType Pharmacist = new("PHID");
    public static readonly IdentifierType PharmacyTech = new("PHTID");
    public static readonly IdentifierType PhysiciansAndSurgeons = new("CPSID");
    public static readonly IdentifierType Nurse = new("RNID");
    public static readonly IdentifierType Midwife = new("RMID");
    public static readonly IdentifierType NaturopathicPhysician = new("NDID");
    public static readonly IdentifierType DentalSurgeon = new("DENID");
    public static readonly IdentifierType Optometrist = new("OPTID");

    public string Value { get; }

    private IdentifierType(string value) => this.Value = value;

    public static implicit operator string(IdentifierType type) => type.Value;
}

public class PlrRecord
{
    public string Cpn { get; set; } = string.Empty;
    public string Ipc { get; set; } = string.Empty;
    public string? CollegeId { get; set; }
    public string? IdentifierType { get; set; }
    public string? ProviderRoleType { get; set; }
    public string? StatusCode { get; set; }
    public DateTime? StatusStartDate { get; set; }
    public string? StatusReasonCode { get; set; }

    public virtual bool IsGoodStanding()
    {
        var goodStatndingReasons = new[] { "GS", "PRAC", "TEMPPER" };
        return this.StatusCode == "ACTIVE"
            && goodStatndingReasons.Contains(this.StatusReasonCode);
    }
}

public class PlrStandingsDigest
{
    private readonly IEnumerable<(string? IdentifierType, bool IsGoodStanding)> records;

    public bool Error { get; private set; }

    private PlrStandingsDigest(bool error, IEnumerable<(string? IdentifierType, bool IsGoodStanding)>? records = null)
    {
        this.Error = error;
        this.records = records ?? Enumerable.Empty<(string? IdentifierType, bool IsGoodStanding)>();
    }

    /// <summary>
    /// Returns true if there is at least one record in good standing.
    /// If any Identifier Types are supplied, only checks records matching the supplied Identifier Types.
    /// </summary>
    public bool HasRecordInGoodStanding(params IdentifierType[] identifierTypes)
    {
        return this.records
            .If(identifierTypes.Any(), e => e
                .Where(record => identifierTypes.Cast<string>().Contains(record.IdentifierType))
            )
            .Any(record => record.IsGoodStanding);
    }

    public static PlrStandingsDigest FromEmpty() => new(false);
    public static PlrStandingsDigest FromError() => new(true);
    public static PlrStandingsDigest FromRecords(IEnumerable<PlrRecord> records)
    {
        return new(false, records.Select(record =>
        (
            record.IdentifierType,
            IsGoodStanding: record.IsGoodStanding()
        )));
    }
}
