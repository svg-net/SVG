namespace Svg.FilterEffects
{
    [SvgElement("feTile")]
    public partial class SvgTile : SvgFilterPrimitive
    {
#if !NO_SDC
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feTile filter Process().
            buffer[Result] = buffer[Input];
        }
#endif

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgTile>();
        }
    }
}
