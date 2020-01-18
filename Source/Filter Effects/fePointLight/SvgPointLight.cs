namespace Svg.FilterEffects
{
    [SvgElement("fePointLight")]
    public class SvgPointLight : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement fePointLight filter Process().
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgPointLight>();
        }
    }
}
