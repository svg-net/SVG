namespace Svg.FilterEffects
{
    [SvgElement("feMorphology")]
    public class SvgMorphology : SvgFilterPrimitive
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

        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feMorphology filter Process().
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgMorphology>();
        }
    }
}
