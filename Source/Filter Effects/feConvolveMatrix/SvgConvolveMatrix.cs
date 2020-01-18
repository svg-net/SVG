namespace Svg.FilterEffects
{
    [SvgElement("feConvolveMatrix")]
    public class SvgConvolveMatrix : SvgFilterPrimitive
    {
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
