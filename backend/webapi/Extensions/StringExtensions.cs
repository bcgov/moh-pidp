namespace Pidp.Extensions;

public static class StringExtensions
{
    public static string EnsureTrailingSlash(this string url)
    {
        if (!url.EndsWith('/'))
        {
            return url + "/";
        }

        return url;
    }
}
