namespace Pidp.Infrastructure.HttpClients.Plr;

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

public class ProviderRoleType
{
    public static readonly ProviderRoleType MedicalDoctor = new("MD");
    public static readonly ProviderRoleType RegisteredNursePractitioner = new("RNP");

    public string Value { get; }

    private ProviderRoleType(string value) => this.Value = value;

    public static implicit operator string(ProviderRoleType type) => type.Value;
}

public class PlrRecord
{
    public string Cpn { get; set; } = string.Empty;
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
    private readonly IEnumerable<DigestRecord> records;

    public bool Error { get; private set; }

    /// <summary>
    /// Returns true if there is at least one record in good standing.
    /// </summary>
    public bool HasGoodStanding => this.records.Any(record => record.IsGoodStanding);

    public IEnumerable<string> LicenceNumbers => this.records.Where(record => record.LicenceNumber != null).Select(record => record.LicenceNumber!);

    public IEnumerable<string> Cpns => this.records.Select(record => record.Cpn);

    private PlrStandingsDigest(bool error, IEnumerable<DigestRecord>? records = null)
    {
        this.Error = error;
        this.records = records ?? Enumerable.Empty<DigestRecord>();
    }

    /// <summary>
    /// Filters the digest to exclude records of the given Identifier Type(s)
    /// </summary>
    /// <param name="identifierTypes"></param>
    public PlrStandingsDigest Excluding(params IdentifierType[] identifierTypes)
    {
        return new PlrStandingsDigest
        (
            this.Error,
            this.records.ExceptBy(identifierTypes.Select(t => (string)t), record => record.IdentifierType)
        );
    }

    /// <summary>
    /// Filters the digest to only include records of the given Identifier Type(s)
    /// </summary>
    /// <param name="identifierTypes"></param>
    public PlrStandingsDigest With(params IdentifierType[] identifierTypes)
    {
        return new PlrStandingsDigest
        (
            this.Error,
            this.records.IntersectBy(identifierTypes.Select(t => (string)t), record => record.IdentifierType)
        );
    }

    /// <summary>
    /// Filters the digest to only include records of the given Provider Role Type(s)
    /// </summary>
    /// <param name="providerRoleTypes"></param>
    public PlrStandingsDigest With(params ProviderRoleType[] providerRoleTypes)
    {
        return new PlrStandingsDigest
        (
            this.Error,
            this.records.IntersectBy(providerRoleTypes.Select(t => (string)t), record => record.ProviderRoleType)
        );
    }

    /// <summary>
    /// Filters the digest to only include records in good standing.
    /// </summary>
    public PlrStandingsDigest WithGoodStanding()
    {
        return new PlrStandingsDigest
        (
            this.Error,
            this.records.Where(record => record.IsGoodStanding)
        );
    }

    public static PlrStandingsDigest FromEmpty() => new(false);
    public static PlrStandingsDigest FromError() => new(true);
    public static PlrStandingsDigest FromRecords(IEnumerable<PlrRecord> records)
    {
        return new(false, records.Select(record => new DigestRecord
        {
            Cpn = record.Cpn,
            IdentifierType = record.IdentifierType,
            LicenceNumber = record.CollegeId,
            ProviderRoleType = record.ProviderRoleType,
            IsGoodStanding = record.IsGoodStanding()
        }));
    }

    private class DigestRecord
    {
        public string Cpn { get; set; } = string.Empty;
        public string? IdentifierType { get; set; }
        public string? LicenceNumber { get; set; }
        public string? ProviderRoleType { get; set; }

        public bool IsGoodStanding { get; set; }
    }
}

public class PlrStatusChangeLog
{
    public int Id { get; set; }
    public string Cpn { get; set; } = string.Empty;
    public bool IsGoodStanding { get; set; }
    public string? ProviderRoleType { get; set; }
}
