#if !NO_SDC
namespace Svg.FilterEffects
{
    public partial class SvgDisplacementMap : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feDisplacementMap filter Process().
            buffer[Result] = buffer[Input];
        }
    }
}
#endif
