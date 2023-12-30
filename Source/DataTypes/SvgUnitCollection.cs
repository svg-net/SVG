using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Svg.Helpers;

namespace Svg
{
    /// <summary>
    /// Represents a list of <see cref="SvgUnit"/>.
    /// </summary>
    [TypeConverter(typeof(SvgUnitCollectionConverter))]
    public class SvgUnitCollection : ObservableCollection<SvgUnit>, ICloneable
    {
        public const string None = "none";

        public const string Inherit = "inherit";

        /// <summary>
        /// Sets <see cref="None"/> or <see cref="Inherit"/> if needed.
        /// </summary>
        public string StringForEmptyValue { get; set; }

        public void AddRange(IEnumerable<SvgUnit> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (collection == this)
            {
                // handle special case where the collection is duplicated
                // we need to clone it to avoid an exception during enumeration
                var clonedCollection = new SvgUnitCollection();
                foreach (var unit in collection)
                    clonedCollection.Add(unit);
                collection = clonedCollection;
            }

            foreach (var unit in collection)
                Add(unit);
        }

        public override string ToString()
        {
            if (Count <= 0 && !string.IsNullOrEmpty(StringForEmptyValue))
                return StringForEmptyValue;

            // The correct separator should be a single white space.
            // More see:
            // http://www.w3.org/TR/SVG/coords.html
            // "Superfluous white space and separators such as commas can be eliminated
            // (e.g., 'M 100 100 L 200 200' contains unnecessary spaces and could be expressed more compactly as 'M100 100L200 200')."
            // http://www.w3.org/TR/SVGTiny12/paths.html#PathDataGeneralInformation
            // https://developer.mozilla.org/en-US/docs/Web/SVG/Attribute/d#Notes
            return string.Join(" ", this.Select(u => u.ToString()).ToArray());
        }

        public static bool IsNullOrEmpty(SvgUnitCollection collection)
        {
            return collection == null || collection.Count < 1 ||
                (collection.Count == 1 && (collection[0] == SvgUnit.Empty || collection[0] == SvgUnit.None));
        }

        public object Clone()
        {
            var units = new SvgUnitCollection
            {
                StringForEmptyValue = StringForEmptyValue
            };
            units.AddRange(this);
            return units;
        }
    }

    /// <summary>
    /// A class to convert string into <see cref="SvgUnitCollection"/> instances.
    /// </summary>
    public class SvgUnitCollectionConverter : TypeConverter
    {
        private static readonly char[] SplitChars = new[] { ',', ' ', '\r', '\n', '\t' };

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
            return value is not string str ? base.ConvertFrom(context, culture, value) : Parse(str.AsSpan());
        }

        public static SvgUnitCollection Parse(ReadOnlySpan<char> points)
        {
            var units = new SvgUnitCollection();
            var splitChars = SplitChars.AsSpan();
            var parts = new StringSplitEnumerator(points, splitChars);

            foreach (var part in parts)
            {
                var newUnit = SvgUnitConverter.Parse(part.Value);
                if (!newUnit.IsNone)
                {
                    units.Add(newUnit);
                }
            }

            return units;
        }
    }

    internal class SvgStrokeDashArrayConverter : SvgUnitCollectionConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string s1)
            {
                var span = s1.AsSpan();
                var s = span.Trim();

                if (MemoryExtensions.Equals(s, SvgUnitCollection.None.AsSpan(), StringComparison.OrdinalIgnoreCase))
                {
                    return new SvgUnitCollection
                    {
                        StringForEmptyValue = SvgUnitCollection.None
                    };
                }
                else if (MemoryExtensions.Equals(s, SvgUnitCollection.Inherit.AsSpan(), StringComparison.OrdinalIgnoreCase))
                {
                    return new SvgUnitCollection
                    {
                        StringForEmptyValue = SvgUnitCollection.Inherit
                    };
                }

                return SvgUnitCollectionConverter.Parse(span);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
