namespace Svg.FilterEffects
{
    [SvgElement("feSpotLight")]
    public class SvgSpotLight : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feSpotLight filter Process().
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgSpotLight>();
        }
    }
}
