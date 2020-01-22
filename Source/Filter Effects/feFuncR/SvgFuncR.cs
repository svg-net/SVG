namespace Svg.FilterEffects
{
    [SvgElement("feFuncR")]
    public class SvgFuncR : SvgComponentTransferFunctionElement
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
