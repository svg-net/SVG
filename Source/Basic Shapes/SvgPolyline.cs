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
                if (Path == null || this.IsPathDirty)
                {
                    Path = new GraphicsPath();

                    try
                    {
                        for (int i = 0; i < Points.Count; i += 2)
                        {
                            PointF endPoint = new PointF(Points[i].ToDeviceValue(this), Points[i + 1].ToDeviceValue(this));

                            // TODO: Remove unrequired first line
                            if (Path.PointCount == 0)
                            {
                                Path.AddLine(endPoint, endPoint);
                            }
                            else
                            {
                                Path.AddLine(Path.GetLastPoint(), endPoint);
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        Trace.TraceError("Error rendering points: " + exc.Message);
                    }
                    this.IsPathDirty = false;
                }
                return Path;
            }
        }
    }
}