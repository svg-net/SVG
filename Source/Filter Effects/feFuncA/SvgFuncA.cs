namespace Svg.FilterEffects
{
    [SvgElement("feFuncA")]
    public class SvgFuncA : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feFuncA filter Process().
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgFuncA>();
        }
    }
}
