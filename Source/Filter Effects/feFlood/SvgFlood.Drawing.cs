namespace Svg.FilterEffects
{
    public partial class SvgFlood : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feFlood filter Process().
            buffer[Result] = buffer[Input];
        }
    }
}
