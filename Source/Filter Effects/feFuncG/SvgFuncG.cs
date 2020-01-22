namespace Svg.FilterEffects
{
    [SvgElement("feFuncG")]
    public class SvgFuncG : SvgComponentTransferFunction
    {
        public override void Process(ImageBuffer buffer)
        {
            // TODO: Implement feFuncG filter Process().
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgFuncG>();
        }
    }
}
