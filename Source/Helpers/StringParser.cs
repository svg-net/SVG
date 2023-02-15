using System;
using System.Globalization;

namespace Svg.Helpers
{
    internal static class StringParser
    {
        private static readonly CultureInfo Format = CultureInfo.InvariantCulture;

        public static float ToFloat(ref ReadOnlySpan<char> value)
        {
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
            return float.Parse(value, NumberStyles.Float, Format);
#else
            return float.Parse(value.ToString(), NumberStyles.Float, Format);
#endif
        }

        public static float ToFloatAny(ref ReadOnlySpan<char> value)
        {
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
            return float.Parse(value, NumberStyles.Any, Format);
#else
            return float.Parse(value.ToString(), NumberStyles.Any, Format);
#endif
        }

        public static double ToDouble(ref ReadOnlySpan<char> value)
        {
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
            return double.Parse(value, NumberStyles.Any, Format);
#else
            return double.Parse(value.ToString(), NumberStyles.Any, Format);
#endif
        }

        public static int ToInt(ref ReadOnlySpan<char> value)
        {
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
            return int.Parse(value, NumberStyles.Integer, Format);
#else
            return int.Parse(value.ToString(), NumberStyles.Integer, Format);
#endif
        }
    }
}
