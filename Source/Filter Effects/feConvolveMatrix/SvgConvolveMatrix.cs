namespace Svg.FilterEffects
{
    [SvgElement("feConvolveMatrix")]
    public class SvgConvolveMatrix : SvgFilterPrimitive
    {
        [SvgAttribute("order")]
        public SvgNumberCollection Order
        {
            get { return GetAttribute("order", false, new SvgNumberCollection() { 3f, 3f }); }
            set { Attributes["order"] = value; }
        }

        [SvgAttribute("kernelMatrix")]
        public SvgNumberCollection KernelMatrix
        {
            get { return GetAttribute<SvgNumberCollection>("kernelMatrix", false); }
            set { Attributes["kernelMatrix"] = value; }
        }

        [SvgAttribute("divisor")]
        public float Divisor
        {
            get { return GetAttribute<float>("divisor", false); }
            set { Attributes["divisor"] = value; }
        }

        [SvgAttribute("bias")]
        public float Bias
        {
            get { return GetAttribute("bias", false, 0f); }
            set { Attributes["bias"] = value; }
        }

        [SvgAttribute("targetX")]
        public int TargetX
        {
            get { return GetAttribute<int>("targetX", false); }
            set { Attributes["targetX"] = value; }
        }

        [SvgAttribute("targetY")]
        public int TargetY
        {
            get { return GetAttribute<int>("targetY", false); }
            set { Attributes["targetY"] = value; }
        }

        [SvgAttribute("edgeMode")]
        public SvgEdgeMode EdgeMode
        {
            get { return GetAttribute("edgeMode", false, SvgEdgeMode.Duplicate); }
            set { Attributes["edgeMode"] = value; }
        }

        [SvgAttribute("kernelUnitLength")]
        public SvgNumberCollection KernelUnitLength
        {
            get { return GetAttribute("kernelUnitLength", false, new SvgNumberCollection() { 1f, 1f }); }
            set { Attributes["kernelUnitLength"] = value; }
        }

        [SvgAttribute("preserveAlpha")]
        public bool PreserveAlpha
        {
            get { return GetAttribute("preserveAlpha", false, defaultValue: false); }
            set { Attributes["preserveAlpha"] = value; }
        }

        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feConvolveMatrix filter Process().
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgConvolveMatrix>();
        }
    }
}
