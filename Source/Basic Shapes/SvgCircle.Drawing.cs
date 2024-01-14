#if !NO_SDC
using System.Drawing.Drawing2D;

namespace Svg
{
    public partial class SvgCircle : SvgPathBasedElement
    {
        private GraphicsPath _path;

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
    }
}
#endif
