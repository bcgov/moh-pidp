namespace UpdateOpId;

public interface IUpdateOpIdService
{
    /// <summary>
    /// Generates an identifier for each Party in the database
    /// with a BCSC account that does not have an OpId
    /// and updates the party in the database.
    /// </summary>
    /// <returns></returns>
    public Task UpdateOpIdAsync();
}
