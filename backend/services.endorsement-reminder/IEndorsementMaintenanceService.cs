namespace EndorsementReminder;

public interface IEndorsementMaintenanceService
{
    /// <summary>
    /// Marks "stalled" Endorsement Requests that are more than 30 days old as Expired.
    /// </summary>
    public Task ExpireOldEndorsementRequestsAsync();
    /// <summary>
    /// Sends email reminders to Parties with "stalled" Endorsement Requests that are 7 days old.
    /// </summary>
    public Task SendReminderEmailsAsync();
}
