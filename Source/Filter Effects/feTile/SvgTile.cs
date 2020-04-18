namespace Svg.FilterEffects
{
    [SvgElement("feTile")]
    public class SvgTile : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feTile filter Process().
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgTile>();
        }
    }
}
