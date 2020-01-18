namespace Svg.FilterEffects
{
    [SvgElement("feMorphology")]
    public class SvgMorphology : SvgFilterPrimitive
    {
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
