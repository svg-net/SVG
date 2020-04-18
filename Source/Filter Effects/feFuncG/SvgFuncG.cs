namespace Svg.FilterEffects
{
    [SvgElement("feFuncG")]
    public class SvgFuncG : SvgComponentTransferFunction
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgFuncG>();
        }
    }
}
