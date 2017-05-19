using System;
using System.Collections.Generic;
using System.Text;


namespace PdfSharp.Drawing
{


    public class XColorHelper
    {


        // StaticConvertFromString
        // https://github.com/mono/mono/blob/master/mcs/class/System.Drawing/System.Drawing/ColorConverter.cs
        public static PdfSharp.Drawing.XColor FromHtml(string htmlColor)
        {
            if (string.IsNullOrEmpty(htmlColor))
            {
                return PdfSharp.Drawing.XColor.FromArgb(System.Drawing.Color.Empty);
            }

            if (htmlColor.StartsWith("#"))
            {
                htmlColor = htmlColor.Substring(1);

                if (htmlColor.Length == 3)
                {
                    string R = htmlColor.Substring(0, 1);
                    string G = htmlColor.Substring(1, 1);
                    string B = htmlColor.Substring(2, 1);
                    htmlColor = System.Convert.ToString(System.Convert.ToString(System.Convert.ToString(System.Convert.ToString(R + R) + G) + G) + B) + B;
                }
                // (htmlColor.Length == 3)

                if (htmlColor.Length == 6)
                {
                    string sR = htmlColor.Substring(0, 2);
                    string sG = htmlColor.Substring(2, 2);
                    string sB = htmlColor.Substring(4, 2);

                    byte R = System.Convert.ToByte(sR, 16);
                    byte G = System.Convert.ToByte(sG, 16);
                    byte B = System.Convert.ToByte(sB, 16);

                    PdfSharp.Drawing.XColor xc = new PdfSharp.Drawing.XColor();
                    xc.R = R;
                    xc.G = G;
                    xc.B = B;
                    xc.A = 1.0;
                    return xc;
                }
                // (htmlColor.Length == 6)

                throw new ArgumentException("Invalid HTML color.");
            }
            // (htmlColor.StartsWith("#"))

            switch (htmlColor.ToLowerInvariant())
            {
                case "buttonface":
                case "threedface":
                    return PdfSharp.Drawing.XColor.FromArgb(System.Drawing.SystemColors.Control);
                case "buttonhighlight":
                case "threedlightshadow":
                    return PdfSharp.Drawing.XColor.FromArgb(System.Drawing.SystemColors.ControlLightLight);
                case "buttonshadow":
                     return PdfSharp.Drawing.XColor.FromArgb(System.Drawing.SystemColors.ControlDark);
                case "captiontext":
                     return PdfSharp.Drawing.XColor.FromArgb(System.Drawing.SystemColors.ActiveCaptionText);
                case "threeddarkshadow":
                     return PdfSharp.Drawing.XColor.FromArgb(System.Drawing.SystemColors.ControlDarkDark);
                case "threedhighlight":
                     return PdfSharp.Drawing.XColor.FromArgb(System.Drawing.SystemColors.ControlLight);
                case "background":
                     return PdfSharp.Drawing.XColor.FromArgb(System.Drawing.SystemColors.Desktop);
                case "buttontext":
                     return PdfSharp.Drawing.XColor.FromArgb(System.Drawing.SystemColors.ControlText);
                case "infobackground":
                     return PdfSharp.Drawing.XColor.FromArgb(System.Drawing.SystemColors.Info);
                // special case for Color.LightGray versus html's LightGrey (#340917)
                case "lightgrey":
                     return PdfSharp.Drawing.XColor.FromArgb(System.Drawing.Color.LightGray);
            } // End Switch (htmlColor.ToLowerInvariant())


            PdfSharp.Drawing.XKnownColor kk = default(PdfSharp.Drawing.XKnownColor);
            if (TryEnumParse<PdfSharp.Drawing.XKnownColor>(htmlColor, true, ref kk))
            {
                return PdfSharp.Drawing.XColor.FromKnownColor(kk);
            }

            return PdfSharp.Drawing.XColor.FromArgb(System.Drawing.Color.Empty);
        } // FromHtml


        // .NET 2.0 BackPort
        public static bool TryEnumParse<T>(string Value, bool CaseSens, ref T kk)
        {
            try
            {
                kk = (T)Enum.Parse(typeof(T), Value, true);
                return true;
            }
            catch
            {
            }

            kk = default(T);
            return false;
        } // TryEnumParse


    } // End Class XColorHelper


} // End Namespace Stammbaum