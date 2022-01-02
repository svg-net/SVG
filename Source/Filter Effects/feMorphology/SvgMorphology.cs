namespace Svg.FilterEffects
{
    [SvgElement("feMorphology")]
    public partial class SvgMorphology : SvgFilterPrimitive
    {
        [SvgAttribute("operator")]
        public SvgMorphologyOperator Operator
        {
            get { return GetAttribute("operator", false, SvgMorphologyOperator.Erode); }
            set { Attributes["operator"] = value; }
        }

        [SvgAttribute("radius")]
        public SvgNumberCollection Radius
        {
            get { return GetAttribute("radius", false, new SvgNumberCollection() { 0f, 0f }); }
            set { Attributes["radius"] = value; }
        }

#if !NO_SDC
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feMorphology filter Process().
            buffer[Result] = buffer[Input];
        }
#endif

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgMorphology>();
        }
    }
}
