using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using Svg.Helpers;

namespace Svg
{
    /// <summary>
    /// Converts string representations of colours into <see cref="Color"/> objects.
    /// </summary>
    public class SvgColourConverter : ColorConverter
    {
#if true
        private static readonly char[] SplitChars = new char[] { ',' , ' ' };

        public object Parse(ITypeDescriptorContext context, CultureInfo culture, ReadOnlySpan<char> colour)
        {
            colour = colour.Trim();

            // RGB support
            if (colour.IndexOf("rgb".AsSpan()) == 0)
            {
                try
                {
                    int start = colour.IndexOf('(') + 1;
                    int length = colour.IndexOf(')') - start;

                    //get the values from the RGB string
                    string[] values = colour.Substring(start, length).Split(SplitChars, StringSplitOptions.RemoveEmptyEntries);

                    //determine the alpha value if this is an RGBA (it will be the 4th value if there is one)
                    int alphaValue = 255;
                    if (values.Length > 3)
                    {
                        //the alpha portion of the rgba is not an int 0-255 it is a decimal between 0 and 1
                        //so we have to determine the corosponding byte value
                        var alphastring = values[3];
                        if (alphastring.StartsWith(".", StringComparison.InvariantCulture))
                        {
                            alphastring = "0" + alphastring;
                        }

                        var alphaDecimal = decimal.Parse(alphastring, CultureInfo.InvariantCulture);

                        if (alphaDecimal <= 1)
                        {
                            alphaValue = (int) Math.Round(alphaDecimal * 255);
                        }
                        else
                        {
                            alphaValue = (int) Math.Round(alphaDecimal);
                        }
                    }

                    Color colorpart;
                    if (values[0].Trim().EndsWith("%", StringComparison.InvariantCulture))
                    {
                        colorpart = Color.FromArgb(alphaValue,
                            (int) Math.Round(255 * float.Parse(values[0].Trim().TrimEnd('%'), NumberStyles.Any,
                                CultureInfo.InvariantCulture) / 100f),
                            (int) Math.Round(255 * float.Parse(values[1].Trim().TrimEnd('%'), NumberStyles.Any,
                                CultureInfo.InvariantCulture) / 100f),
                            (int) Math.Round(255 * float.Parse(values[2].Trim().TrimEnd('%'), NumberStyles.Any,
                                CultureInfo.InvariantCulture) / 100f));
                    }
                    else
                    {
                        colorpart = Color.FromArgb(alphaValue, int.Parse(values[0], CultureInfo.InvariantCulture),
                            int.Parse(values[1], CultureInfo.InvariantCulture),
                            int.Parse(values[2], CultureInfo.InvariantCulture));
                    }

                    return colorpart;
                }
                catch
                {
                    throw new SvgException("Colour is in an invalid format: '" + colour.ToString() + "'");
                }
            }

            // HSL support
            if (colour.IndexOf("hsl".AsSpan()) == 0)
            {
                try
                {
                    int start = colour.IndexOf('(') + 1;
                    int length = colour.IndexOf(')') - start;

                    //get the values from the RGB string

                    var parts = new StringSplitEnumerator(colour.Slice(start, length), SplitChars.AsSpan());




                    string[] values = colour.Substring(start, length).Split(SplitChars, StringSplitOptions.RemoveEmptyEntries);

                    if (values[1].EndsWith("%", StringComparison.InvariantCulture))
                    {
                        values[1] = values[1].TrimEnd('%');
                    }

                    if (values[2].EndsWith("%", StringComparison.InvariantCulture))
                    {
                        values[2] = values[2].TrimEnd('%');
                    }




                    // Get the HSL values in a range from 0 to 1.
                    double h = double.Parse(values[0], CultureInfo.InvariantCulture) / 360.0;
                    double s = double.Parse(values[1], CultureInfo.InvariantCulture) / 100.0;
                    double l = double.Parse(values[2], CultureInfo.InvariantCulture) / 100.0;

                    // Convert the HSL color to an RGB color
                    Color colorpart = Hsl2Rgb(h, s, l);
                    return colorpart;
                }
                catch
                {
                    throw new SvgException("Colour is in an invalid format: '" + colour.ToString() + "'");
                }
            }

            // HEX support
            if (colour.IndexOf('#') == 0 && colour.Length == 4)
            {
                // TODO: Optimize
                var hex = string.Format(culture, "#{0}{0}{1}{1}{2}{2}", colour[1], colour[2], colour[3]);
                return base.ConvertFrom(context, culture, hex);
            }

