namespace Svg
{
    [SvgElement("mask")]
    public class SvgMask : SvgElement
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgMask>();
        }
    }
}
