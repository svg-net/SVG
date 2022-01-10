namespace Svg
{
    [SvgElement("tspan")]
    public partial class SvgTextSpan : SvgTextBase
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgTextSpan>();
        }
    }
}
