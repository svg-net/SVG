namespace Svg.FilterEffects
{
    [SvgElement("feFuncA")]
    public partial class SvgFuncA : SvgComponentTransferFunction
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgFuncA>();
        }
    }
}
