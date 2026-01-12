namespace DoWork.Services.BCProviderAttributeService;

public interface IBCProviderAttributeService
{
    /// <summary>
    /// Updates BCProvider Attributes in EntraID.
    /// </summary>
    public Task UpdateBCProviderAttributesAsync();
}
