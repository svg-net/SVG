namespace Svg.FilterEffects
{
    [SvgElement("feComponentTransfer")]
    public partial class SvgComponentTransfer : SvgFilterPrimitive
    {
#if !NO_SDC
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feComponentTransfer filter Process().
            buffer[Result] = buffer[Input];
        }
#endif

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgComponentTransfer>();
        }
    }
}
