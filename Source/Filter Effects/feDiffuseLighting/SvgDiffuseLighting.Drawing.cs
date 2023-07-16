namespace Svg.FilterEffects
{
    public partial class SvgDiffuseLighting : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feDiffuseLighting filter Process().
            buffer[Result] = buffer[Input];
        }
    }
}
