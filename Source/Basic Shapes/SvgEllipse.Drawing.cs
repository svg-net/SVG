using System.Drawing.Drawing2D;

namespace Svg
{
    public partial class SvgEllipse : SvgPathBasedElement
    {
        private GraphicsPath _path;

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
    }
}
