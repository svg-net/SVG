namespace Svg.FilterEffects
{
    public partial class SvgComponentTransfer : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feComponentTransfer filter Process().
            buffer[Result] = buffer[Input];
        }
    }
}
