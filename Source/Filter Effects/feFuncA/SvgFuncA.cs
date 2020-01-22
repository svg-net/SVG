namespace Svg.FilterEffects
{
    [SvgElement("feFuncA")]
    public class SvgFuncA : SvgComponentTransferFunctionElement
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgFuncA>();
        }
    }
}
