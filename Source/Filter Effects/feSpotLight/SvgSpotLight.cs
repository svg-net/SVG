namespace Svg.FilterEffects
{
    [SvgElement("feSpotLight")]
    public class SvgSpotLight : SvgElement
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgSpotLight>();
        }
    }
}
