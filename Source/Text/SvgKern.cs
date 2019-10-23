namespace Svg
{
    public abstract class SvgKern : SvgElement
    {
        [SvgAttribute("g1")]
        public string Glyph1
        {
            get { return GetAttribute<string>("g1", true); }
            set { Attributes["g1"] = value; }
        }

        [SvgAttribute("g2")]
        public string Glyph2
        {
            get { return GetAttribute<string>("g2", true); }
            set { Attributes["g2"] = value; }
        }

        [SvgAttribute("u1")]
        public string Unicode1
        {
            get { return GetAttribute<string>("u1", true); }
            set { Attributes["u1"] = value; }
        }

        [SvgAttribute("u2")]
        public string Unicode2
        {
            get { return GetAttribute<string>("u2", true); }
            set { Attributes["u2"] = value; }
        }

        [SvgAttribute("k")]
        public float Kerning
        {
            get { return GetAttribute("k", true, 0f); }
            set { Attributes["k"] = value; }
        }
    }

    [SvgElement("vkern")]
    public class SvgVerticalKern : SvgKern
    {
        public override SvgElement DeepCopy()
        {
            return base.DeepCopy<SvgVerticalKern>();
        }
    }
    [SvgElement("hkern")]
    public class SvgHorizontalKern : SvgKern
    {
        public override SvgElement DeepCopy()
        {
            return base.DeepCopy<SvgHorizontalKern>();
        }
    }
}
