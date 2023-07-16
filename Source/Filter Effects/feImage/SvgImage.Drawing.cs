namespace Svg.FilterEffects
{
    public partial class SvgImage : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feImage filter Process().
            buffer[Result] = buffer[Input];
        }
    }
}
