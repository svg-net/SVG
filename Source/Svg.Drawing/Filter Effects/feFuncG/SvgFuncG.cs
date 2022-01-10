namespace Svg.FilterEffects
{
    [SvgElement("feFuncG")]
    public partial class SvgFuncG : SvgComponentTransferFunction
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgFuncG>();
        }
    }
}
