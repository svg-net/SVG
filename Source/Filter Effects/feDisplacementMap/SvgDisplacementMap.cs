namespace Svg.FilterEffects
{
    [SvgElement("feDisplacementMap")]
    public class SvgDisplacementMap : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feDisplacementMap filter Process().
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgDisplacementMap>();
        }
    }
}
