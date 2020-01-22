namespace Svg.FilterEffects
{
    [SvgElement("fePointLight")]
    public class SvgPointLight : SvgElement
    {
        [SvgAttribute("x")]
        public float X
        {
            get { return GetAttribute("x", false, 0f); }
            set { Attributes["x"] = value; }
        }

        [SvgAttribute("y")]
        public float Y
        {
            get { return GetAttribute("y", false, 0f); }
            set { Attributes["y"] = value; }
        }

        [SvgAttribute("z")]
        public float Z
        {
            get { return GetAttribute("z", false, 0f); }
            set { Attributes["z"] = value; }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgPointLight>();
        }
    }
}
