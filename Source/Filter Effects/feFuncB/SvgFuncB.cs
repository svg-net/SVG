namespace Svg.FilterEffects
{
    [SvgElement("feFuncB")]
    public class SvgFuncB : SvgComponentTransferFunctionElement
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgFuncB>();
        }
    }
}
