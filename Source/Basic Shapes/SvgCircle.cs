using System.Drawing.Drawing2D;

namespace Svg
{
    /// <summary>
    /// An SVG element to render circles to the document.
    /// </summary>
    [SvgElement("circle")]
    public class SvgCircle : SvgPathBasedElement
    {
        private SvgUnit _centerX = 0f;
        private SvgUnit _centerY = 0f;
        private SvgUnit _radius = 0f;

        private GraphicsPath _path;

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

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> representing this element.
        /// </summary>
        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            if (this._path == null || this.IsPathDirty)
            {
                var halfStrokeWidth = base.StrokeWidth / 2;

                // If it is to render, don't need to consider stroke width.
                // i.e stroke width only to be considered when calculating boundary
                if (renderer != null)
                {
                    halfStrokeWidth = 0;
                    this.IsPathDirty = false;
                }

                _path = new GraphicsPath();
                _path.StartFigure();
                var center = this.Center.ToDeviceValue(renderer, this);
                var radius = this.Radius.ToDeviceValue(renderer, UnitRenderingType.Other, this) + halfStrokeWidth;
                _path.AddEllipse(center.X - radius, center.Y - radius, 2 * radius, 2 * radius);
                _path.CloseFigure();
            }
            return _path;
        }

        /// <summary>
        /// Renders the circle using the specified <see cref="ISvgRenderer"/> object.
        /// </summary>
        /// <param name="renderer">The renderer object.</param>
        protected override void Render(ISvgRenderer renderer)
        {
            // Don't draw if there is no radius set
            if (this.Radius.Value > 0.0f)
            {
                base.Render(renderer);
            }
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
