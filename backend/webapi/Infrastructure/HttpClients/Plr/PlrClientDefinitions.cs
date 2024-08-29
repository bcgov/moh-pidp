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

public class StatusCode
{
    public static readonly StatusCode Active = new("ACTIVE");
    public static readonly StatusCode Terminated = new("TERMINATED");
    public static readonly StatusCode Suspended = new("SUSPENDED");
    public static readonly StatusCode Pending = new("PENDING");

    public string Value { get; }

    private StatusCode(string value) => this.Value = value;

    public static implicit operator string(StatusCode type) => type.Value;
}

public class StatusReasonCode
{
    public static readonly StatusReasonCode GoodStanding = new("GS");
    public static readonly StatusReasonCode Practicing = new("PRAC");
    public static readonly StatusReasonCode NonPracticing = new("NONPRAC");
    public static readonly StatusReasonCode TemporaryInactive = new("TI");
    public static readonly StatusReasonCode VoluntaryWithdrawn = new("VW");
    public static readonly StatusReasonCode Tempper = new("TEMPPER");

    public string Value { get; }

    private StatusReasonCode(string value) => this.Value = value;

    public static implicit operator string(StatusReasonCode type) => type.Value;
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
    public string? MspId { get; set; }

    public virtual bool IsGoodStanding()
    {
        // A Licence is in good standing if the Status is "ACTIVE" and the StatusReason is one of a few allowable values.
        // Additionally, "TI" (Temporary Inactive) and "VW" (Voluntary Withdrawn) are "SUSPENDED" in PLR rather than "ACTIVE", but are still considered to be in good standing.
        var goodStandingReasons = new[] { "GS", "PRAC", "TEMPPER", "TI", "VW" };
        var suspendedAllowed = new[] { "TI", "VW" };

        if (this.StatusCode == "ACTIVE")
        {
            return goodStandingReasons.Contains(this.StatusReasonCode);
        }
        else if (this.StatusCode == "SUSPENDED")
        {
            return suspendedAllowed.Contains(this.StatusReasonCode);
        }
        return false;
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

    /// <summary>
    /// Returns true if there is at least one record for a post graduate
    /// licenced individual in pending non-practicing status.
    /// </summary>
    public bool IsPostGrad => this.records.Any(
        record => record.IdentifierType == IdentifierType.PhysiciansAndSurgeons
        && record.StatusCode == StatusCode.Pending
        && record.StatusReasonCode == StatusReasonCode.NonPracticing);

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
    /// Filters the digest to only include records of the given Status Code
    /// </summary>
    /// <param name="statusCode"></param>
    public PlrStandingsDigest With(params StatusCode[] statusCodes)
    {
        return new PlrStandingsDigest
        (
            this.Error,
            this.records.IntersectBy(statusCodes.Select(t => (string)t), record => record.StatusCode)
        );
    }

    /// <summary>
    /// Filters the digest to only include records of the given Status Reason Code
    /// </summary>
    /// <param name="statusReasonCode"></param>
    public PlrStandingsDigest With(params StatusReasonCode[] statusReasonCodes)
    {
        return new PlrStandingsDigest
        (
            this.Error,
            this.records.IntersectBy(statusReasonCodes.Select(t => (string)t), record => record.StatusReasonCode)
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
            StatusCode = record.StatusCode,
            StatusReasonCode = record.StatusReasonCode,
            IsGoodStanding = record.IsGoodStanding()
        }));
    }

    private class DigestRecord
    {
        public string Cpn { get; set; } = string.Empty;
        public string? IdentifierType { get; set; }
        public string? LicenceNumber { get; set; }
        public string? ProviderRoleType { get; set; }
        public string? StatusCode { get; set; }
        public string? StatusReasonCode { get; set; }

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
