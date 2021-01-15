using System;
using System.Collections.Generic;
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
        private static readonly char[] SplitChars = new char[] { ',' , ' ' };

        private static int ClampInt(int value, int min, int max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        private static float ClampFloat(float value, float min, float max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        private static double ClampDouble(double value, double min, double max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        private static double ClampHue(double h)
        {
            if (h > 360.0)
            {
                h = h % 360.0;
            }

            if (h < 0)
            {
                h = 360.0 - (-h) % 360.0;
            }

            return h;
        }

        public static uint ToArgb(int alpha, int red, int green, int blue)
        {
            return (uint) alpha << 24 | (uint) red << 16 | (uint) green << 8 | (uint) blue << 0;
        }

        public object Parse(ITypeDescriptorContext context, CultureInfo culture, string colour)
        {
            var span = colour.AsSpan().Trim();

            // RGB support
            if (span.IndexOf("rgb".AsSpan()) == 0)
            {
                try
                {
                    // get the values from the RGB string
                    var start = span.IndexOf('(') + 1;
                    var length = span.IndexOf(')') - start;
                    var parts = new StringSplitEnumerator(span.Slice(start, length), SplitChars.AsSpan());
                    var count = 0;
                    int alpha = 255;
                    int red = default;
                    int green = default;
                    int blue = default;
                    bool isDecimal = false;

                    foreach (var part in parts)
                    {
                        var partValue = part.Value;
                        if (count == 0)
                        {
                            if (partValue.IndexOf('%') == partValue.Length - 1)
                            {
                                partValue = partValue.TrimEnd('%');
                                var redDecimal = StringParser.ToFloatAny(ref partValue);
                                redDecimal = ClampFloat(redDecimal, 0f, 100f);
                                red = (int) Math.Round(255 * redDecimal / 100f);
                                isDecimal = true;
                            }
                            else
                            {
                                red = StringParser.ToInt(ref partValue);
                                red = ClampInt(red, 0, 255);
                                isDecimal = false;
                            }
                        }
                        else if (count == 1)
                        {
                            if (partValue.IndexOf('%') == partValue.Length - 1)
                            {
                                if (!isDecimal)
                                {
                                    throw new SvgException("Colour is in an invalid format: '" + span.ToString() + "'");
                                }

                                partValue = partValue.TrimEnd('%');
                                var greenDecimal = StringParser.ToFloatAny(ref partValue);
                                greenDecimal = ClampFloat(greenDecimal, 0f, 100f);
                                green = (int) Math.Round(255 * greenDecimal / 100f);
                            }
                            else
                            {
                                if (isDecimal)
                                {
                                    throw new SvgException("Colour is in an invalid format: '" + span.ToString() + "'");
                                }

                                green = StringParser.ToInt(ref partValue);
                                green = ClampInt(green, 0, 255);
                            }
                        }
                        else if (count == 2)
                        {
                            if (partValue.IndexOf('%') == partValue.Length - 1)
                            {
                                if (!isDecimal)
                                {
                                    throw new SvgException("Colour is in an invalid format: '" + span.ToString() + "'");
                                }

                                partValue = partValue.TrimEnd('%');
                                var blueDecimal = StringParser.ToFloatAny(ref partValue);
                                blueDecimal = ClampFloat(blueDecimal, 0f, 100f);
                                blue = (int) Math.Round(255 * blueDecimal / 100f);
                            }
                            else
                            {
                                if (isDecimal)
                                {
                                    throw new SvgException("Colour is in an invalid format: '" + span.ToString() + "'");
                                }

                                blue = StringParser.ToInt(ref partValue);
                                blue = ClampInt(blue, 0, 255);
                            }
                        }
                        else if (count == 3)
                        {
                            // determine the alpha value if this is an RGBA (it will be the 4th value if there is one)
                            // the alpha portion of the rgba is not an int 0-255 it is a decimal between 0 and 1
                            // so we have to determine the corresponding byte value
                            if (partValue.IndexOf('.') == 0)
                            {
                                // TODO: partValue = "0" + partValue;
                            }

                            var alphaDecimal = StringParser.ToDouble(ref partValue);
                            if (alphaDecimal <= 1)
                            {
                                alphaDecimal = ClampDouble(alphaDecimal, 0f, 1f);
                                alpha = (int) Math.Round(alphaDecimal * 255);
                            }
                            else
                            {
                                alphaDecimal = ClampDouble(alphaDecimal, 0f, 255f);
                                alpha = (int) Math.Round(alphaDecimal);
                            }
                        }
                        else
                        {
                            throw new SvgException("Colour is in an invalid format: '" + span.ToString() + "'");
                        }

                        count++;
                    }

                    var argb = ToArgb(alpha, red, green, blue);
                    if (ArgbToNamedColorDictionary.TryGetValue(argb, out var argbNamedColor))
                    {
                        return argbNamedColor();
                    }
                    return Color.FromArgb(alpha, red, green, blue);
                }
                catch
                {
                    throw new SvgException("Colour is in an invalid format: '" + span.ToString() + "'");
                }
            }

            // HSL support
            if (span.IndexOf("hsl".AsSpan()) == 0)
            {
                try
                {
                    // get the values from the HSL string
                    var start = span.IndexOf('(') + 1;
                    var length = span.IndexOf(')') - start;
                    var parts = new StringSplitEnumerator(span.Slice(start, length), SplitChars.AsSpan());
                    var count = 0;
                    var h = default(double);
                    var s = default(double);
                    var l = default(double);

                    // Get the HSL values in a range from 0 to 1.
                    foreach (var part in parts)
                    {
                        var partValue = part.Value;
                        if (count == 0)
                        {
                            var hDecimal = StringParser.ToDouble(ref partValue);
                            hDecimal = ClampHue(hDecimal);
                            h = hDecimal / 360.0;
                        }
                        else if (count == 1)
                        {
                            if (partValue.IndexOf('%') == partValue.Length - 1)
                            {
                                partValue = partValue.TrimEnd('%');

                                var sDecimal = StringParser.ToDouble(ref partValue);
                                sDecimal = ClampDouble(sDecimal, 0f, 100f);
                                s = sDecimal / 100.0;
                            }
                            else
                            {
                                throw new SvgException("Colour is in an invalid format: '" + span.ToString() + "'");
                            }
                        }
                        else if (count == 2)
                        {
                            if (partValue.IndexOf('%') == partValue.Length - 1)
                            {
                                partValue = partValue.TrimEnd('%');
                                var lDecimal = StringParser.ToDouble(ref partValue);
                                lDecimal = ClampDouble(lDecimal, 0f, 100f);
                                l = lDecimal / 100.0;
                            }
                            else
                            {
                                throw new SvgException("Colour is in an invalid format: '" + span.ToString() + "'");
                            }
                        }
                        else
                        {
                            throw new SvgException("Colour is in an invalid format: '" + span.ToString() + "'");
                        }

                        count++;
                    }

                    // Convert the HSL color to an RGB color
                    Hsl2Rgb(h, s, l, out var red, out var green, out var blue);
                    var argb = ToArgb(255, red, green, blue);
                    if (ArgbToNamedColorDictionary.TryGetValue(argb, out var argbNamedColor))
                    {
                        return argbNamedColor();
                    }
                    return Color.FromArgb(255, red, green, blue);
                }
                catch
                {
                    throw new SvgException("Colour is in an invalid format: '" + span.ToString() + "'");
                }
            }

            // HEX support
            if (span.IndexOf('#') == 0)
            {
                // The format of an RGB value in hexadecimal notation is a '#' immediately followed by either three or six hexadecimal characters.

                if (span.Length == 4)
                {
                    // The three-digit RGB notation (#rgb) is converted into six-digit form (#rrggbb) by replicating digits, not by adding zeros.
                    // For example, #fb0 expands to #ffbb00.
                    Span<char> redString = stackalloc char[2] {span[1], span[1]};
                    Span<char> greenString = stackalloc char[2] {span[2], span[2]};
                    Span<char> blueString = stackalloc char[2] {span[3], span[3]};
#if NETSTANDARD2_1 || NETCORE || NETCOREAPP2_1 || NETCOREAPP3_1 || NET5_0
                    var red = int.Parse(redString, NumberStyles.AllowHexSpecifier);
                    var green = int.Parse(greenString, NumberStyles.AllowHexSpecifier);
                    var blue = int.Parse(blueString, NumberStyles.AllowHexSpecifier);
#else
                    var red = int.Parse(redString.ToString(), NumberStyles.AllowHexSpecifier);
                    var green = int.Parse(greenString.ToString(), NumberStyles.AllowHexSpecifier);
                    var blue = int.Parse(blueString.ToString(), NumberStyles.AllowHexSpecifier);
#endif
                    var argb = ToArgb(255, red, green, blue);
                    if (ArgbToNamedColorDictionary.TryGetValue(argb, out var argbNamedColor))
                    {
                        return argbNamedColor();
                    }
                    return Color.FromArgb(255, red, green, blue);
                }

                if (span.Length == 7)
                {
                    var redString = span.Slice(1, 2);
                    var greenString = span.Slice(3, 2);
                    var blueString = span.Slice(5, 2);
#if NETSTANDARD2_1 || NETCORE || NETCOREAPP2_1 || NETCOREAPP3_1 || NET5_0
                    var red = int.Parse(redString, NumberStyles.AllowHexSpecifier);
                    var green = int.Parse(greenString, NumberStyles.AllowHexSpecifier);
                    var blue = int.Parse(blueString, NumberStyles.AllowHexSpecifier);
#else
                    var red = int.Parse(redString.ToString(), NumberStyles.AllowHexSpecifier);
                    var green = int.Parse(greenString.ToString(), NumberStyles.AllowHexSpecifier);
                    var blue = int.Parse(blueString.ToString(), NumberStyles.AllowHexSpecifier);
#endif
                    var argb = ToArgb(255, red, green, blue);
                    if (ArgbToNamedColorDictionary.TryGetValue(argb, out var argbNamedColor))
                    {
                        return argbNamedColor();
                    }
                    return Color.FromArgb(255, red, green, blue);
                }
            }

            // Colors support
            if (TryToGetNamedColor(ref span, out var namedColor))
            {
                return namedColor;
            }

            // System Colors support
            if (TryToGetSystemColor(ref span, out var systemColor))
            {
                return systemColor;
            }

            // Grey Colors support
            if (TryToGetGreyColor(ref span, out var greyColor))
            {
                return greyColor;
            }

            return SvgPaintServer.NotSet;
        }

        private static uint ComputeStringHash(in ReadOnlySpan<char> text)
        {
            uint hashCode = 0;
            if (text != null)
            {
                var length = text.Length;

                hashCode = unchecked((uint)2166136261);

                int i = 0;
                goto start;

                again:
                hashCode = unchecked((text[i] ^ hashCode) * 16777619);
                i = i + 1;

                start:
                if (i < length)
                    goto again;
            }
            return hashCode;
        }

        public static void ToLowerAscii(in ReadOnlySpan<char> colour, ref Span<char> buffer)
        {
            for (int i = 0; i < colour.Length; i++)
            {
                var c = colour[i];
                switch (c)
                {
                    case 'A': buffer[i] = 'a'; break;
                    case 'B': buffer[i] = 'b'; break;
                    case 'C': buffer[i] = 'c'; break;
                    case 'D': buffer[i] = 'd'; break;
                    case 'E': buffer[i] = 'e'; break;
                    case 'F': buffer[i] = 'f'; break;
                    case 'G': buffer[i] = 'g'; break;
                    case 'H': buffer[i] = 'h'; break;
                    case 'I': buffer[i] = 'i'; break;
                    case 'J': buffer[i] = 'j'; break;
                    case 'K': buffer[i] = 'k'; break;
                    case 'L': buffer[i] = 'l'; break;
                    case 'M': buffer[i] = 'm'; break;
                    case 'N': buffer[i] = 'n'; break;
                    case 'O': buffer[i] = 'o'; break;
                    case 'P': buffer[i] = 'p'; break;
                    case 'Q': buffer[i] = 'q'; break;
                    case 'R': buffer[i] = 'r'; break;
                    case 'S': buffer[i] = 's'; break;
                    case 'T': buffer[i] = 't'; break;
                    case 'U': buffer[i] = 'u'; break;
                    case 'V': buffer[i] = 'v'; break;
                    case 'W': buffer[i] = 'w'; break;
                    case 'X': buffer[i] = 'x'; break;
                    case 'Y': buffer[i] = 'y'; break;
                    case 'Z': buffer[i] = 'z'; break;
                    default: buffer[i] = c; break;
                }
            }
        }

        public static bool TryToGetGreyColor(ref ReadOnlySpan<char> colour, out Color greyColor)
        {
            Span<char> buffer = stackalloc char[colour.Length];
            ToLowerAscii(colour, ref buffer);
            var stringHash = ComputeStringHash(buffer);
            switch(stringHash)
            {
                case 0x7fb0e019: greyColor = Color.DarkSlateGray; return true; // darkslategrey
                case 0xb29019a6: greyColor = Color.Gray; return true; // grey
                case 0x23b55208: greyColor = Color.LightGray; return true; // lightgrey
            }
            greyColor = default;
            return false;
        }

        public static bool TryToGetSystemColor(ref ReadOnlySpan<char> colour, out Color systemColor)
        {
            Span<char> buffer = stackalloc char[colour.Length];
            ToLowerAscii(colour, ref buffer);
            var stringHash = ComputeStringHash(buffer);
            switch(stringHash)
            {
                case 0x96b1f469: systemColor = SystemColors.ActiveBorder; return true; // activeborder
                case 0x2cc5885f: systemColor = SystemColors.ActiveCaption; return true; // activecaption
                case 0xb4f0f429: systemColor = SystemColors.AppWorkspace; return true; // appworkspace
                case 0x4babd89d: systemColor = SystemColors.Desktop; return true; // background
                case 0xb8a66038: systemColor = SystemColors.ButtonFace; return true; // buttonface
                case 0x9050ce9b: systemColor = SystemColors.ControlLightLight; return true; // buttonhighlight
                case 0x6ceea1b5: systemColor = SystemColors.ControlDark; return true; // buttonshadow
                case 0xb6b04242: systemColor = SystemColors.ControlText; return true; // buttontext
                case 0xf29413de: systemColor = SystemColors.ActiveCaptionText; return true; // captiontext
                case 0x9642ba91: systemColor = SystemColors.GrayText; return true; // graytext
                case 0x1c9ff127: systemColor = SystemColors.Highlight; return true; // highlight
                case 0x635b6be0: systemColor = SystemColors.HighlightText; return true; // highlighttext
                case 0xa59d8bc6: systemColor = SystemColors.InactiveBorder; return true; // inactiveborder
                case 0xba88bd1a: systemColor = SystemColors.InactiveCaption; return true; // inactivecaption
                case 0xcb67dc11: systemColor = SystemColors.InactiveCaptionText; return true; // inactivecaptiontext
                case 0x9c8c4bdd: systemColor = SystemColors.Info; return true; // infobackground
                case 0xb164837e: systemColor = SystemColors.InfoText; return true; // infotext
                case 0x99e4dd3a: systemColor = SystemColors.Menu; return true; // menu
                case 0x4c924831: systemColor = SystemColors.MenuText; return true; // menutext
                case 0xd5b6c079: systemColor = SystemColors.ScrollBar; return true; // scrollbar
                case 0xffa62901: systemColor = SystemColors.ControlDarkDark; return true; // threeddarkshadow
                case 0x77fd6efc: systemColor = SystemColors.Control; return true; // threedface
                case 0xd4724bc7: systemColor = SystemColors.ControlLight; return true; // threedhighlight
                case 0x238bb757: systemColor = SystemColors.ControlLightLight; return true; // threedlightshadow
                case 0xa172b7dd: systemColor = SystemColors.Window; return true; // window
                case 0x6f554c7e: systemColor = SystemColors.WindowFrame; return true; // windowframe
                case 0xe477b746: systemColor = SystemColors.WindowText; return true; // windowtext
            }
            systemColor = default;
            return false;
        }

        public static bool TryToGetNamedColor(ref ReadOnlySpan<char> colour, out Color namedColor)
        {
            Span<char> buffer = stackalloc char[colour.Length];
            ToLowerAscii(colour, ref buffer);
            var stringHash = ComputeStringHash(buffer);
            switch(stringHash)
            {
                case 0x262562c3: namedColor = Color.AliceBlue; return true; // aliceblue
                case 0xcd4bddcf: namedColor = Color.AntiqueWhite; return true; // antiquewhite
                case 0x42374f95: namedColor = Color.Aqua; return true; // aqua
                case 0xf3aa9557: namedColor = Color.Aquamarine; return true; // aquamarine
                case 0x3758c2e0: namedColor = Color.Azure; return true; // azure
                case 0x38540573: namedColor = Color.Beige; return true; // beige
                case 0xc17f9b3a: namedColor = Color.Bisque; return true; // bisque
                case 0x568f4ba4: namedColor = Color.Black; return true; // black
                case 0x16ee77b1: namedColor = Color.BlanchedAlmond; return true; // blanchedalmond
                case 0x82fbf5cd: namedColor = Color.Blue; return true; // blue
                case 0x419e5a8a: namedColor = Color.BlueViolet; return true; // blueviolet
                case 0x30be372f: namedColor = Color.Brown; return true; // brown
                case 0x952635e8: namedColor = Color.BurlyWood; return true; // burlywood
                case 0x480fb75e: namedColor = Color.CadetBlue; return true; // cadetblue
                case 0x66bc38dd: namedColor = Color.Chartreuse; return true; // chartreuse
                case 0x429bb099: namedColor = Color.Chocolate; return true; // chocolate
                case 0x6d9b9752: namedColor = Color.Coral; return true; // coral
                case 0x559a4808: namedColor = Color.CornflowerBlue; return true; // cornflowerblue
                case 0x1ed7a4ea: namedColor = Color.Cornsilk; return true; // cornsilk
                case 0x042602ee: namedColor = Color.Crimson; return true; // crimson
                case 0x4961533a: namedColor = Color.Cyan; return true; // cyan
                case 0x0817f94d: namedColor = Color.DarkBlue; return true; // darkblue
                case 0xce7d56ba: namedColor = Color.DarkCyan; return true; // darkcyan
                case 0x1a236609: namedColor = Color.DarkGoldenrod; return true; // darkgoldenrod
                case 0x3fb6b71a: namedColor = Color.DarkGray; return true; // darkgray
                case 0x0c376f3c: namedColor = Color.DarkGreen; return true; // darkgreen
                case 0x7cacbc39: namedColor = Color.DarkKhaki; return true; // darkkhaki
                case 0x1e8db068: namedColor = Color.DarkMagenta; return true; // darkmagenta
                case 0x9049cd77: namedColor = Color.DarkOliveGreen; return true; // darkolivegreen
                case 0x3edce36b: namedColor = Color.DarkOrange; return true; // darkorange
                case 0xa60f29ba: namedColor = Color.DarkOrchid; return true; // darkorchid
                case 0xebd89f5c: namedColor = Color.DarkRed; return true; // darkred
                case 0x32db86df: namedColor = Color.DarkSalmon; return true; // darksalmon
                case 0x0340e137: namedColor = Color.DarkSeaGreen; return true; // darkseagreen
                case 0x05b72af6: namedColor = Color.DarkSlateBlue; return true; // darkslateblue
                case 0x87a7f255: namedColor = Color.DarkSlateGray; return true; // darkslategray
                case 0xe707aada: namedColor = Color.DarkTurquoise; return true; // darkturquoise
                case 0x7495c772: namedColor = Color.DarkViolet; return true; // darkviolet
                case 0x4f9530f7: namedColor = Color.DeepPink; return true; // deeppink
                case 0x5cabe370: namedColor = Color.DeepSkyBlue; return true; // deepskyblue
                case 0x29f41e26: namedColor = Color.DimGray; return true; // dimgray
                case 0x51b1437e: namedColor = Color.DodgerBlue; return true; // dodgerblue
                case 0x677785b6: namedColor = Color.Firebrick; return true; // firebrick
                case 0x82e33fb4: namedColor = Color.FloralWhite; return true; // floralwhite
                case 0xb8ea87d3: namedColor = Color.ForestGreen; return true; // forestgreen
                case 0x03e1343a: namedColor = Color.Fuchsia; return true; // fuchsia
                case 0xe15092fb: namedColor = Color.Gainsboro; return true; // gainsboro
                case 0xd57e7bfd: namedColor = Color.GhostWhite; return true; // ghostwhite
                case 0xec66d793: namedColor = Color.Gold; return true; // gold
                case 0xa8543b89: namedColor = Color.Goldenrod; return true; // goldenrod
                case 0xba9ab39a: namedColor = Color.Gray; return true; // gray
                case 0x011decbc: namedColor = Color.Green; return true; // green
                case 0x0d8329fc: namedColor = Color.GreenYellow; return true; // greenyellow
                case 0x60605b18: namedColor = Color.Honeydew; return true; // honeydew
                case 0x17c1a698: namedColor = Color.HotPink; return true; // hotpink
                case 0x9807620d: namedColor = Color.IndianRed; return true; // indianred
                case 0x4d77512b: namedColor = Color.Indigo; return true; // indigo
                case 0x9dea06b6: namedColor = Color.Ivory; return true; // ivory
                case 0x719339b9: namedColor = Color.Khaki; return true; // khaki
                case 0xb928b3ee: namedColor = Color.Lavender; return true; // lavender
                case 0xd591a34c: namedColor = Color.LavenderBlush; return true; // lavenderblush
                case 0x8bb2ab96: namedColor = Color.LawnGreen; return true; // lawngreen
                case 0xa9ddfd93: namedColor = Color.LemonChiffon; return true; // lemonchiffon
                case 0xc370be3b: namedColor = Color.LightBlue; return true; // lightblue
                case 0x4efa960c: namedColor = Color.LightCoral; return true; // lightcoral
                case 0xf060b994: namedColor = Color.LightCyan; return true; // lightcyan
                case 0xb2f7293f: namedColor = Color.LightGoldenrodYellow; return true; // lightgoldenrodyellow
                case 0x2bbe58fc: namedColor = Color.LightGray; return true; // lightgray
                case 0x9c3f19c6: namedColor = Color.LightGreen; return true; // lightgreen
                case 0x699ce1b7: namedColor = Color.LightPink; return true; // lightpink
                case 0x714b1745: namedColor = Color.LightSalmon; return true; // lightsalmon
                case 0xe243e671: namedColor = Color.LightSeaGreen; return true; // lightseagreen
                case 0x09357b30: namedColor = Color.LightSkyBlue; return true; // lightskyblue
                case 0x43bdda67: namedColor = Color.LightSlateGray; return true; // lightslategray
                case 0x8989e5fe: namedColor = Color.LightSteelBlue; return true; // lightsteelblue
                case 0xa124f36b: namedColor = Color.LightYellow; return true; // lightyellow
                case 0x07e34bbc: namedColor = Color.Lime; return true; // lime
                case 0x5f247ea7: namedColor = Color.LimeGreen; return true; // limegreen
                case 0xd6e414eb: namedColor = Color.Linen; return true; // linen
                case 0x63e629e8: namedColor = Color.Magenta; return true; // magenta
                case 0x6e32ccf5: namedColor = Color.Maroon; return true; // maroon
                case 0x22d8ff1c: namedColor = Color.MediumAquamarine; return true; // mediumaquamarine
                case 0xfdec2a8e: namedColor = Color.MediumBlue; return true; // mediumblue
                case 0x1a0e44ad: namedColor = Color.MediumOrchid; return true; // mediumorchid
                case 0xa9362f24: namedColor = Color.MediumPurple; return true; // mediumpurple
                case 0x0ba64a14: namedColor = Color.MediumSeaGreen; return true; // mediumseagreen
                case 0x772890e7: namedColor = Color.MediumSlateBlue; return true; // mediumslateblue
                case 0x71780822: namedColor = Color.MediumSpringGreen; return true; // mediumspringgreen
                case 0xcbdc3f67: namedColor = Color.MediumTurquoise; return true; // mediumturquoise
                case 0x1bdaa4b0: namedColor = Color.MediumVioletRed; return true; // mediumvioletred
                case 0xcc8e0751: namedColor = Color.MidnightBlue; return true; // midnightblue
                case 0x0920e031: namedColor = Color.MintCream; return true; // mintcream
                case 0xe1858adc: namedColor = Color.MistyRose; return true; // mistyrose
                case 0x45658504: namedColor = Color.Moccasin; return true; // moccasin
                case 0x4c7a1b8f: namedColor = Color.NavajoWhite; return true; // navajowhite
                case 0xa0fff1d1: namedColor = Color.Navy; return true; // navy
                case 0x9b55348d: namedColor = Color.OldLace; return true; // oldlace
                case 0x835cd3cc: namedColor = Color.Olive; return true; // olive
                case 0xb351bdbd: namedColor = Color.OliveDrab; return true; // olivedrab
                case 0x45b473eb: namedColor = Color.Orange; return true; // orange
                case 0x97e0985a: namedColor = Color.OrangeRed; return true; // orangered
                case 0xace6ba3a: namedColor = Color.Orchid; return true; // orchid
                case 0x6021b659: namedColor = Color.PaleGoldenrod; return true; // palegoldenrod
                case 0x8e1fedcc: namedColor = Color.PaleGreen; return true; // palegreen
                case 0xcc91efca: namedColor = Color.PaleTurquoise; return true; // paleturquoise
                case 0x907d3a11: namedColor = Color.PaleVioletRed; return true; // palevioletred
                case 0x8c361729: namedColor = Color.PapayaWhip; return true; // papayawhip
                case 0xe92d961d: namedColor = Color.PeachPuff; return true; // peachpuff
                case 0xb8f04003: namedColor = Color.Peru; return true; // peru
                case 0x225e036d: namedColor = Color.Pink; return true; // pink
                case 0xb6adb06b: namedColor = Color.Plum; return true; // plum
                case 0x6fdd84bc: namedColor = Color.PowderBlue; return true; // powderblue
                case 0x9a6e02ff: namedColor = Color.Purple; return true; // purple
                case 0x40f480dc: namedColor = Color.Red; return true; // red
                case 0xf8d1f5a6: namedColor = Color.RosyBrown; return true; // rosybrown
                case 0xbfbdf46c: namedColor = Color.RoyalBlue; return true; // royalblue
                case 0xe800849a: namedColor = Color.SaddleBrown; return true; // saddlebrown
                case 0x39b3175f: namedColor = Color.Salmon; return true; // salmon
                case 0x7b75476a: namedColor = Color.SandyBrown; return true; // sandybrown
                case 0xad8825b7: namedColor = Color.SeaGreen; return true; // seagreen
                case 0x46aadbde: namedColor = Color.SeaShell; return true; // seashell
                case 0x3227246b: namedColor = Color.Sienna; return true; // sienna
                case 0xb554f920: namedColor = Color.Silver; return true; // silver
                case 0x1c96ce4e: namedColor = Color.SkyBlue; return true; // skyblue
                case 0x93e80076: namedColor = Color.SlateBlue; return true; // slateblue
                case 0x15d8c7d5: namedColor = Color.SlateGray; return true; // slategray
                case 0x848317f2: namedColor = Color.Snow; return true; // snow
                case 0xf79b7417: namedColor = Color.SpringGreen; return true; // springgreen
                case 0x54408ac8: namedColor = Color.SteelBlue; return true; // steelblue
                case 0x9cf73498: namedColor = Color.Tan; return true; // tan
                case 0xa3fd7e9f: namedColor = Color.Teal; return true; // teal
                case 0x08a71a94: namedColor = Color.Thistle; return true; // thistle
                case 0x35a2e5c3: namedColor = Color.Tomato; return true; // tomato
                case 0xafe34731: namedColor = Color.Transparent; return true; // transparent
                case 0x7538805a: namedColor = Color.Turquoise; return true; // turquoise
                case 0x7b6d57f2: namedColor = Color.Violet; return true; // violet
                case 0x043cb490: namedColor = Color.Wheat; return true; // wheat
                case 0xde020766: namedColor = Color.White; return true; // white
                case 0xaa3e3a1f: namedColor = Color.WhiteSmoke; return true; // whitesmoke
                case 0x05bf6449: namedColor = Color.Yellow; return true; // yellow
                case 0xb6b77908: namedColor = Color.YellowGreen; return true; // yellowgreen
            }
            namedColor = default;
            return false;
        }

        public static readonly Dictionary<uint, Func<Color>> ArgbToNamedColorDictionary = new Dictionary<uint, Func<Color>>()
        {
            [0x00FFFFFF] = () => Color.Transparent,
            [0xFFF0F8FF] = () => Color.AliceBlue,
            [0xFFFAEBD7] = () => Color.AntiqueWhite,
            [0xFF00FFFF] = () => Color.Aqua,
            [0xFF7FFFD4] = () => Color.Aquamarine,
            [0xFFF0FFFF] = () => Color.Azure,
            [0xFFF5F5DC] = () => Color.Beige,
            [0xFFFFE4C4] = () => Color.Bisque,
            [0xFF000000] = () => Color.Black,
            [0xFFFFEBCD] = () => Color.BlanchedAlmond,
            [0xFF0000FF] = () => Color.Blue,
            [0xFF8A2BE2] = () => Color.BlueViolet,
            [0xFFA52A2A] = () => Color.Brown,
            [0xFFDEB887] = () => Color.BurlyWood,
            [0xFF5F9EA0] = () => Color.CadetBlue,
            [0xFF7FFF00] = () => Color.Chartreuse,
            [0xFFD2691E] = () => Color.Chocolate,
            [0xFFFF7F50] = () => Color.Coral,
            [0xFF6495ED] = () => Color.CornflowerBlue,
            [0xFFFFF8DC] = () => Color.Cornsilk,
            [0xFFDC143C] = () => Color.Crimson,
            [0xFF00FFFF] = () => Color.Cyan,
            [0xFF00008B] = () => Color.DarkBlue,
            [0xFF008B8B] = () => Color.DarkCyan,
            [0xFFB8860B] = () => Color.DarkGoldenrod,
            [0xFFA9A9A9] = () => Color.DarkGray,
            [0xFF006400] = () => Color.DarkGreen,
            [0xFFBDB76B] = () => Color.DarkKhaki,
            [0xFF8B008B] = () => Color.DarkMagenta,
            [0xFF556B2F] = () => Color.DarkOliveGreen,
            [0xFFFF8C00] = () => Color.DarkOrange,
            [0xFF9932CC] = () => Color.DarkOrchid,
            [0xFF8B0000] = () => Color.DarkRed,
            [0xFFE9967A] = () => Color.DarkSalmon,
            [0xFF8FBC8B] = () => Color.DarkSeaGreen,
            [0xFF483D8B] = () => Color.DarkSlateBlue,
            [0xFF2F4F4F] = () => Color.DarkSlateGray,
            [0xFF00CED1] = () => Color.DarkTurquoise,
            [0xFF9400D3] = () => Color.DarkViolet,
            [0xFFFF1493] = () => Color.DeepPink,
            [0xFF00BFFF] = () => Color.DeepSkyBlue,
            [0xFF696969] = () => Color.DimGray,
            [0xFF1E90FF] = () => Color.DodgerBlue,
            [0xFFB22222] = () => Color.Firebrick,
            [0xFFFFFAF0] = () => Color.FloralWhite,
            [0xFF228B22] = () => Color.ForestGreen,
            [0xFFFF00FF] = () => Color.Fuchsia,
            [0xFFDCDCDC] = () => Color.Gainsboro,
            [0xFFF8F8FF] = () => Color.GhostWhite,
            [0xFFFFD700] = () => Color.Gold,
            [0xFFDAA520] = () => Color.Goldenrod,
            [0xFF808080] = () => Color.Gray,
            [0xFF008000] = () => Color.Green,
            [0xFFADFF2F] = () => Color.GreenYellow,
            [0xFFF0FFF0] = () => Color.Honeydew,
            [0xFFFF69B4] = () => Color.HotPink,
            [0xFFCD5C5C] = () => Color.IndianRed,
            [0xFF4B0082] = () => Color.Indigo,
            [0xFFFFFFF0] = () => Color.Ivory,
            [0xFFF0E68C] = () => Color.Khaki,
            [0xFFE6E6FA] = () => Color.Lavender,
            [0xFFFFF0F5] = () => Color.LavenderBlush,
            [0xFF7CFC00] = () => Color.LawnGreen,
            [0xFFFFFACD] = () => Color.LemonChiffon,
            [0xFFADD8E6] = () => Color.LightBlue,
            [0xFFF08080] = () => Color.LightCoral,
            [0xFFE0FFFF] = () => Color.LightCyan,
            [0xFFFAFAD2] = () => Color.LightGoldenrodYellow,
            [0xFF90EE90] = () => Color.LightGreen,
            [0xFFD3D3D3] = () => Color.LightGray,
            [0xFFFFB6C1] = () => Color.LightPink,
            [0xFFFFA07A] = () => Color.LightSalmon,
            [0xFF20B2AA] = () => Color.LightSeaGreen,
            [0xFF87CEFA] = () => Color.LightSkyBlue,
            [0xFF778899] = () => Color.LightSlateGray,
            [0xFFB0C4DE] = () => Color.LightSteelBlue,
            [0xFFFFFFE0] = () => Color.LightYellow,
            [0xFF00FF00] = () => Color.Lime,
            [0xFF32CD32] = () => Color.LimeGreen,
            [0xFFFAF0E6] = () => Color.Linen,
            [0xFFFF00FF] = () => Color.Magenta,
            [0xFF800000] = () => Color.Maroon,
            [0xFF66CDAA] = () => Color.MediumAquamarine,
            [0xFF0000CD] = () => Color.MediumBlue,
            [0xFFBA55D3] = () => Color.MediumOrchid,
            [0xFF9370DB] = () => Color.MediumPurple,
            [0xFF3CB371] = () => Color.MediumSeaGreen,
            [0xFF7B68EE] = () => Color.MediumSlateBlue,
            [0xFF00FA9A] = () => Color.MediumSpringGreen,
            [0xFF48D1CC] = () => Color.MediumTurquoise,
            [0xFFC71585] = () => Color.MediumVioletRed,
            [0xFF191970] = () => Color.MidnightBlue,
            [0xFFF5FFFA] = () => Color.MintCream,
            [0xFFFFE4E1] = () => Color.MistyRose,
            [0xFFFFE4B5] = () => Color.Moccasin,
            [0xFFFFDEAD] = () => Color.NavajoWhite,
            [0xFF000080] = () => Color.Navy,
            [0xFFFDF5E6] = () => Color.OldLace,
            [0xFF808000] = () => Color.Olive,
            [0xFF6B8E23] = () => Color.OliveDrab,
            [0xFFFFA500] = () => Color.Orange,
            [0xFFFF4500] = () => Color.OrangeRed,
            [0xFFDA70D6] = () => Color.Orchid,
            [0xFFEEE8AA] = () => Color.PaleGoldenrod,
            [0xFF98FB98] = () => Color.PaleGreen,
            [0xFFAFEEEE] = () => Color.PaleTurquoise,
            [0xFFDB7093] = () => Color.PaleVioletRed,
            [0xFFFFEFD5] = () => Color.PapayaWhip,
            [0xFFFFDAB9] = () => Color.PeachPuff,
            [0xFFCD853F] = () => Color.Peru,
            [0xFFFFC0CB] = () => Color.Pink,
            [0xFFDDA0DD] = () => Color.Plum,
            [0xFFB0E0E6] = () => Color.PowderBlue,
            [0xFF800080] = () => Color.Purple,
            [0xFFFF0000] = () => Color.Red,
            [0xFFBC8F8F] = () => Color.RosyBrown,
            [0xFF4169E1] = () => Color.RoyalBlue,
            [0xFF8B4513] = () => Color.SaddleBrown,
            [0xFFFA8072] = () => Color.Salmon,
            [0xFFF4A460] = () => Color.SandyBrown,
            [0xFF2E8B57] = () => Color.SeaGreen,
            [0xFFFFF5EE] = () => Color.SeaShell,
            [0xFFA0522D] = () => Color.Sienna,
            [0xFFC0C0C0] = () => Color.Silver,
            [0xFF87CEEB] = () => Color.SkyBlue,
            [0xFF6A5ACD] = () => Color.SlateBlue,
            [0xFF708090] = () => Color.SlateGray,
            [0xFFFFFAFA] = () => Color.Snow,
            [0xFF00FF7F] = () => Color.SpringGreen,
            [0xFF4682B4] = () => Color.SteelBlue,
            [0xFFD2B48C] = () => Color.Tan,
            [0xFF008080] = () => Color.Teal,
            [0xFFD8BFD8] = () => Color.Thistle,
            [0xFFFF6347] = () => Color.Tomato,
            [0xFF40E0D0] = () => Color.Turquoise,
            [0xFFEE82EE] = () => Color.Violet,
            [0xFFF5DEB3] = () => Color.Wheat,
            [0xFFFFFFFF] = () => Color.White,
            [0xFFF5F5F5] = () => Color.WhiteSmoke,
            [0xFFFFFF00] = () => Color.Yellow,
            [0xFF9ACD32] = () => Color.YellowGreen,
        };

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

        public object Parse_OLD(ITypeDescriptorContext context, CultureInfo culture, string colour)
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
                        colorpart = Color.FromArgb(
                            alphaValue,
                            (int) Math.Round(255 * float.Parse(values[0].Trim().TrimEnd('%'), NumberStyles.Any, CultureInfo.InvariantCulture) / 100f),
                            (int) Math.Round(255 * float.Parse(values[1].Trim().TrimEnd('%'), NumberStyles.Any, CultureInfo.InvariantCulture) / 100f),
                            (int) Math.Round(255 * float.Parse(values[2].Trim().TrimEnd('%'), NumberStyles.Any, CultureInfo.InvariantCulture) / 100f));
                    }
                    else
                    {
                        colorpart = Color.FromArgb(
                            alphaValue,
                            int.Parse(values[0], CultureInfo.InvariantCulture),
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
            if (TryToGetSystemColor_OLD(colour, out var systemColor))
            {
                return systemColor;
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

        public static bool TryToGetSystemColor_OLD(string colour, out Color systemColor)
        {
            switch (colour.ToLowerInvariant())
            {
                case "activeborder":
                {
                    systemColor = SystemColors.ActiveBorder;
                    return true;
                }
                case "activecaption":
                {
                    systemColor = SystemColors.ActiveCaption;
                    return true;
                }
                case "appworkspace":
                {
                    systemColor = SystemColors.AppWorkspace;
                    return true;
                }
                case "background":
                {
                    systemColor = SystemColors.Desktop;
                    return true;
                }
                case "buttonface":
                {
                    systemColor = SystemColors.Control;
                    return true;
                }
                case "buttonhighlight":
                {
                    systemColor = SystemColors.ControlLightLight;
                    return true;
                }
                case "buttonshadow":
                {
                    systemColor = SystemColors.ControlDark;
                    return true;
                }
                case "buttontext":
                {
                    systemColor = SystemColors.ControlText;
                    return true;
                }
                case "captiontext":
                {
                    systemColor = SystemColors.ActiveCaptionText;
                    return true;
                }
                case "graytext":
                {
                    systemColor = SystemColors.GrayText;
                    return true;
                }
                case "highlight":
                {
                    systemColor = SystemColors.Highlight;
                    return true;
                }
                case "highlighttext":
                {
                    systemColor = SystemColors.HighlightText;
                    return true;
                }
                case "inactiveborder":
                {
                    systemColor = SystemColors.InactiveBorder;
                    return true;
                }
                case "inactivecaption":
                {
                    systemColor = SystemColors.InactiveCaption;
                    return true;
                }
                case "inactivecaptiontext":
                {
                    systemColor = SystemColors.InactiveCaptionText;
                    return true;
                }
                case "infobackground":
                {
                    systemColor = SystemColors.Info;
                    return true;
                }
                case "infotext":
                {
                    systemColor = SystemColors.InfoText;
                    return true;
                }
                case "menu":
                {
                    systemColor = SystemColors.Menu;
                    return true;
                }
                case "menutext":
                {
                    systemColor = SystemColors.MenuText;
                    return true;
                }
                case "scrollbar":
                {
                    systemColor = SystemColors.ScrollBar;
                    return true;
                }
                case "threeddarkshadow":
                {
                    systemColor = SystemColors.ControlDarkDark;
                    return true;
                }
                case "threedface":
                {
                    systemColor = SystemColors.Control;
                    return true;
                }
                case "threedhighlight":
                {
                    systemColor = SystemColors.ControlLight;
                    return true;
                }
                case "threedlightshadow":
                {
                    systemColor = SystemColors.ControlLightLight;
                    return true;
                }
                case "window":
                {
                    systemColor = SystemColors.Window;
                    return true;
                }
                case "windowframe":
                {
                    systemColor = SystemColors.WindowFrame;
                    return true;
                }
                case "windowtext":
                {
                    systemColor = SystemColors.WindowText;
                    return true;
                }
            }

            systemColor = default;
            return false;
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

        /// <summary>
        /// Converts HSL color (with HSL specified from 0 to 1) to RGB color.
        /// Taken from https://geekymonkey.com/Programming/CSharp/RGB2HSL_HSL2RGB.htm
        /// </summary>
        /// <param name="h">The hue value.</param>
        /// <param name="sl">The saturation value.</param>
        /// <param name="l">The lightness value.</param>
        /// <param name="red">The red value.</param>
        /// <param name="green">The green value.</param>
        /// <param name="blue">The blue value.</param>
        private static void Hsl2Rgb(double h, double sl, double l, out int red, out int green, out int blue)
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

            red = (int)Math.Round(r * 255.0);
            green = (int) Math.Round(g * 255.0);
            blue = (int) Math.Round(b * 255.0);
        }
    }
}
