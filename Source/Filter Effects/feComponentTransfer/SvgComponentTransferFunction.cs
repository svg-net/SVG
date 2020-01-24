namespace Svg.FilterEffects
{
    public abstract class SvgComponentTransferFunction : SvgElement
    {
        [SvgAttribute("type")]
        public SvgComponentTransferType Type
        {
            get { return GetAttribute("type", false, SvgComponentTransferType.Identity); }
            set { Attributes["type"] = value; }
        }

        [SvgAttribute("tableValues")]
        public SvgNumberCollection TableValues
        {
            get { return GetAttribute("tableValues", false, new SvgNumberCollection()); }
            set { Attributes["tableValues"] = value; }
        }

        [SvgAttribute("slope")]
        public float Slope
        {
            get { return GetAttribute("slope", false, 1f); }
            set { Attributes["slope"] = value; }
        }

        [SvgAttribute("intercept")]
        public float Intercept
        {
            get { return GetAttribute("intercept", false, 0f); }
            set { Attributes["intercept"] = value; }
        }

        [SvgAttribute("amplitude")]
        public float Amplitude
        {
            get { return GetAttribute("amplitude", false, 1f); }
            set { Attributes["amplitude"] = value; }
        }

        [SvgAttribute("exponent")]
        public float Exponent
        {
            get { return GetAttribute("exponent", false, 1f); }
            set { Attributes["exponent"] = value; }
        }

        [SvgAttribute("offset")]
        public float Offset
        {
            get { return GetAttribute("offset", false, 0f); }
            set { Attributes["offset"] = value; }
        }
    }
}
