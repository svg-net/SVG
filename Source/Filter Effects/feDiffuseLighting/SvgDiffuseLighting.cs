namespace Svg.FilterEffects
{
    [SvgElement("feDiffuseLighting")]
    public class SvgDiffuseLighting : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feDiffuseLighting filter Process().
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgDiffuseLighting>();
        }
    }
}
