namespace Pidp.Infrastructure.HttpClients.Plr;

using Pidp.Models.Lookups;

public interface IPlrClient
{
    Task<PlrRecord> GetPlrRecord(string licenceNumber, CollegeCode collegeCode);
}
