namespace Svg.FilterEffects
{
    [SvgElement("feDistantLight")]
    public class SvgDistantLight : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feDistantLight filter Process().
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgDistantLight>();
        }
    }
}
