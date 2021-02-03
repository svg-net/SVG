namespace Svg
{
    [SvgElement("font-face-src")]
    public partial class SvgFontFaceSrc : SvgElement
    {
        public override SvgElement DeepCopy()
        {
            return base.DeepCopy<SvgFontFaceSrc>();
        }
    }
}
