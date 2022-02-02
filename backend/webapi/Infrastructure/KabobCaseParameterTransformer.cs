namespace Pidp.Infrastructure;

using System.Text.RegularExpressions;

public class KabobCaseParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value == null || value.ToString() == null)
        {
            return null;
        }

        return Regex.Replace(value.ToString()!, "([a-z])([A-Z])", "$1-$2", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(100))
            .ToLowerInvariant();
    }
}
