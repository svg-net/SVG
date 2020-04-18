namespace Svg.FilterEffects
{
    [SvgElement("feFuncB")]
    public class SvgFuncB : SvgComponentTransferFunction
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgFuncB>();
        }
    }
}
