using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using Svg.Pathing;

namespace Svg
{
    /// <summary>
    /// SvgPolygon defines a closed shape consisting of a set of connected straight line segments.
    /// </summary>
    [SvgElement("polygon")]
    public class SvgPolygon : SvgVisualElement
    {
        private GraphicsPath _path;
        private SvgUnitCollection _points;

        /// <summary>
        /// The points that make up the SvgPolygon
        /// </summary>
        [SvgAttribute("points")]
        public SvgUnitCollection Points
        {
            get { return this._points; }
            set { this._points = value; this.IsPathDirty = true; }
        }

        protected override bool RequiresSmoothRendering
        {
            get { return true; }
        }

        public override GraphicsPath Path
        {
            get
            {
                if (this._path == null || this.IsPathDirty)
                {
                    this._path = new GraphicsPath();
                    this._path.StartFigure();

                    try
                    {
                        for (int i = 2; i < this._points.Count; i+=2)
                        {
                            PointF endPoint = new PointF(this._points[i].ToDeviceValue(this), this._points[i+1].ToDeviceValue(this));

                            //first line
                            if (_path.PointCount == 0)
                            {
                                _path.AddLine(new PointF(this._points[i-2].ToDeviceValue(this), this._points[i-1].ToDeviceValue(this)), endPoint);
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
                    this.IsPathDirty = false;
                }
                return this._path;
            }
            protected set
            {
                _path = value;
            }
        }

        public override RectangleF Bounds
        {
            get { return this.Path.GetBounds(); }
        }


		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgPolygon>();
		}

		public override SvgElement DeepCopy<T>()
		{
			var newObj = base.DeepCopy<T>() as SvgPolygon;
			newObj.Points = new SvgUnitCollection();
			foreach (var pt in this.Points)
				newObj.Points.Add(pt);
			return newObj;
		}
    }
}