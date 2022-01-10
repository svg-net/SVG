#if !NO_SDC
namespace Svg.FilterEffects
{
    public partial class SvgTurbulence : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feTurbulence filter Process().
            buffer[Result] = buffer[Input];
        }
    }
}
#endif
