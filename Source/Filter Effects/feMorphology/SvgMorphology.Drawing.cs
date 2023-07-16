namespace Svg.FilterEffects
{
    public partial class SvgMorphology : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feMorphology filter Process().
            buffer[Result] = buffer[Input];
        }
    }
}
