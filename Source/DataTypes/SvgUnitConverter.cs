using System;
using System.ComponentModel;
using System.Globalization;

namespace Svg
{
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

            return (unit.Substring(identifierIndex).Trim().ToLowerInvariant()) switch
            {
                "mm" => new SvgUnit(SvgUnitType.Millimeter, val),
                "cm" => new SvgUnit(SvgUnitType.Centimeter, val),
                "in" => new SvgUnit(SvgUnitType.Inch, val),
                "px" => new SvgUnit(SvgUnitType.Pixel, val),
                "pt" => new SvgUnit(SvgUnitType.Point, val),
                "pc" => new SvgUnit(SvgUnitType.Pica, val),
                "%" => new SvgUnit(SvgUnitType.Percentage, val),
                "em" => new SvgUnit(SvgUnitType.Em, val),
                "ex" => new SvgUnit(SvgUnitType.Ex, val),
                _ => throw new FormatException("Unit is in an invalid format '" + unit + "'."),
            };
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
}
