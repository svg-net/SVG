namespace Svg.FilterEffects
{
    [SvgElement("fePointLight")]
    public class SvgPointLight : SvgElement
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgPointLight>();
        }
    }
}
