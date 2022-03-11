namespace Pidp.Models.ProfileStatus;

public interface IProfileSection
{
    string Name { get; }
    HashSet<Alert> Alerts { get; }
    StatusCode StatusCode { get; }

    bool IsComplete => this.StatusCode == StatusCode.Complete;
}
