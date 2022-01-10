namespace Svg.FilterEffects
{
    [SvgElement("feBlend")]
    public partial class SvgBlend : SvgFilterPrimitive
    {
        [SvgAttribute("mode")]
        public SvgBlendMode Mode
        {
            get { return GetAttribute("mode", false, SvgBlendMode.Normal); }
            set { Attributes["mode"] = value; }
        }

        [SvgAttribute("in2")]
        public string Input2
        {
            get { return GetAttribute<string>("in2", false); }
            set { Attributes["in2"] = value; }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgBlend>();
        }
    }
}
