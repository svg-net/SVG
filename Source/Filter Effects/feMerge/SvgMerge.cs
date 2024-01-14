namespace Svg.FilterEffects
{
    [SvgElement("feMerge")]
    public partial class SvgMerge : SvgFilterPrimitive
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgMerge>();
        }
    }
}
