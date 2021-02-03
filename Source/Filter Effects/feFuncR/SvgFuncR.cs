namespace Svg.FilterEffects
{
    [SvgElement("feFuncR")]
    public partial class SvgFuncR : SvgComponentTransferFunction
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgFuncR>();
        }
    }
}
