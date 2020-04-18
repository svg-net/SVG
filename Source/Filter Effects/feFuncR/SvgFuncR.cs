namespace Svg.FilterEffects
{
    [SvgElement("feFuncR")]
    public class SvgFuncR : SvgComponentTransferFunction
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgFuncR>();
        }
    }
}
