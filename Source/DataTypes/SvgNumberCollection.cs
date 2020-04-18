using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Svg
{
    /// <summary>
    /// Represents a list of <see cref="float"/>.
    /// </summary>
    [TypeConverter(typeof(SvgNumberCollectionConverter))]
    public class SvgNumberCollection : List<float>, ICloneable
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
            return string.Join(" ", this.Select(v => v.ToString(CultureInfo.InvariantCulture)).ToArray());
        }
    }

    /// <summary>
    /// A class to convert string into <see cref="SvgNumberCollection"/> instances.
    /// </summary>
    internal class SvgNumberCollectionConverter : TypeConverter
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
            if (value is string str)
            {
                var collection = new SvgNumberCollection();
                var values = str.Split(new char[] { ' ', '\t', '\n', '\r', ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var v in values)
                {
                    var result = float.Parse(v, NumberStyles.Any, CultureInfo.InvariantCulture);
                    collection.Add(result);
                }
                return collection;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
