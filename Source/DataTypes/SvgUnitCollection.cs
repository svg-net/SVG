using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Svg
{
    /// <summary>
    /// Represents a list of <see cref="SvgUnits"/>.
    /// </summary>
    [TypeConverter(typeof(SvgUnitCollectionConverter))]
    public class SvgUnitCollection : List<SvgUnit>
    {
        public override string ToString()
        {
            string ret = "";
            foreach (var unit in this)
            {
                ret += unit.ToString() + " ";
            }

            return ret;
        }
    }

    /// <summary>
    /// A class to convert string into <see cref="SvgUnitCollection"/> instances.
    /// </summary>
    internal class SvgUnitCollectionConverter : TypeConverter
    {
        private static readonly SvgUnitConverter _unitConverter = new SvgUnitConverter();
        /// <summary>
        /// Converts the given object to the type of this converter, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo"/> to use as the current culture.</param>
        /// <param name="value">The <see cref="T:System.Object"/> to convert.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                string[] points = ((string)value).Trim().Split(new char[] { ',', ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                SvgUnitCollection units = new SvgUnitCollection();

                foreach (string point in points)
                {
                    SvgUnit newUnit = (SvgUnit)_unitConverter.ConvertFrom(point.Trim());
                    if (!newUnit.IsNone)
                        units.Add(newUnit);
                }

                return units;
            }

            return base.ConvertFrom(context, culture, value);
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
                return value.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}