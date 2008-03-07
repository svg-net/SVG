using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Svg.Transforms
{
    public class SvgTransformConverter : TypeConverter
    {
        private static IEnumerable<string> SplitTransforms(string transforms)
        {
            int transformEnd = 0;

            for (int i = 0; i < transforms.Length; i++)
            {
                if (transforms[i] == ')')
                {
                    yield return transforms.Substring(transformEnd, i - transformEnd + 1).Trim();
                    transformEnd = i+1;
                }
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
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                SvgTransformCollection transformList = new SvgTransformCollection();

                string[] parts;
                string contents;
                string transformName;

                foreach (string transform in SvgTransformConverter.SplitTransforms((string)value))
                {
                    if (string.IsNullOrEmpty(transform))
                        continue;

                    parts = transform.Split('(', ')');
                    transformName = parts[0].Trim();
                    contents = parts[1].Trim();

                    switch (transformName)
                    {
                        case "translate":
                            string[] coords = contents.Split(new char[]{',', ' '}, StringSplitOptions.RemoveEmptyEntries);
                            float x = float.Parse(coords[0].Trim());
                            float y = float.Parse(coords[1].Trim());
                            transformList.Add(new SvgTranslate(x, y));
                            break;
                        case "rotate":
                            float angle = float.Parse(contents);
                            transformList.Add(new SvgRotate(angle));
                            break;
                        case "scale":
                            float scaleFactor = float.Parse(contents);
                            transformList.Add(new SvgScale(scaleFactor));
                            break;
                    }
                }

                return transformList;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}