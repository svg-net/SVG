using System;
using System.ComponentModel;
using System.Globalization;

namespace Svg
{
#if false
    public sealed class SvgUnitConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value == null)
            {
                return new SvgUnit(SvgUnitType.User, 0.0f);
            }

            if (!(value is string))
            {
                throw new ArgumentOutOfRangeException("value must be a string.");
            }

            // http://www.w3.org/TR/CSS21/syndata.html#values
            // http://www.w3.org/TR/SVG11/coords.html#Units

            string unit = (string)value;
            int identifierIndex = -1;

            if (unit == "none")
                return SvgUnit.None;

            // Note: these are ad-hoc values based on a factor of about 1.2 between adjacent values
            // see https://www.w3.org/TR/CSS2/fonts.html#value-def-absolute-size for more information
            if (unit == "medium")
                unit = "1em";
            else if (unit == "small")
                unit = "0.8em";
            else if (unit == "x-small")
                unit = "0.7em";
            else if (unit == "xx-small")
                unit = "0.6em";
            else if (unit == "large")
                unit = "1.2em";
            else if (unit == "x-large")
                unit = "1.4em";
            else if (unit == "xx-large")
                unit = "1.7em";

            for (int i = 0; i < unit.Length; i++)
            {
                // If the character is a percent sign or a letter which is not an exponent 'e'
                if (unit[i] == '%' || (char.IsLetter(unit[i]) && !((unit[i] == 'e' || unit[i] == 'E') && i < unit.Length - 1 && !char.IsLetter(unit[i + 1]))))
                {
                    identifierIndex = i;
                    break;
                }
            }

            float val = 0.0f;
            float.TryParse((identifierIndex > -1) ? unit.Substring(0, identifierIndex) : unit, NumberStyles.Float, CultureInfo.InvariantCulture, out val);

            if (identifierIndex == -1)
            {
                return new SvgUnit(val);
            }

            switch (unit.Substring(identifierIndex).Trim().ToLower())
            {
                case "mm":
                    return new SvgUnit(SvgUnitType.Millimeter, val);
                case "cm":
                    return new SvgUnit(SvgUnitType.Centimeter, val);
                case "in":
                    return new SvgUnit(SvgUnitType.Inch, val);
                case "px":
                    return new SvgUnit(SvgUnitType.Pixel, val);
                case "pt":
                    return new SvgUnit(SvgUnitType.Point, val);
                case "pc":
                    return new SvgUnit(SvgUnitType.Pica, val);
                case "%":
                    return new SvgUnit(SvgUnitType.Percentage, val);
                case "em":
                    return new SvgUnit(SvgUnitType.Em, val);
                case "ex":
                    return new SvgUnit(SvgUnitType.Ex, val);
                default:
                    throw new FormatException("Unit is in an invalid format '" + unit + "'.");
            }
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return ((SvgUnit)value).ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
#else
    public sealed class SvgUnitConverter : TypeConverter
    {
        private static float ToFloat(ref ReadOnlySpan<char> value)
        {
#if NETSTANDARD2_1 || NETCORE || NETCOREAPP2_2 || NETCOREAPP3_0
            return float.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture);
#else
            return float.Parse(value.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture);
#endif
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
            {
                return new SvgUnit(SvgUnitType.User, 0.0f);
            }

            if (!(value is string unit))
            {
                throw new ArgumentException("The value argument must be a string.");
            }

            // http://www.w3.org/TR/CSS21/syndata.html#values
            // http://www.w3.org/TR/SVG11/coords.html#Units

            int identifierIndex = -1;
            if (unit == "none")
            {
                return SvgUnit.None;
            }

            // Note: these are ad-hoc values based on a factor of about 1.2 between adjacent values
            // see https://www.w3.org/TR/CSS2/fonts.html#value-def-absolute-size for more information
            if (unit == "medium")
            {
                // unit = "1em";
                return new SvgUnit(SvgUnitType.Em, 1f);
            }
            if (unit == "small")
            {
                // unit = "0.8em";
                return new SvgUnit(SvgUnitType.Em, 0.8f);
            }
            if (unit == "x-small")
            {
                // unit = "0.7em";
                return new SvgUnit(SvgUnitType.Em, 0.7f);
            }
            if (unit == "xx-small")
            {
                // unit = "0.6em";
                return new SvgUnit(SvgUnitType.Em, 0.6f);
            }
            if (unit == "large")
            {
                // unit = "1.2em";
                return new SvgUnit(SvgUnitType.Em, 1.2f);
            }
            if (unit == "x-large")
            {
                // unit = "1.4em";
                return new SvgUnit(SvgUnitType.Em, 1.4f);
            }
            if (unit == "xx-large")
            {
                // unit = "1.7em";
                return new SvgUnit(SvgUnitType.Em, 1.7f);
            }

            var span = unit.AsSpan();
            var spanLength = span.Length;

            for (var i = 0; i < spanLength; i++)
            {
                var currentChar = span[i];

                // If the character is a percent sign or a letter which is not an exponent 'e'

                if (currentChar == '%')
                {
                    identifierIndex = i;
                    break;
                }

                if (char.IsLetter(currentChar) && !((currentChar == 'e' || currentChar == 'E') && i < spanLength - 1 && !char.IsLetter(span[i + 1])))
                {
                    identifierIndex = i;
                    break;
                }
            }

            var valSpan = identifierIndex > -1 ? span.Slice(0, identifierIndex) : span;
            var val = ToFloat(ref valSpan);
            if (identifierIndex == -1)
            {
                return new SvgUnit(val);
            }

            Span<char> typeSpan = stackalloc char[2];
            var toLowerLength = span.Slice(identifierIndex).Trim().ToLowerInvariant(typeSpan);
            if (toLowerLength <= 0 || toLowerLength > 2)
            {
                throw new FormatException("Unit is in an invalid format '" + span.ToString() + "'.");
            }

            if (toLowerLength == 1)
            {
                if (typeSpan[0] == '%')
                {
                    return new SvgUnit(SvgUnitType.Percentage, val);
                }
            }
            else
            {
                var char0 = typeSpan[0];
                var char1 = typeSpan[1];

                // mm
                if (char0 == 'm' && char1 == 'm')
                {
                    return new SvgUnit(SvgUnitType.Millimeter, val);
                }
                // cm
                if (char0 == 'c' && char1 == 'm')
                {
                    return new SvgUnit(SvgUnitType.Centimeter, val);
                }
                // in
                if (char0 == 'i' && char1 == 'n')
                {
                    return new SvgUnit(SvgUnitType.Inch, val);
                }
                // px
                if (char0 == 'p' && char1 == 'x')
                {
                    return new SvgUnit(SvgUnitType.Pixel, val);
                }
                // pt
                if (char0 == 'p' && char1 == 't')
                {
                    return new SvgUnit(SvgUnitType.Point, val);
                }
                // pc
                if (char0 == 'p' && char1 == 'c')
                {
                    return new SvgUnit(SvgUnitType.Pica, val);
                }
                // em
                if (char0 == 'e' && char1 == 'm')
                {
                    return new SvgUnit(SvgUnitType.Em, val);
                }
                // ex
                if (char0 == 'e' && char1 == 'x')
                {
                    return new SvgUnit(SvgUnitType.Ex, val);
                }
            }

            throw new FormatException("Unit is in an invalid format '" + span.ToString() + "'.");
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return destinationType == typeof(string) ?
                ((SvgUnit)value).ToString() :
                base.ConvertTo(context, culture, value, destinationType);
        }
    }
#endif
}
