namespace Svg.FilterEffects
{
    [SvgElement("feComposite")]
    public class SvgComposite : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feComposite filter Process().
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgComposite>();
        }
    }
}
