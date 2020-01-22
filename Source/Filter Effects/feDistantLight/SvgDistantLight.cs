namespace Svg.FilterEffects
{
    [SvgElement("feDistantLight")]
    public class SvgDistantLight : SvgElement
    {
        [SvgAttribute("azimuth")]
        public float Azimuth
        {
            get { return GetAttribute("azimuth", false, 0f); }
            set { Attributes["azimuth"] = value; }
        }

        [SvgAttribute("elevation")]
        public float Elevation
        {
            get { return GetAttribute("elevation", false, 0f); }
            set { Attributes["elevation"] = value; }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgDistantLight>();
        }
    }
}
