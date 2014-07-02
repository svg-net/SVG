using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Svg.DataTypes;
using System.Text.RegularExpressions;

namespace Svg
{
    internal class FontData
    {
        private SvgUnit _fontSize;
        private SvgFontWeight _fontWeight = SvgFontWeight.inherit;
        private string _font;
        private string _fontFamily;

        private const string DefaultFontFamily = "Times New Roman";

        public FontData()
        {
            this._fontSize = new SvgUnit(0.0f);
        }

        /// <summary>
        /// Indicates which font family is to be used to render the text.
        /// </summary>
        public virtual string FontFamily
        {
            get { return this._fontFamily ?? DefaultFontFamily; }
            set
            {
                this._fontFamily = ValidateFontFamily(value);
            }
        }

        /// <summary>
        /// Refers to the size of the font from baseline to baseline when multiple lines of text are set solid in a multiline layout environment.
        /// </summary>
        public virtual SvgUnit FontSize
        {
            get { return this._fontSize; }
            set { this._fontSize = value; }
        }

        /// <summary>
        /// Refers to the boldness of the font.
        /// </summary>
        public virtual SvgFontWeight FontWeight
        {
            get { return this._fontWeight; }
            set { this._fontWeight = value; }
        }

        /// <summary>
        /// Set all font information.
        /// </summary>
        public string Font
        {
            get { return this._font; }
            set
            {
                var parts = value.Split(',');
                foreach (var part in parts)
                {
                    //This deals with setting font size. Looks for either <number>px or <number>pt style="font: bold 16px/normal 'trebuchet ms', verdana, sans-serif;"
                    Regex rx = new Regex(@"(\d+)+(?=pt|px)");
                    var res = rx.Match(part);
                    if (res.Success)
                    {
                        int fontSize = 10;
                        int.TryParse(res.Value, out fontSize);
                        this.FontSize = new SvgUnit((float)fontSize);
                    }

                    //this assumes "bold" has spaces around it. e.g.: style="font: bold 16px/normal 
                    rx = new Regex(@"\sbold\s");
                    res = rx.Match(part);
                    if (res.Success)
                    {
                        this.FontWeight = SvgFontWeight.bold;
                    }
                }
                var font = ValidateFontFamily(value);
                this._fontFamily = font;
                this._font = font; //not sure this is used?
            }
        }

        public Font GetFont(ISvgStylable owner, FontData inherit)
        {
            float fontSize = this.FontSize.ToDeviceValue(owner);
            if (fontSize == 0.0f)
            {
                fontSize = (inherit == null ? 1.0f : inherit.FontSize.ToDeviceValue(owner));
                fontSize = (fontSize == 0.0f ? 1.0f : fontSize);
            }
            var baseWeight = (_fontWeight == SvgFontWeight.inherit && inherit != null ? inherit.FontWeight : _fontWeight);
            var fontWeight = (baseWeight == SvgFontWeight.bold ? FontStyle.Bold : FontStyle.Regular);
            var family = _fontFamily ?? (inherit == null ? DefaultFontFamily : inherit.FontFamily);
            return new Font(family, fontSize, fontWeight, GraphicsUnit.Pixel);
        }

        private static string ValidateFontFamily(string fontFamilyList)
        {
            // Split font family list on "," and then trim start and end spaces and quotes.
            var fontParts = fontFamilyList.Split(new[] { ',' }).Select(fontName => fontName.Trim(new[] { '"', ' ', '\'' }));

            var families = System.Drawing.FontFamily.Families;

            // Find a the first font that exists in the list of installed font families.
            //styles from IE get sent through as lowercase.
            foreach (var f in fontParts.Where(f => families.Any(family => family.Name.ToLower() == f.ToLower())))
            {
                return f;
            }
            // No valid font family found from the list requested.
            return null;
        }

        public override bool Equals(object obj)
        {
            var thisType = obj as FontData;
            if (thisType != null) return Equals(thisType);
            return base.Equals(obj);
        }
        public bool Equals(FontData obj)
        {
            return obj._fontFamily == this._fontFamily && obj._fontSize == this._fontSize && obj._fontWeight == this._fontWeight;
        }
        public override int GetHashCode()
        {
            return (this._fontFamily == null ? 0 : this._fontFamily.GetHashCode()) ^
                   (this._fontSize == null ? 0 : this._fontSize.GetHashCode()) ^
                   this._fontWeight.GetHashCode();
        }
    }
}
