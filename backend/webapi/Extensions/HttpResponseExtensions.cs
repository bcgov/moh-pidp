namespace Pidp.Extensions;

public static class HttpResponseExtensions
{
    /// <summary>
    /// Adds the given key + value to the Headers if both the key and value are not null or whitespace and the Header does not already exist.
    /// </summary>
    /// <param name="response"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns>true if the header was successfully added, false otherwise.</returns>
    public static bool SafeAddHeader(this HttpResponse response, string? key, string? value)
    {
        if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        return response.Headers.TryAdd(key, value);
    }
}
