using System.Drawing.Drawing2D;

namespace Svg
{
    /// <summary>
    /// Represents and SVG ellipse element.
    /// </summary>
    [SvgElement("ellipse")]
    public class SvgEllipse : SvgPathBasedElement
    {
        private SvgUnit _centerX = 0f;
        private SvgUnit _centerY = 0f;
        private SvgUnit _radiusX = 0f;
        private SvgUnit _radiusY = 0f;

        private GraphicsPath _path;

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

        [SvgAttribute("rx")]
        public virtual SvgUnit RadiusX
        {
            get { return _radiusX; }
            set { _radiusX = value; Attributes["rx"] = value; IsPathDirty = true; }
        }

        [SvgAttribute("ry")]
        public virtual SvgUnit RadiusY
        {
            get { return _radiusY; }
            set { _radiusY = value; Attributes["ry"] = value; IsPathDirty = true; }
        }

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        /// <value></value>
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

                var center = SvgUnit.GetDevicePoint(this.CenterX, this.CenterY, renderer, this);
                var radiusX = this.RadiusX.ToDeviceValue(renderer, UnitRenderingType.Other, this) + halfStrokeWidth;
                var radiusY = this.RadiusY.ToDeviceValue(renderer, UnitRenderingType.Other, this) + halfStrokeWidth;

                this._path = new GraphicsPath();
                _path.StartFigure();
                _path.AddEllipse(center.X - radiusX, center.Y - radiusY, 2 * radiusX, 2 * radiusY);
                _path.CloseFigure();
            }
            return _path;
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents using the specified <see cref="ISvgRenderer"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object used for rendering.</param>
        protected override void Render(ISvgRenderer renderer)
        {
            if (this.RadiusX.Value > 0.0f && this.RadiusY.Value > 0.0f)
            {
                base.Render(renderer);
            }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgEllipse>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgEllipse;

            newObj._centerX = _centerX;
            newObj._centerY = _centerY;
            newObj._radiusX = _radiusX;
            newObj._radiusY = _radiusY;
            return newObj;
        }
    }
}
