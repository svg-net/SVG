namespace Svg.FilterEffects
{
    [SvgElement("feFuncB")]
    public partial class SvgFuncB : SvgComponentTransferFunction
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgFuncB>();
        }
    }
}
