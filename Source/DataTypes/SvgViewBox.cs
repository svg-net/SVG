﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace Svg
{
    /// <summary>
    /// It is often desirable to specify that a given set of graphics stretch to fit a particular container element. The viewBox attribute provides this capability.
    /// </summary>
    [TypeConverter(typeof(SvgViewBoxConverter))]
    public partial struct SvgViewBox
    {
        public static readonly SvgViewBox Empty = new SvgViewBox();

        /// <summary>
        /// Gets or sets the position where the viewport starts horizontally.
        /// </summary>
        public float MinX { get; set; }

        /// <summary>
        /// Gets or sets the position where the viewport starts vertically.
        /// </summary>
        public float MinY { get; set; }

        /// <summary>
        /// Gets or sets the width of the viewport.
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the viewport.
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SvgViewBox"/> to <see cref="RectangleF"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator RectangleF(SvgViewBox value)
        {
            return new RectangleF(value.MinX, value.MinY, value.Width, value.Height);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="RectangleF"/> to <see cref="SvgViewBox"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SvgViewBox(RectangleF value)
        {
            return new SvgViewBox(value.X, value.Y, value.Width, value.Height);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgViewBox"/> struct.
        /// </summary>
        /// <param name="minX">The min X.</param>
        /// <param name="minY">The min Y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public SvgViewBox(float minX, float minY, float width, float height)
        {
            MinX = minX;
            MinY = minY;
            Width = width;
            Height = height;
        }

        #region Equals and GetHashCode implementation
        public override bool Equals(object obj)
        {
            return (obj is SvgViewBox) && Equals((SvgViewBox)obj);
        }

        public bool Equals(SvgViewBox other)
        {
            return MinX == other.MinX
                && MinY == other.MinY
                && Width == other.Width
                && Height == other.Height;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            unchecked
            {
                hashCode += 1000000007 * MinX.GetHashCode();
                hashCode += 1000000009 * MinY.GetHashCode();
                hashCode += 1000000021 * Width.GetHashCode();
                hashCode += 1000000033 * Height.GetHashCode();
            }
            return hashCode;
        }

        public static bool operator ==(SvgViewBox lhs, SvgViewBox rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(SvgViewBox lhs, SvgViewBox rhs)
        {
            return !(lhs == rhs);
        }
        #endregion
    }

    internal class SvgViewBoxConverter : TypeConverter
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
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                var coords = ((string)value).Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (coords.Length != 4)
                    throw new SvgException("The 'viewBox' attribute must be in the format 'minX, minY, width, height'.");

                return new SvgViewBox(float.Parse(coords[0], NumberStyles.Float, CultureInfo.InvariantCulture),
                    float.Parse(coords[1], NumberStyles.Float, CultureInfo.InvariantCulture),
                    float.Parse(coords[2], NumberStyles.Float, CultureInfo.InvariantCulture),
                    float.Parse(coords[3], NumberStyles.Float, CultureInfo.InvariantCulture));
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                var viewBox = (SvgViewBox)value;

                return string.Format("{0}, {1}, {2}, {3}",
                    viewBox.MinX.ToString(CultureInfo.InvariantCulture), viewBox.MinY.ToSvgString(),
                    viewBox.Width.ToString(CultureInfo.InvariantCulture), viewBox.Height.ToSvgString());
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
