using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg
{
    [SvgElement("font-face")]
    public class SvgFontFace : SvgElement
    {
        [SvgAttribute("alphabetic")]
        public float Alphabetic
        {
            get { return GetAttribute("alphabetic", Inherited, 0f); }
            set { Attributes["alphabetic"] = value; }
        }

        [SvgAttribute("ascent")]
        public float Ascent
        {
            get { return GetAttribute("ascent", Inherited, Parent is SvgFont ? UnitsPerEm - ((SvgFont)Parent).VertOriginY : 0f); }
            set { Attributes["ascent"] = value; }
        }

        [SvgAttribute("ascent-height")]
        public float AscentHeight
        {
            get { return GetAttribute("ascent-height", Inherited, Ascent); }
            set { Attributes["ascent-height"] = value; }
        }

        [SvgAttribute("descent")]
        public float Descent
        {
            get { return GetAttribute("descent", Inherited, Parent is SvgFont ? ((SvgFont)Parent).VertOriginY : 0f); }
            set { Attributes["descent"] = value; }
        }

        /// <summary>
        /// Indicates which font family is to be used to render the text.
        /// </summary>
        [SvgAttribute("font-family")]
        public override string FontFamily
        {
            get { return GetAttribute<string>("font-family", Inherited); }
            set { Attributes["font-family"] = value; }
        }

        /// <summary>
        /// Refers to the size of the font from baseline to baseline when multiple lines of text are set solid in a multiline layout environment.
        /// </summary>
        [SvgAttribute("font-size")]
        public override SvgUnit FontSize
        {
            get { return GetAttribute("font-size", Inherited, SvgUnit.Empty); }
            set { Attributes["font-size"] = value; }
        }

        /// <summary>
        /// Refers to the style of the font.
        /// </summary>
        [SvgAttribute("font-style")]
        public override SvgFontStyle FontStyle
        {
            get { return GetAttribute("font-style", Inherited, SvgFontStyle.All); }
            set { Attributes["font-style"] = value; }
        }

        /// <summary>
        /// Refers to the varient of the font.
        /// </summary>
        [SvgAttribute("font-variant")]
        public override SvgFontVariant FontVariant
        {
            get { return GetAttribute("font-variant", Inherited, SvgFontVariant.Inherit); }
            set { Attributes["font-variant"] = value; }
        }

        /// <summary>
        /// Refers to the boldness of the font.
        /// </summary>
        [SvgAttribute("font-weight")]
        public override SvgFontWeight FontWeight
        {
            get { return GetAttribute("font-weight", Inherited, SvgFontWeight.Inherit); }
            set { Attributes["font-weight"] = value; }
        }

        [SvgAttribute("panose-1")]
        public string Panose1
        {
            get { return GetAttribute<string>("panose-1", Inherited); }
            set { Attributes["panose-1"] = value; }
        }

        [SvgAttribute("units-per-em")]
        public float UnitsPerEm
        {
            get { return GetAttribute("units-per-em", Inherited, 1000f); }
            set { Attributes["units-per-em"] = value; }
        }

        [SvgAttribute("x-height")]
        public float XHeight
        {
            get { return GetAttribute("x-height", Inherited, float.MinValue); }
            set { Attributes["x-height"] = value; }
        }

        public override SvgElement DeepCopy()
        {
            return base.DeepCopy<SvgFontFace>();
        }
    }
}
