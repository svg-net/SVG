using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg
{
    /// <summary>
    /// Represents a list of <see cref="SvgUnits"/>.
    /// </summary>
    [TypeConverter(typeof(SvgUnitCollectionConverter))]
    public class SvgUnitCollection : List<SvgUnit>
    {
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
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                string[] points = ((string)value).Trim().Split(new char[] { ',', ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                SvgUnitCollection units = new SvgUnitCollection();

                foreach (string point in points)
                {
                    units.Add((SvgUnit)_unitConverter.ConvertFrom(point.Trim()));
                }

                return units;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}