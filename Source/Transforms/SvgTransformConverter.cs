using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Svg.Helpers;

namespace Svg.Transforms
{
    public class SvgTransformConverter : TypeConverter
    {
        private static readonly char[] SplitChars = new[] { ' ', ',' };
        private const string TranslateTransform = "translate";
        private const string RotateTransform = "rotate";
        private const string ScaleTransform = "scale";
        private const string MatrixTransform = "matrix";
        private const string ShearTransform = "shear";
        private const string SkewXTransform = "skewX";
        private const string SkewYTransform = "skewY";

        private enum TransformType
        {
            Invalid,
            Translate,
            Rotate,
            Scale,
            Matrix,
            Shear,
            SkewX,
            SkewY
        }

        private static TransformType GetTransformType(ref ReadOnlySpan<char> transformName)
        {
            if (transformName.SequenceEqual(TranslateTransform.AsSpan()))
            {
                return TransformType.Translate;
            }
            else if (transformName.SequenceEqual(RotateTransform.AsSpan()))
            {
                return TransformType.Rotate;
            }
            else if (transformName.SequenceEqual(ScaleTransform.AsSpan()))
            {
                return TransformType.Scale;
            }
            else if (transformName.SequenceEqual(MatrixTransform.AsSpan()))
            {
                return TransformType.Matrix;
            }
            else if (transformName.SequenceEqual(ShearTransform.AsSpan()))
            {
                return TransformType.Shear;
            }
            else if (transformName.SequenceEqual(SkewXTransform.AsSpan()))
            {
                return TransformType.SkewX;
            }
            else if (transformName.SequenceEqual(SkewYTransform.AsSpan()))
            {
                return TransformType.SkewY;
            }
            return TransformType.Invalid;
        }

        private static float ToFloat(ref ReadOnlySpan<char> value)
        {
#if NETSTANDARD2_1 || NETCORE || NETCOREAPP2_2 || NETCOREAPP3_0
            return float.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture);
#else
            return float.Parse(value.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture);
#endif
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
            if (value is not string str)
            {
                return base.ConvertFrom(context, culture, value);
            }

            var transformList = new SvgTransformCollection();
            var source = str.AsSpan().TrimStart();
            var sourceLength = source.Length;
            var splitChars = SplitChars.AsSpan();

