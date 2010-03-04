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
        public override GraphicsPath Path
        {
            get
            {
                if (this._path == null || this.IsPathDirty)
                {
                    this._path = new GraphicsPath();

                    try
                    {
                        for (int i = 0; i < this._points.Count; i += 2)
                        {
                            PointF endPoint = new PointF(this._points[i].ToDeviceValue(this), this._points[i + 1].ToDeviceValue(this));

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
                    this.IsPathDirty = false;
                }
                return this._path;
            }
        }
    }
}