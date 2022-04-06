namespace Pidp.Extensions;

public static class HttpResponseExtensions
{
    /// <summary>
    /// Adds the given key + value to the Headers if both the key and value are not null or whitespace.
    /// </summary>
    public static void SafeAddHeader(this HttpResponse response, string? key, string? value)
    {
        if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        response.Headers.Add(key, value);
    }
}
