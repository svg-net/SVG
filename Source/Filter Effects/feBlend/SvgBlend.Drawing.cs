namespace Svg.FilterEffects
{
    public partial class SvgBlend : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feBlend filter Process().
            buffer[Result] = buffer[Input];
        }
    }
}
