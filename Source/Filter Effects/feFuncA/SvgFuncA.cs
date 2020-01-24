namespace Svg.FilterEffects
{
    [SvgElement("feFuncA")]
    public class SvgFuncA : SvgComponentTransferFunction
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgFuncA>();
        }
    }
}
