using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg
{
    /// <summary>
    /// Converts string representations of colours into <see cref="Color"/> objects.
    /// </summary>
    public class SvgColourConverter : System.Drawing.ColorConverter
    {
        /// <summary>
        /// Converts the given object to the converter's native type.
        /// </summary>
        /// <param name="context">A <see cref="T:System.ComponentModel.TypeDescriptor"/> that provides a format context. You can use this object to get additional information about the environment from which this converter is being invoked.</param>
        /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo"/> that specifies the culture to represent the color.</param>
        /// <param name="value">The object to convert.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> representing the converted value.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">The conversion cannot be performed.</exception>
        /// <PermissionSet>
        /// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
        /// </PermissionSet>
        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            string colour = value as string;

            if (colour != null)
            {
                colour = colour.Trim();

                if (colour.StartsWith("rgb"))
                {
                    try
                    {
                        int start = colour.IndexOf("(") + 1;
                        string[] values = colour.Substring(start, colour.IndexOf(")") - start).Split(new char[]{',', ' '}, StringSplitOptions.RemoveEmptyEntries);

                        return System.Drawing.Color.FromArgb(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]));
                    }
                    catch
                    {
                        throw new SvgException("Colour is in an invalid format: '" + colour + "'");
                    }
                }
                else if (colour.StartsWith("#") && colour.Length == 4)
                {
                    colour = string.Format(culture, "#{0}{0}{1}{1}{2}{2}", colour[1], colour[2], colour[3]);
                    return base.ConvertFrom(context, culture, colour);
                }
                else
                {
                    return base.ConvertFrom(context, culture, value);
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}