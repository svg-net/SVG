using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace Svg
{
    /// <summary>
    /// SvgPolygon defines a closed shape consisting of a set of connected straight line segments.
    /// </summary>
    [SvgElement("polygon")]
    public class SvgPolygon : SvgMarkerElement
    {
        private GraphicsPath _path;
        
        /// <summary>
        /// The points that make up the SvgPolygon
        /// </summary>
        [SvgAttribute("points")]
        public SvgPointCollection Points
        {
            get { return this.Attributes["points"] as SvgPointCollection; }
            set { this.Attributes["points"] = value; this.IsPathDirty = true; }
        }

        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            if (this._path == null || this.IsPathDirty)
            {
                this._path = new GraphicsPath();
                this._path.StartFigure();

                try
                {
                    var points = this.Points;
                    for (int i = 2; (i + 1) < points.Count; i += 2)
                    {
                        var endPoint = SvgUnit.GetDevicePoint(points[i], points[i + 1], renderer, this);

                      // If it is to render, don't need to consider stroke width.
                        // i.e stroke width only to be considered when calculating boundary
                        if (renderer == null)
                        {
                          var radius = base.StrokeWidth / 2;
                          _path.AddEllipse(endPoint.X - radius, endPoint.Y - radius, 2 * radius, 2 * radius);
                          continue;
                        }

                        //first line
                        if (_path.PointCount == 0)
                        {
                            _path.AddLine(SvgUnit.GetDevicePoint(points[i - 2], points[i - 1], renderer, this), endPoint);
                        }
                        else
                        {
                            _path.AddLine(_path.GetLastPoint(), endPoint);
                        }
                    }
                }
                catch
                {
                    Trace.TraceError("Error parsing points");
                }

                this._path.CloseFigure();
                if (renderer != null)
                  this.IsPathDirty = false;
            }
            return this._path;
        }

		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgPolygon>();
		}

		public override SvgElement DeepCopy<T>()
		{
			var newObj = base.DeepCopy<T>() as SvgPolygon;
			newObj.Points = new SvgPointCollection();
			foreach (var pt in this.Points)
				newObj.Points.Add(pt);
			return newObj;
		}
    }
}