            // SystemColors support
            if (colour.Equals("activeborder".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.ActiveBorder;
            }
            if (colour.Equals("activecaption".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.ActiveCaption;
            }
            if (colour.Equals("appworkspace".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.AppWorkspace;
            }
            if (colour.Equals("background".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.Desktop;
            }
            if (colour.Equals("buttonface".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.Control;
            }
            if (colour.Equals("buttonhighlight".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.ControlLightLight;
            }
            if (colour.Equals("buttonshadow".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.ControlDark;
            }
            if (colour.Equals("buttontext".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.ControlText;
            }
            if (colour.Equals("captiontext".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.ActiveCaptionText;
            }
            if (colour.Equals("graytext".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.GrayText;
            }
            if (colour.Equals("highlight".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.Highlight;
            }
            if (colour.Equals("highlighttext".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.HighlightText;
            }
            if (colour.Equals("inactiveborder".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.InactiveBorder;
            }
            if (colour.Equals("inactivecaption".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.InactiveCaption;
            }
            if (colour.Equals("inactivecaptiontext".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.InactiveCaptionText;
            }
            if (colour.Equals("infobackground".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.Info;
            }
            if (colour.Equals("infotext".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.InfoText;
            }
            if (colour.Equals("menu".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.Menu;
            }
            if (colour.Equals("menutext".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.MenuText;
            }
            if (colour.Equals("scrollbar".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.ScrollBar;
            }
            if (colour.Equals("threeddarkshadow".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.ControlDarkDark;
            }
            if (colour.Equals("threedface".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.Control;
            }
            if (colour.Equals("threedhighlight".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.ControlLight;
            }
            if (colour.Equals("threedlightshadow".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.ControlLightLight;
            }
            if (colour.Equals("window".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.Window;
            }
            if (colour.Equals("windowframe".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.WindowFrame;
            }
            if (colour.Equals("windowtext".AsSpan(), StringComparison.InvariantCultureIgnoreCase))
            {
                return SystemColors.WindowText;
            }

            // Numbers are handled as colors by System.Drawing.ColorConverter - we
            // have to prevent this and ignore the color instead (see #342).
#if NETSTANDARD2_1 || NETCORE || NETCOREAPP2_2 || NETCOREAPP3_0
            if (int.TryParse(colour, NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
            {
                return SvgPaintServer.NotSet;
            }
#else
            if (int.TryParse(colour.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
            {
                return SvgPaintServer.NotSet;
            }
#endif
            var index = colour.LastIndexOf("grey".AsSpan(), StringComparison.InvariantCultureIgnoreCase);
            if (index >= 0 && index + 4 == colour.Length)
            {
                var gray = new StringBuilder(colour)
                    .Replace("grey", "gray", index, 4)
                    .Replace("Grey", "Gray", index, 4)
                    .ToString();

                return base.ConvertFrom(context, culture, gray);
            }

            return base.ConvertFrom(context, culture, colour);
        }

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
        ///     <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
        /// </PermissionSet>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string colour)
            {
                return Parse(context, culture, colour);
            }

            return base.ConvertFrom(context, culture, value);
        }
#else
        public object Parse(ITypeDescriptorContext context, CultureInfo culture, string colour)
        {
            colour = colour.Trim();

            // RGB support
            if (colour.StartsWith("rgb", StringComparison.InvariantCulture))
            {
                try
                {
                    int start = colour.IndexOf("(", StringComparison.InvariantCulture) + 1;

                    //get the values from the RGB string
                    string[] values = colour.Substring(start, colour.IndexOf(")", StringComparison.InvariantCulture) - start)
                        .Split(new char[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries);

                    //determine the alpha value if this is an RGBA (it will be the 4th value if there is one)
                    int alphaValue = 255;
                    if (values.Length > 3)
                    {
                        //the alpha portion of the rgba is not an int 0-255 it is a decimal between 0 and 1
                        //so we have to determine the corosponding byte value
                        var alphastring = values[3];
                        if (alphastring.StartsWith(".", StringComparison.InvariantCulture))
                        {
                            alphastring = "0" + alphastring;
                        }

                        var alphaDecimal = decimal.Parse(alphastring, CultureInfo.InvariantCulture);

                        if (alphaDecimal <= 1)
                        {
                            alphaValue = (int) Math.Round(alphaDecimal * 255);
                        }
                        else
                        {
                            alphaValue = (int) Math.Round(alphaDecimal);
                        }
                    }

                    Color colorpart;
                    if (values[0].Trim().EndsWith("%", StringComparison.InvariantCulture))
                    {
                        colorpart = Color.FromArgb(alphaValue,
                            (int) Math.Round(255 * float.Parse(values[0].Trim().TrimEnd('%'), NumberStyles.Any,
                                CultureInfo.InvariantCulture) / 100f),
                            (int) Math.Round(255 * float.Parse(values[1].Trim().TrimEnd('%'), NumberStyles.Any,
                                CultureInfo.InvariantCulture) / 100f),
                            (int) Math.Round(255 * float.Parse(values[2].Trim().TrimEnd('%'), NumberStyles.Any,
                                CultureInfo.InvariantCulture) / 100f));
                    }
                    else
                    {
                        colorpart = Color.FromArgb(alphaValue, int.Parse(values[0], CultureInfo.InvariantCulture),
                            int.Parse(values[1], CultureInfo.InvariantCulture),
                            int.Parse(values[2], CultureInfo.InvariantCulture));
                    }

                    return colorpart;
                }
                catch
                {
                    throw new SvgException("Colour is in an invalid format: '" + colour + "'");
                }
            }

            // HSL support
            if (colour.StartsWith("hsl", StringComparison.InvariantCulture))
            {
                try
                {
                    int start = colour.IndexOf("(", StringComparison.InvariantCulture) + 1;

                    //get the values from the RGB string
                    string[] values = colour.Substring(start, colour.IndexOf(")", StringComparison.InvariantCulture) - start)
                        .Split(new char[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries);
                    if (values[1].EndsWith("%", StringComparison.InvariantCulture))
                    {
                        values[1] = values[1].TrimEnd('%');
                    }

                    if (values[2].EndsWith("%", StringComparison.InvariantCulture))
                    {
                        values[2] = values[2].TrimEnd('%');
                    }

                    // Get the HSL values in a range from 0 to 1.
                    double h = double.Parse(values[0], CultureInfo.InvariantCulture) / 360.0;
                    double s = double.Parse(values[1], CultureInfo.InvariantCulture) / 100.0;
                    double l = double.Parse(values[2], CultureInfo.InvariantCulture) / 100.0;
                    // Convert the HSL color to an RGB color
                    Color colorpart = Hsl2Rgb(h, s, l);
                    return colorpart;
                }
                catch
                {
                    throw new SvgException("Colour is in an invalid format: '" + colour + "'");
                }
            }

            // HEX support
            if (colour.StartsWith("#", StringComparison.InvariantCulture) && colour.Length == 4)
            {
                colour = string.Format(culture, "#{0}{0}{1}{1}{2}{2}", colour[1], colour[2], colour[3]);
                return base.ConvertFrom(context, culture, colour);
            }

            // SystemColors support
            switch (colour.ToLowerInvariant())
            {
                case "activeborder": return SystemColors.ActiveBorder;
                case "activecaption": return SystemColors.ActiveCaption;
                case "appworkspace": return SystemColors.AppWorkspace;
                case "background": return SystemColors.Desktop;
                case "buttonface": return SystemColors.Control;
                case "buttonhighlight": return SystemColors.ControlLightLight;
                case "buttonshadow": return SystemColors.ControlDark;
                case "buttontext": return SystemColors.ControlText;
                case "captiontext": return SystemColors.ActiveCaptionText;
                case "graytext": return SystemColors.GrayText;
                case "highlight": return SystemColors.Highlight;
                case "highlighttext": return SystemColors.HighlightText;
                case "inactiveborder": return SystemColors.InactiveBorder;
                case "inactivecaption": return SystemColors.InactiveCaption;
                case "inactivecaptiontext": return SystemColors.InactiveCaptionText;
                case "infobackground": return SystemColors.Info;
                case "infotext": return SystemColors.InfoText;
                case "menu": return SystemColors.Menu;
                case "menutext": return SystemColors.MenuText;
                case "scrollbar": return SystemColors.ScrollBar;
                case "threeddarkshadow": return SystemColors.ControlDarkDark;
                case "threedface": return SystemColors.Control;
                case "threedhighlight": return SystemColors.ControlLight;
                case "threedlightshadow": return SystemColors.ControlLightLight;
                case "window": return SystemColors.Window;
                case "windowframe": return SystemColors.WindowFrame;
                case "windowtext": return SystemColors.WindowText;
            }

            // Numbers are handled as colors by System.Drawing.ColorConverter - we
            // have to prevent this and ignore the color instead (see #342).
            if (int.TryParse(colour, NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
            {
                return SvgPaintServer.NotSet;
            }

            var index = colour.LastIndexOf("grey", StringComparison.InvariantCultureIgnoreCase);
            if (index >= 0 && index + 4 == colour.Length)
            {
                var gray = new StringBuilder(colour)
                    .Replace("grey", "gray", index, 4)
                    .Replace("Grey", "Gray", index, 4)
                    .ToString();

                return base.ConvertFrom(context, culture, gray);
            }

            return base.ConvertFrom(context, culture, colour);
        }

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
        ///     <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
        /// </PermissionSet>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string colour)
            {
                return Parse(context, culture, colour);
            }

            return base.ConvertFrom(context, culture, value);
        }
#endif

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
                var colorString = ColorTranslator.ToHtml((Color)value).Replace("LightGrey", "LightGray");
                // color names are expected to be lower case in XML
                return colorString.StartsWith("#", StringComparison.InvariantCulture) ? colorString : colorString.ToLowerInvariant();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// Converts HSL color (with HSL specified from 0 to 1) to RGB color.
        /// Taken from http://www.geekymonkey.com/Programming/CSharp/RGB2HSL_HSL2RGB.htm
        /// </summary>
        /// <param name="h"></param>
        /// <param name="sl"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        private static Color Hsl2Rgb(double h, double sl, double l)
        {
            double r = l;   // default to gray
            double g = l;
            double b = l;
            double v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);
            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = l + l - v;
                sv = (v - m) / v;
                h *= 6.0;
                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;
                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }
            Color rgb = Color.FromArgb((int)Math.Round(r * 255.0), (int)Math.Round(g * 255.0), (int)Math.Round(b * 255.0));
            return rgb;
        }
    }
}
