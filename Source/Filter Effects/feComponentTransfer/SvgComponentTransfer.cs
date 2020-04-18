namespace Svg.FilterEffects
{
    [SvgElement("feComponentTransfer")]
    public class SvgComponentTransfer : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feComponentTransfer filter Process().
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgComponentTransfer>();
        }
    }
}
