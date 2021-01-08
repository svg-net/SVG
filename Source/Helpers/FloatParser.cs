using System;
using System.Globalization;

namespace Svg.Helpers
{
    internal static class FloatParser
    {
        private static readonly CultureInfo Format = CultureInfo.InvariantCulture;

        public static float ToFloat(ref ReadOnlySpan<char> value)
        {
#if NETSTANDARD2_1 || NETCORE || NETCOREAPP2_2 || NETCOREAPP3_0
            return float.Parse(value, NumberStyles.Float, Format);
#else
            return float.Parse(value.ToString(), NumberStyles.Float, Format);
#endif
        }

        public static float ToFloatAny(ref ReadOnlySpan<char> value)
        {
#if NETSTANDARD2_1 || NETCORE || NETCOREAPP2_2 || NETCOREAPP3_0
            return float.Parse(value, NumberStyles.Any, Format);
#else
            return float.Parse(value.ToString(), NumberStyles.Any, Format);
#endif
        }
    }
}

