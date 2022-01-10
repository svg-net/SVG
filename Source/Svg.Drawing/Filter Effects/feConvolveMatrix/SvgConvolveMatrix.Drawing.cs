#if !NO_SDC
namespace Svg.FilterEffects
{
    public partial class SvgConvolveMatrix : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feConvolveMatrix filter Process().
            buffer[Result] = buffer[Input];
        }
    }
}
#endif
