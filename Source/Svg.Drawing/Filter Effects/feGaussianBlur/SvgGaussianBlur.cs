namespace Svg.FilterEffects
{
    public enum BlurType
    {
        Both,
        HorizontalOnly,
        VerticalOnly,
    }

    [SvgElement("feGaussianBlur")]
    public partial class SvgGaussianBlur : SvgFilterPrimitive
    {
        /// <summary>
        /// Gets or sets the radius of the blur (only allows for one value - not the two specified in the SVG Spec)
        /// </summary>
        [SvgAttribute("stdDeviation")]
        public SvgNumberCollection StdDeviation
        {
            get { return GetAttribute("stdDeviation", false, new SvgNumberCollection() { 0f }); }
            set { Attributes["stdDeviation"] = value; }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgGaussianBlur>();
        }
    }
}
