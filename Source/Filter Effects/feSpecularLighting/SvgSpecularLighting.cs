namespace Svg.FilterEffects
{
    [SvgElement("feSpecularLighting")]
    public class SvgSpecularLighting : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feSpecularLighting filter Process().
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgSpecularLighting>();
        }
    }
}
