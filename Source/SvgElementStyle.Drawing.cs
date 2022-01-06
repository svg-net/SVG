#if !NO_SDC
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Svg
{
    public partial class SvgElement
    {
        /// <summary>
        /// Get the font information based on data stored with the text object or inherited from the parent.
        /// </summary>
        /// <returns></returns>
        internal IFontDefn GetFont(ISvgRenderer renderer, SvgFontManager fontManager)
        {
            // Get the font-size
            float fontSize;
            var fontSizeUnit = this.FontSize;
            if (fontSizeUnit == SvgUnit.None || fontSizeUnit == SvgUnit.Empty)
            {
                fontSize = new SvgUnit(SvgUnitType.Em, 1.0f);
            }
            else
            {
                fontSize = fontSizeUnit.ToDeviceValue(renderer, UnitRenderingType.Vertical, this);
            }

            var family = ValidateFontFamily(this.FontFamily, this.OwnerDocument, fontManager ?? this.OwnerDocument.FontManager);
            var sFaces = family as IEnumerable<SvgFontFace>;
            var ppi = this.OwnerDocument?.Ppi ?? SvgDocument.PointsPerInch;

            if (sFaces == null)
            {
                var fontStyle = System.Drawing.FontStyle.Regular;

                // Get the font-weight
                switch (this.FontWeight)
                {
                    case SvgFontWeight.Bold:
                    case SvgFontWeight.W600:
                    case SvgFontWeight.W700:
                    case SvgFontWeight.W800:
                    case SvgFontWeight.W900:
                        fontStyle |= System.Drawing.FontStyle.Bold;
                        break;
                    case SvgFontWeight.Bolder:
                        switch (Parent?.FontWeight ?? SvgFontWeight.Normal)
                        {
                            case SvgFontWeight.W100:
                            case SvgFontWeight.W200:
                            case SvgFontWeight.W300:
                                break;
                            default:
                                fontStyle |= System.Drawing.FontStyle.Bold;
                                break;
                        }
                        break;
                    case SvgFontWeight.Lighter:
                        switch (Parent?.FontWeight ?? SvgFontWeight.Normal)
                        {
                            case SvgFontWeight.W800:
                            case SvgFontWeight.W900:
                                fontStyle |= System.Drawing.FontStyle.Bold;
                                break;
                        }
                        break;
                }

                // Get the font-style
                switch (this.FontStyle)
                {
                    case SvgFontStyle.Italic:
                    case SvgFontStyle.Oblique:
                        fontStyle |= System.Drawing.FontStyle.Italic;
                        break;
                }

                // Get the text-decoration
                switch (this.TextDecoration)
                {
                    case SvgTextDecoration.LineThrough:
                        fontStyle |= System.Drawing.FontStyle.Strikeout;
                        break;
                    case SvgTextDecoration.Underline:
                        fontStyle |= System.Drawing.FontStyle.Underline;
                        break;
                }

                var ff = family as FontFamily;
                if (!ff.IsStyleAvailable(fontStyle))
                {
                    // Do Something
                }

                // Get the font-family
                return new GdiFontDefn(new Font(ff, fontSize, fontStyle, GraphicsUnit.Pixel), ppi);
            }
            else
            {
                var font = sFaces.First().Parent as SvgFont;
                if (font == null)
                {
                    var uri = sFaces.First().Descendants().OfType<SvgFontFaceUri>().First().ReferencedElement;
                    font = OwnerDocument.IdManager.GetElementById(uri) as SvgFont;
                }
                return new SvgFontDefn(font, fontSize, ppi);
            }
        }

        public static object ValidateFontFamily(string fontFamilyList, SvgDocument doc, SvgFontManager fontManager)
        {
            // Split font family list on "," and then trim start and end spaces and quotes.
            var fontParts = (fontFamilyList ?? string.Empty).Split(new[] { ',' }).Select(fontName => fontName.Trim(new[] { '"', ' ', '\'' }));

            // Find a the first font that exists in the list of installed font families.
            // styles from IE get sent through as lowercase.
            foreach (var f in fontParts)
            {
                IEnumerable<SvgFontFace> fontFaces;
                if (doc != null && doc.FontDefns().TryGetValue(f, out fontFaces))
                    return fontFaces;

                var family = fontManager.FindFont(f);
                if (family != null)
                    return family;
            }

            // No valid font family found from the list requested.
            return System.Drawing.FontFamily.GenericSansSerif;
        }
    }
}
#endif
