namespace Svg.FilterEffects
{
    [SvgElement("feTile")]
    public partial class SvgTile : SvgFilterPrimitive
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgTile>();
        }
    }
}
