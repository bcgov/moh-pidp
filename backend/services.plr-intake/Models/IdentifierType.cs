namespace PlrIntake.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(IdentifierType))]
public class IdentifierType
{
    /// <summary>
    /// Identifier OID, e.g. 2.16.840.1.113883.3.40.2.20
    /// </summary>
    [Key]
    public string Oid { get; set; } = string.Empty;

    /// <summary>
    /// Concept referenced by OID, e.g. RNPID aka "British Columbia Registered Nurse Practitioner ID"
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
