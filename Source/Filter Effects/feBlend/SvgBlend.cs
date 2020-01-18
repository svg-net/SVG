namespace Svg.FilterEffects
{
    [SvgElement("feBlend")]
    public class SvgBlend : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feBlend filter Process().
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgBlend>();
        }
    }
}
