namespace Svg.FilterEffects
{
    [SvgElement("feImage")]
    public class SvgImage : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feImage filter Process().
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgImage>();
        }
    }
}
