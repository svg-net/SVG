using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    public partial class SvgLine : SvgMarkerElement
    {
        private GraphicsPath _path;

        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            if ((this._path == null || this.IsPathDirty) && base.StrokeWidth > 0)
            {
                PointF start = new PointF(this.StartX.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this),
                    this.StartY.ToDeviceValue(renderer, UnitRenderingType.Vertical, this));
                PointF end = new PointF(this.EndX.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this),
                    this.EndY.ToDeviceValue(renderer, UnitRenderingType.Vertical, this));

                this._path = new GraphicsPath();

                // If it is to render, don't need to consider stroke width.
                // i.e stroke width only to be considered when calculating boundary
                if (renderer != null)
                {
                    this._path.AddLine(start, end);
                    this.IsPathDirty = false;
                }
                else
                {    // only when calculating boundary
                    _path.StartFigure();
                    var radius = base.StrokeWidth / 2;
                    _path.AddEllipse(start.X - radius, start.Y - radius, 2 * radius, 2 * radius);
                    _path.AddEllipse(end.X - radius, end.Y - radius, 2 * radius, 2 * radius);
                    _path.CloseFigure();
                }
            }
            return this._path;
        }
    }
}
