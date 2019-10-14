namespace Svg
{
    [SvgElement("font-face")]
    public class SvgFontFace : SvgElement
    {
        [SvgAttribute("alphabetic")]
        public float Alphabetic
        {
            get { return GetAttribute("alphabetic", true, 0f); }
            set { Attributes["alphabetic"] = value; }
        }

        [SvgAttribute("ascent")]
        public float Ascent
        {
            get { return GetAttribute("ascent", true, Parent is SvgFont ? UnitsPerEm - ((SvgFont)Parent).VertOriginY : 0f); }
            set { Attributes["ascent"] = value; }
        }

        [SvgAttribute("ascent-height")]
        public float AscentHeight
        {
            get { return GetAttribute("ascent-height", true, Ascent); }
            set { Attributes["ascent-height"] = value; }
        }

        [SvgAttribute("descent")]
        public float Descent
        {
            get { return GetAttribute("descent", true, Parent is SvgFont ? ((SvgFont)Parent).VertOriginY : 0f); }
            set { Attributes["descent"] = value; }
        }

        [SvgAttribute("panose-1")]
        public string Panose1
        {
            get { return GetAttribute<string>("panose-1", true); }
            set { Attributes["panose-1"] = value; }
        }

        [SvgAttribute("units-per-em")]
        public float UnitsPerEm
        {
            get { return GetAttribute("units-per-em", true, 1000f); }
            set { Attributes["units-per-em"] = value; }
        }

        [SvgAttribute("x-height")]
        public float XHeight
        {
            get { return GetAttribute("x-height", true, float.MinValue); }
            set { Attributes["x-height"] = value; }
        }

        public override SvgElement DeepCopy()
        {
            return base.DeepCopy<SvgFontFace>();
        }
    }
}
