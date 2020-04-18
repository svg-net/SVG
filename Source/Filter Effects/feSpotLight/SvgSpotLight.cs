namespace Svg.FilterEffects
{
    [SvgElement("feSpotLight")]
    public class SvgSpotLight : SvgElement
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

        [SvgAttribute("pointsAtX")]
        public float PointsAtX
        {
            get { return GetAttribute("pointsAtX", false, 0f); }
            set { Attributes["pointsAtX"] = value; }
        }

        [SvgAttribute("pointsAtY")]
        public float PointsAtY
        {
            get { return GetAttribute("pointsAtY", false, 0f); }
            set { Attributes["pointsAtY"] = value; }
        }

        [SvgAttribute("pointsAtZ")]
        public float PointsAtZ
        {
            get { return GetAttribute("pointsAtZ", false, 0f); }
            set { Attributes["pointsAtZ"] = value; }
        }

        [SvgAttribute("specularExponent")]
        public float SpecularExponent
        {
            get { return GetAttribute("specularExponent", false, 1f); }
            set { Attributes["specularExponent"] = value; }
        }

        [SvgAttribute("limitingConeAngle")]
        public float LimitingConeAngle
        {
            get { return GetAttribute("limitingConeAngle", false, float.NaN); }
            set { Attributes["limitingConeAngle"] = value; }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgSpotLight>();
        }
    }
}
