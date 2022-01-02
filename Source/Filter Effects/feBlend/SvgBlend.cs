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

#if !NO_SDC
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feBlend filter Process().
            buffer[Result] = buffer[Input];
        }
#endif

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgBlend>();
        }
    }
}
