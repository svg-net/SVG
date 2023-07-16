namespace Svg.FilterEffects
{
    public partial class SvgComposite : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feComposite filter Process().
            buffer[Result] = buffer[Input];
        }
    }
}
