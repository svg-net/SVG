namespace Svg.FilterEffects
{
    [SvgElement("feFuncB")]
    public class SvgFuncB : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feFuncB filter Process().
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgFuncB>();
        }
    }
}
