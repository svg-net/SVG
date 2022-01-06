#if !NO_SDC
namespace Svg.FilterEffects
{
    public partial class SvgTile : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feTile filter Process().
            buffer[Result] = buffer[Input];
        }
    }
}
#endif
