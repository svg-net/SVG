using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Svg
{
    /// <summary>
    /// Represents a list of <see cref="SvgUnit"/> used with the <see cref="SvgPolyline"/> and <see cref="SvgPolygon"/>.
    /// </summary>
    [TypeConverter(typeof(SvgPointCollectionConverter))]
    public class SvgPointCollection : List<SvgUnit>, ICloneable
    {
        public object Clone()
        {
            var points = new SvgPointCollection();
            foreach (var point in this)
                points.Add(point);
            return points;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (var i = 0; i < Count; i += 2)
            {
                if (i + 1 < Count)
                {
                    if (i > 1)
                    {
                        builder.Append(" ");
                    }
                    // we don't need unit type
                    builder.Append(this[i].Value.ToString(CultureInfo.InvariantCulture));
                    builder.Append(",");
                    builder.Append(this[i + 1].Value.ToString(CultureInfo.InvariantCulture));
                }
            }
            return builder.ToString();
        }
    }

    /// <summary>
    /// A class to convert string into <see cref="SvgPointCollection"/> instances.
    /// </summary>
    internal class SvgPointCollectionConverter : TypeConverter
    {
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
                var parser = new CoordinateParser(((string)value).Trim());
                var pointValue = 0.0f;
                var result = new SvgPointCollection();
                while (parser.TryGetFloat(out pointValue))
                {
                    result.Add(new SvgUnit(SvgUnitType.User, pointValue));
                }

                return result;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
