using System;
using System.ComponentModel;
using System.Globalization;
using Svg.Helpers;

namespace Svg
{
    public sealed class SvgUnitConverter : TypeConverter
    {
        public static SvgUnit Parse(ReadOnlySpan<char> unit)
        {
            // http://www.w3.org/TR/CSS21/syndata.html#values
            // http://www.w3.org/TR/SVG11/coords.html#Units

            int identifierIndex = -1;

            if (unit.SequenceEqual("none".AsSpan()))
            {
                return SvgUnit.None;
            }

            // Note: these are ad-hoc values based on a factor of about 1.2 between adjacent values
            // see https://www.w3.org/TR/CSS2/fonts.html#value-def-absolute-size for more information

            if (unit.SequenceEqual("medium".AsSpan()))
            {
                // unit = "1em";
                return new SvgUnit(SvgUnitType.Em, 1f);
            }
            if (unit.SequenceEqual("small".AsSpan()))
            {
                // unit = "0.8em";
                return new SvgUnit(SvgUnitType.Em, 0.8f);
            }
            if (unit.SequenceEqual("x-small".AsSpan()))
            {
                // unit = "0.7em";
                return new SvgUnit(SvgUnitType.Em, 0.7f);
            }
            if (unit.SequenceEqual("xx-small".AsSpan()))
            {
                // unit = "0.6em";
                return new SvgUnit(SvgUnitType.Em, 0.6f);
            }
            if (unit.SequenceEqual("large".AsSpan()))
            {
                // unit = "1.2em";
                return new SvgUnit(SvgUnitType.Em, 1.2f);
            }
            if (unit.SequenceEqual("x-large".AsSpan()))
            {
                // unit = "1.4em";
                return new SvgUnit(SvgUnitType.Em, 1.4f);
            }
            if (unit.SequenceEqual("xx-large".AsSpan()))
            {
                // unit = "1.7em";
                return new SvgUnit(SvgUnitType.Em, 1.7f);
            }

            var spanLength = unit.Length;

            for (var i = 0; i < spanLength; i++)
            {
                var currentChar = unit[i];

                // If the character is a percent sign or a letter which is not an exponent 'e'

                if (currentChar == '%')
                {
                    identifierIndex = i;
                    break;
                }

                if (char.IsLetter(currentChar) && !((currentChar == 'e' || currentChar == 'E') && i < spanLength - 1 &&
                                                    !char.IsLetter(unit[i + 1])))
                {
                    identifierIndex = i;
                    break;
                }
            }

            var valSpan = identifierIndex > -1 ? unit.Slice(0, identifierIndex) : unit;
            var val = StringParser.ToFloat(ref valSpan);
            if (identifierIndex == -1)
            {
                return new SvgUnit(val);
            }

            Span<char> typeSpan = stackalloc char[2];
            var toLowerLength = unit.Slice(identifierIndex).Trim().ToLowerInvariant(typeSpan);
            if (toLowerLength <= 0 || toLowerLength > 2)
            {
                throw new FormatException("Unit is in an invalid format '" + unit.ToString() + "'.");
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

            throw new FormatException("Unit is in an invalid format '" + unit.ToString() + "'.");
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

            return Parse(unit.AsSpan());
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
}
