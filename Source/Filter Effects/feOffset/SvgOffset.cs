namespace Svg.FilterEffects
{
    /// <summary>
    /// Note: this is not used in calculations to bitmap - used only to allow for svg xml output
    /// </summary>
    [SvgElement("feOffset")]
    public partial class SvgOffset : SvgFilterPrimitive
    {
        private SvgUnit _dx = 0f;
        private SvgUnit _dy = 0f;

        /// <summary>
        /// The amount to offset the input graphic along the x-axis. The offset amount is expressed in the coordinate system established by attribute 'primitiveUnits' on the 'filter' element.
        /// If the attribute is not specified, then the effect is as if a value of 0 were specified.
        /// Note: this is not used in calculations to bitmap - used only to allow for svg xml output
        /// </summary>
        [SvgAttribute("dx")]
        public SvgUnit Dx
        {
            get { return _dx; }
            set { _dx = value; Attributes["dx"] = value; }
        }

        /// <summary>
        /// The amount to offset the input graphic along the y-axis. The offset amount is expressed in the coordinate system established by attribute 'primitiveUnits' on the 'filter' element.
        /// If the attribute is not specified, then the effect is as if a value of 0 were specified.
        /// Note: this is not used in calculations to bitmap - used only to allow for svg xml output
        /// </summary>
        [SvgAttribute("dy")]
        public SvgUnit Dy
        {
            get { return _dy; }
            set { _dy = value; Attributes["dy"] = value; }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgOffset>();
        }
    }
}