            while (true)
            {
                var currentIndex = 0;
                var startIndex = source.IndexOf('(');
                var endIndex = source.IndexOf(')');

                if (startIndex < 0 || endIndex <= startIndex)
                {
                    break;
                }

                var transformName = source.Slice(currentIndex, startIndex - currentIndex).Trim().Trim(',').Trim();
                var contents = source.Slice(startIndex + 1, endIndex - startIndex - 1).Trim();
                var parts = new StringSplitEnumerator(contents, splitChars);
                var transformType = GetTransformType(ref transformName);

                switch (transformType)
                {
                    case TransformType.Translate:
                        {
                            var count = 0;
                            var x = default(float);
                            var y = default(float);

                            foreach (var part in parts)
                            {
                                var partValue = part.Value;
                                if (count == 0)
                                {
                                    x = ToFloat(ref partValue);
                                }
                                else if (count == 1)
                                {
                                    y = ToFloat(ref partValue);
                                }
                                count++;
                            }

                            if (count == 0 || count > 2)
                            {
                                throw new FormatException("Translate transforms must be in the format 'translate(x [y])'");
                            }

                            transformList.Add(count > 1 ? new SvgTranslate(x, y) : new SvgTranslate(x));
                        }
                        break;
                    case TransformType.Rotate:
                        {
                            int count = 0;
                            var angle = default(float);
                            var cx = default(float);
                            var cy = default(float);

                            foreach (var part in parts)
                            {
                                var partValue = part.Value;
                                if (count == 0)
                                {
                                    angle = ToFloat(ref partValue);
                                }
                                else if (count == 1)
                                {
                                    cx = ToFloat(ref partValue);
                                }
                                else if (count == 2)
                                {
                                    cy = ToFloat(ref partValue);
                                }
                                count++;
                            }

                            if (count != 1 && count != 3)
                            {
                                throw new FormatException("Rotate transforms must be in the format 'rotate(angle [cx cy])'");
                            }

                            transformList.Add(count == 1 ? new SvgRotate(angle) : new SvgRotate(angle, cx, cy));
                        }
                        break;
                    case TransformType.Scale:
                        {
                            int count = 0;
                            var sx = default(float);
                            var sy = default(float);

                            foreach (var part in parts)
                            {
                                var partValue = part.Value;
                                if (count == 0)
                                {
                                    sx = ToFloat(ref partValue);
                                }
                                else if (count == 1)
                                {
                                    sy = ToFloat(ref partValue);
                                }
                                count++;
                            }

                            if (count == 0 || count > 2)
                            {
                                throw new FormatException("Scale transforms must be in the format 'scale(x [y])'");
                            }

                            transformList.Add(count > 1 ? new SvgScale(sx, sy) : new SvgScale(sx));
                        }
                        break;
                    case TransformType.Matrix:
                        {
                            int count = 0;
                            var m11 = default(float);
                            var m12 = default(float);
                            var m21 = default(float);
                            var m22 = default(float);
                            var dx = default(float);
                            var dy = default(float);

                            foreach (var part in parts)
                            {
                                var partValue = part.Value;
                                if (count == 0)
                                {
                                    m11 = ToFloat(ref partValue);
                                }
                                else if (count == 1)
                                {
                                    m12 = ToFloat(ref partValue);
                                }
                                else if (count == 2)
                                {
                                    m21 = ToFloat(ref partValue);
                                }
                                else if (count == 3)
                                {
                                    m22 = ToFloat(ref partValue);
                                }
                                else if (count == 4)
                                {
                                    dx = ToFloat(ref partValue);
                                }
                                else if (count == 5)
                                {
                                    dy = ToFloat(ref partValue);
                                }
                                count++;
                            }

                            if (count != 6)
                            {
                                throw new FormatException("Matrix transforms must be in the format 'matrix(m11 m12 m21 m22 dx dy)'");
                            }

                            transformList.Add(new SvgMatrix(new List<float>(6) { m11, m12, m21, m22, dx, dy }));
                        }
                        break;
                    case TransformType.Shear:
                        {
                            int count = 0;
                            var hx = default(float);
                            var hy = default(float);

                            foreach (var part in parts)
                            {
                                var partValue = part.Value;
                                if (count == 0)
                                {
                                    hx = ToFloat(ref partValue);
                                }
                                else if (count == 1)
                                {
                                    hy = ToFloat(ref partValue);
                                }
                                count++;
                            }

                            if (count == 0 || count > 2)
                            {
                                throw new FormatException("Shear transforms must be in the format 'shear(x [y])'");
                            }

                            transformList.Add(count > 1 ? new SvgShear(hx, hy) : new SvgShear(hx));
                        }
                        break;
                    case TransformType.SkewX:
                        {
                            int count = 0;
                            var ax = default(float);

                            foreach (var part in parts)
                            {
                                var partValue = part.Value;
                                if (count == 0)
                                {
                                    ax = ToFloat(ref partValue);
                                }
                                count++;
                            }

                            if (count != 1)
                            {
                                throw new FormatException("SkewX transforms must be in the format 'skewX(a)'");
                            }

                            transformList.Add(new SvgSkew(ax, 0f));
                        }
                        break;
                    case TransformType.SkewY:
                        {
                            int count = 0;
                            var ay = default(float);

                            foreach (var part in parts)
                            {
                                var partValue = part.Value;
                                if (count == 0)
                                {
                                    ay = ToFloat(ref partValue);
                                }
                                count++;
                            }

                            if (count != 1)
                            {
                                throw new FormatException("SkewY transforms must be in the format 'skewY(a)'");
                            }

                            transformList.Add(new SvgSkew(0f, ay));
                        }
                        break;
                }

                currentIndex = endIndex;
                if (currentIndex + 1 > sourceLength)
                {
                    break;
                }

                source = source.Slice(currentIndex + 1, sourceLength - currentIndex - 1).TrimStart();
                sourceLength = source.Length;
                if (sourceLength <= 0)
                {
                    break;
                }
            }

            return transformList;

        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is SvgTransformCollection collection)
                return string.Join(" ", collection.Select(t => t.WriteToString()).ToArray());

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
