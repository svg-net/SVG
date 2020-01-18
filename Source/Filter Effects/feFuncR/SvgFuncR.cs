namespace Svg.FilterEffects
{
    [SvgElement("feFuncR")]
    public class SvgFuncR : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feFuncR filter Process().
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgFuncR>();
        }
    }
}
