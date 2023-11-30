namespace EndorsementReminder;

/// <summary>
/// Sends email reminders to Parties with "stalled" Endorsement Requests that are 7 days old.
/// </summary>
public interface IEndorsementReminderService
{
    public Task DoWorkAsync();
}
