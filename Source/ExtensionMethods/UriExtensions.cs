using System;

namespace Svg.ExtensionMethods
{
    public static class UriExtensions
    {
        public static Uri ReplaceWithNullIfNone(this Uri uri)
        {
            return string.Equals(uri?.ToString().Trim(), "none", StringComparison.OrdinalIgnoreCase) ? null : uri;
        }
    }
}
