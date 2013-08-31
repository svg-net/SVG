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
    public struct SvgViewBox
    {
        public static readonly SvgViewBox Empty = new SvgViewBox();

        /// <summary>
        /// Gets or sets the position where the viewport starts horizontally.
        /// </summary>
        public float MinX
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the position where the viewport starts vertically.
        /// </summary>
        public float MinY
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the width of the viewport.
        /// </summary>
        public float Width
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the height of the viewport.
        /// </summary>
        public float Height
        {
            get;
            set;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Svg.SvgViewBox"/> to <see cref="System.Drawing.RectangleF"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator RectangleF(SvgViewBox value)
        {
            return new RectangleF(value.MinX, value.MinY, value.Width, value.Height);
        }
        
        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Drawing.RectangleF"/> to <see cref="Svg.SvgViewBox"/>.
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
        public SvgViewBox(float minX, float minY, float width, float height) : this()
        {
            this.MinX = minX;
            this.MinY = minY;
            this.Width = width;
            this.Height = height;
        }
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
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as string;
            if (s != null)
            {
                string[] coords = s.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (coords.Length != 4)
                {
                    throw new SvgException("The 'viewBox' attribute must be in the format 'minX, minY, width, height'.");
                }

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
                var viewBox = (SvgViewBox)value;

                return string.Format("{0}, {1}, {2}, {3}",
                    viewBox.MinX.ToString(CultureInfo.InvariantCulture), viewBox.MinY.ToString(CultureInfo.InvariantCulture),
                    viewBox.Width.ToString(CultureInfo.InvariantCulture), viewBox.Height.ToString(CultureInfo.InvariantCulture));
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}