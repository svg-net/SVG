#if !NO_SDC
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace Svg
{
    public partial class SvgPolyline : SvgPolygon
    {
        private GraphicsPath _path;

        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            if (_path == null || this.IsPathDirty)
            {
                _path = new GraphicsPath();

                try
                {
                    for (int i = 0; (i + 1) < Points.Count; i += 2)
                    {
                        PointF endPoint = new PointF(Points[i].ToDeviceValue(renderer, UnitRenderingType.Horizontal, this),
                            Points[i + 1].ToDeviceValue(renderer, UnitRenderingType.Vertical, this));

                        if (renderer == null)
                        {
                            var radius = base.StrokeWidth / 2;
                            _path.AddEllipse(endPoint.X - radius, endPoint.Y - radius, 2 * radius, 2 * radius);
                            continue;
                        }

                        // TODO: Remove unrequired first line
                        if (_path.PointCount == 0)
                        {
                            _path.AddLine(endPoint, endPoint);
                        }
                        else
                        {
                            _path.AddLine(_path.GetLastPoint(), endPoint);
                        }
                    }
                }
                catch (Exception exc)
                {
                    Trace.TraceError("Error rendering points: " + exc.Message);
                }
                if (renderer != null)
                    this.IsPathDirty = false;
            }
            return _path;
        }
    }
}
#endif
