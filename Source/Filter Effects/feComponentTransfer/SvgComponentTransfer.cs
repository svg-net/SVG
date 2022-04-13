namespace Svg.FilterEffects
{
    [SvgElement("feComponentTransfer")]
    public partial class SvgComponentTransfer : SvgFilterPrimitive
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgComponentTransfer>();
        }
    }
}
