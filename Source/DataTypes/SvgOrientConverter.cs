using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Svg
{
    public sealed class SvgOrientConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value == null || value.ToString() == string.Empty || value.ToString() == "auto")
            {
                return new SvgOrient();
            }
            else if (value is float)
            {
                return new SvgOrient((float)value);
            }
            else if (value is int)
            {
                return new SvgOrient((float)value);
            }

            throw new ArgumentException("The value '" + value.ToString() + "' cannot be converted to an SVG value.");
        }
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string) || sourceType == typeof(float) || sourceType == typeof(int))
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
                return ((SvgOrient)value).ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
