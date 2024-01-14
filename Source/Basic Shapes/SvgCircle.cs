namespace Svg
{
    /// <summary>
    /// An SVG element to render circles to the document.
    /// </summary>
    [SvgElement("circle")]
    public partial class SvgCircle : SvgPathBasedElement
    {
        private SvgUnit _centerX = 0f;
        private SvgUnit _centerY = 0f;
        private SvgUnit _radius = 0f;

        /// <summary>
        /// Gets the center point of the circle.
        /// </summary>
        /// <value>The center.</value>
        public SvgPoint Center
        {
            get { return new SvgPoint(this.CenterX, this.CenterY); }
        }

        [SvgAttribute("cx")]
        public virtual SvgUnit CenterX
        {
            get { return _centerX; }
            set { _centerX = value; Attributes["cx"] = value; IsPathDirty = true; }
        }

        [SvgAttribute("cy")]
        public virtual SvgUnit CenterY
        {
            get { return _centerY; }
            set { _centerY = value; Attributes["cy"] = value; IsPathDirty = true; }
        }

        [SvgAttribute("r")]
        public virtual SvgUnit Radius
        {
            get { return _radius; }
            set { _radius = value; Attributes["r"] = value; IsPathDirty = true; }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgCircle>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgCircle;

            newObj._centerX = _centerX;
            newObj._centerY = _centerY;
            newObj._radius = _radius;
            return newObj;
        }
    }
}
