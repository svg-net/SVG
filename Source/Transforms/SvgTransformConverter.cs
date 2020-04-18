using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Svg.Transforms
{
    public class SvgTransformConverter : TypeConverter
    {
        private static IEnumerable<string> SplitTransforms(string transforms)
        {
            transforms = transforms.Replace(',', ' ');
            var transformStart = 0;

            for (var i = 0; i < transforms.Length; ++i)
                if (transforms[i] == ')')
                {
                    yield return transforms.Substring(transformStart, i + 1 - transformStart).TrimStart();
                    transformStart = i + 1;
                }
        }

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
                var transformList = new SvgTransformCollection();

                string[] parts;
                string contents;
                string transformName;

                foreach (var transform in SplitTransforms((string)value))
                {
                    if (string.IsNullOrEmpty(transform))
                        continue;

                    parts = transform.Split('(', ')');
                    transformName = parts[0].TrimEnd();
                    contents = parts[1].Trim();

                    switch (transformName)
                    {
                        case "translate":
                            var coords = contents.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            if (coords.Length == 0 || coords.Length > 2)
                                throw new FormatException("Translate transforms must be in the format 'translate(x [y])'");

                            var x = float.Parse(coords[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture);
                            if (coords.Length > 1)
                            {
                                var y = float.Parse(coords[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture);
                                transformList.Add(new SvgTranslate(x, y));
                            }
                            else
                                transformList.Add(new SvgTranslate(x));
                            break;
                        case "rotate":
                            var args = contents.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            if (args.Length != 1 && args.Length != 3)
                                throw new FormatException("Rotate transforms must be in the format 'rotate(angle [cx cy])'");

                            var angle = float.Parse(args[0], NumberStyles.Float, CultureInfo.InvariantCulture);

                            if (args.Length == 1)
                                transformList.Add(new SvgRotate(angle));
                            else
                            {
                                var cx = float.Parse(args[1], NumberStyles.Float, CultureInfo.InvariantCulture);
                                var cy = float.Parse(args[2], NumberStyles.Float, CultureInfo.InvariantCulture);
                                transformList.Add(new SvgRotate(angle, cx, cy));
                            }
                            break;
                        case "scale":
                            var scales = contents.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            if (scales.Length == 0 || scales.Length > 2)
                                throw new FormatException("Scale transforms must be in the format 'scale(x [y])'");

                            var sx = float.Parse(scales[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture);
                            if (scales.Length > 1)
                            {
                                var sy = float.Parse(scales[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture);
                                transformList.Add(new SvgScale(sx, sy));
                            }
                            else
                                transformList.Add(new SvgScale(sx));
                            break;
                        case "matrix":
                            var points = contents.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            if (points.Length != 6)
                                throw new FormatException("Matrix transforms must be in the format 'matrix(m11 m12 m21 m22 dx dy)'");

                            var mPoints = new List<float>(6);
                            foreach (var point in points)
                                mPoints.Add(float.Parse(point.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture));

                            transformList.Add(new SvgMatrix(mPoints));
                            break;
                        case "shear":
                            var shears = contents.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            if (shears.Length == 0 || shears.Length > 2)
                                throw new FormatException("Shear transforms must be in the format 'shear(x [y])'");

                            var hx = float.Parse(shears[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture);
                            if (shears.Length > 1)
                            {
                                var hy = float.Parse(shears[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture);
                                transformList.Add(new SvgShear(hx, hy));
                            }
                            else
                                transformList.Add(new SvgShear(hx));
                            break;
                        case "skewX":
                            var ax = float.Parse(contents, NumberStyles.Float, CultureInfo.InvariantCulture);
                            transformList.Add(new SvgSkew(ax, 0f));
                            break;
                        case "skewY":
                            var ay = float.Parse(contents, NumberStyles.Float, CultureInfo.InvariantCulture);
                            transformList.Add(new SvgSkew(0f, ay));
                            break;
                    }
                }

                return transformList;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is SvgTransformCollection)
                return string.Join(" ", ((SvgTransformCollection)value).Select(t => t.WriteToString()).ToArray());

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
