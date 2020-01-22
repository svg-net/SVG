namespace Svg.FilterEffects
{
    [SvgElement("feFuncR")]
    public class SvgFuncR : SvgComponentTransferFunction
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
