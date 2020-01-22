namespace Svg.FilterEffects
{
    [SvgElement("feDistantLight")]
    public class SvgDistantLight : SvgElement
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgDistantLight>();
        }
    }
}
