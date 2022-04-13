#if !NO_SDC
namespace Svg.FilterEffects
{
    public partial class SvgSpecularLighting : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feSpecularLighting filter Process().
            buffer[Result] = buffer[Input];
        }
    }
}
#endif
