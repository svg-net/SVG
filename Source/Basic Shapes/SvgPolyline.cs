using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace Svg
{
    /// <summary>
    /// SvgPolyline defines a set of connected straight line segments. Typically, <see cref="SvgPolyline"/> defines open shapes.
    /// </summary>
    [SvgElement("polyline")]
    public class SvgPolyline : SvgPolygon
    {
        private GraphicsPath _Path;
        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            if (_Path == null || this.IsPathDirty)
            {
                _Path = new GraphicsPath();

                try
                {
                    for (int i = 0; i < Points.Count; i += 2)
                    {
                        PointF endPoint = new PointF(Points[i].ToDeviceValue(renderer, UnitRenderingType.Horizontal, this), 
                                                     Points[i + 1].ToDeviceValue(renderer, UnitRenderingType.Vertical, this));

                        // TODO: Remove unrequired first line
                        if (_Path.PointCount == 0)
                        {
                            _Path.AddLine(endPoint, endPoint);
                        }
                        else
                        {
                            _Path.AddLine(_Path.GetLastPoint(), endPoint);
                        }
                    }
                }
                catch (Exception exc)
                {
                    Trace.TraceError("Error rendering points: " + exc.Message);
                }
                this.IsPathDirty = false;
            }
            return _Path;
        }
    }
}