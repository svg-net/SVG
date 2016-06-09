using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Android.Graphics;
using Android.Provider;
using PointF = System.Drawing.PointF;

namespace Svg.Droid
{
    public class AndroidGraphicsPath : GraphicsPath
    {
        private readonly FillMode _fillmode;
        private readonly List<PointF> _points = new List<PointF>();
        private readonly List<byte> _pathTypes = new List<byte>();
        private Android.Graphics.Path _path = new Android.Graphics.Path();
        private Paint _paint = new Paint() {Color = Android.Graphics.Color.Black};

        public AndroidGraphicsPath()
        {
        }

        public AndroidGraphicsPath(FillMode fillmode)
        {
            _fillmode = fillmode;

            switch (fillmode)
            {
                case FillMode.Alternate:
                    Path.SetFillType(Path.FillType.EvenOdd);
                    break;
                case FillMode.Winding:
                    Path.SetFillType(Path.FillType.Winding);
                    break;
            }
        }


        public void Dispose()
        {
            if (_path != null)
            {
                _path.Dispose();
                _path = null;
            }
            if (_paint != null)
            {
                _paint.Dispose();
                _paint = null;
            }
        }

        public RectangleF GetBounds()
        {
            var r = new RectF();
            _path.ComputeBounds(r, true);
            return new RectangleF(r.Left, r.Top, r.Width(), r.Height());
        }

        public void StartFigure()
        {
            
        }
        public void CloseFigure()
        {
            Path.Close();
        }

        public decimal PointCount { get { return _points.Count; } }
        public PointF[] PathPoints { get { return _points.ToArray(); } }
        public FillMode FillMode { get; set; }

        /// <summary>
        /// see: https://msdn.microsoft.com/en-us/library/system.drawing.drawing2d.graphicspath.pathtypes%28v=vs.110%29.aspx
        /// </summary>
        public byte[] PathTypes
        {
            get
            {
                return _pathTypes.ToArray();
            }
            set
            {
                _pathTypes.Clear();
                _pathTypes.AddRange(value);
            }
        }

        public PathData PathData
        {
            get
            {
                return new PathData(PathPoints, PathTypes);
            }
        }

        public Path Path
        {
            get { return _path; }
        }

        public void AddEllipse(float x, float y, float width, float height)
        {
            // TODO LX: Which direction is correct?
            Path.AddOval(new RectF(x, y, x + width, y + height), Path.Direction.Cw);

            _points.Add(new PointF(x, y));
            _points.Add(new PointF(x + width, y + height));
            _pathTypes.Add(0); // start of a figure
            _pathTypes.Add(0x80); // last point in closed sublath
        }

        public void MoveTo(PointF start)
        {
            Path.MoveTo(start.X, start.Y);
            _points.Add(start);
            _pathTypes.Add(1); // end point of line
        }


        public void AddLine(PointF start, PointF end)
        {
            Path.MoveTo(start.X, start.Y);
            Path.LineTo(end.X, end.Y);
            _points.Add(start);
            _points.Add(end);
            _pathTypes.Add(1); // start of a line
            _pathTypes.Add(1); // end point of line
        }

        public PointF GetLastPoint()
        {
            return _points.LastOrDefault();
        }

        public void AddRectangle(RectangleF rectangle)
        {
            Path.AddRect(rectangle.ToRectF(), Path.Direction.Cw);
            _points.Add(new PointF(rectangle.Location.X, rectangle.Location.Y));
            _pathTypes.Add(0); // start of a figure
            _points.Add(new PointF(rectangle.Location.X + rectangle.Width, rectangle.Location.Y));
            _pathTypes.Add(0x7); // TODO LX: ???
            _points.Add(new PointF(rectangle.Location.X, rectangle.Location.Y + rectangle.Height));
            _pathTypes.Add(0x7); // TODO LX: ???
            _points.Add(new PointF(rectangle.Location.X + rectangle.Width, rectangle.Location.Y + rectangle.Height));
            _pathTypes.Add(0x80); // TODO LX: ???
        }

        public void AddArc(RectangleF rectangle, float startAngle, float sweepAngle)
        {
            Path.AddArc(rectangle.ToRectF(), startAngle, sweepAngle);

            _points.Add(new PointF(rectangle.Location.X, rectangle.Location.Y));
            _pathTypes.Add(1); // start point of line
            _points.Add(new PointF(rectangle.Location.X + rectangle.Width, rectangle.Location.Y));
            _pathTypes.Add(0x20); // TODO LX: ???
            _points.Add(new PointF(rectangle.Location.X, rectangle.Location.Y + rectangle.Height));
            _pathTypes.Add(0x20); // TODO LX: ???
            _points.Add(new PointF(rectangle.Location.X + rectangle.Width, rectangle.Location.Y + rectangle.Height));
            _pathTypes.Add(1); // end point of line
        }

        public GraphicsPath Clone()
        {
            var cl = new AndroidGraphicsPath();
            cl._path = new Path(this.Path);
            cl._points.AddRange(this._points);
            cl._pathTypes.AddRange(this._pathTypes);
            return cl;
        }

        public void Transform(Matrix transform)
        {
            var m = new Android.Graphics.Matrix();
            m.SetValues(transform.Elements);
            Path.Transform(m);
        }

        public void AddPath(GraphicsPath childPath, bool connect)
        {
            var ap = (AndroidGraphicsPath) childPath;
            // TODO LX: How to connect? And is 0, 0 correct?
            Path.AddPath(ap.Path, 0, 0);

            _points.AddRange(ap._points);
            _pathTypes.AddRange(ap._pathTypes);
        }

        public void AddString(string text, FontFamily fontFamily, int style, float size, PointF location,
            StringFormat createStringFormatGenericTypographic)
        {
            throw new NotSupportedException();
        }

        public void AddBezier(PointF start, PointF point1, PointF point2, PointF point3)
        {
            Path.MoveTo(start.X, start.Y);
            Path.CubicTo(point1.X, point1.Y, point2.X, point2.Y, point3.X, point3.Y);

            _points.AddRange(new[] { start, point1, point2, point3 });
            _pathTypes.Add(1); // start point of line
            _pathTypes.Add(3); // control point of cubic bezier spline
            _pathTypes.Add(3); // control point of cubic bezier spline
            _pathTypes.Add(3); // endpoint of cubic bezier spline
        }

        public void AddBezier(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            Path.MoveTo(x1, y2);
            Path.CubicTo(x2, y2, x3, y3, x4, y4);

            _points.AddRange(new[] { new PointF(x1, y1), new PointF(x2, y2), new PointF(x3, y3), new PointF(x4, y4) });
            _pathTypes.Add(1); // start point of line
            _pathTypes.Add(3); // control point of cubic bezier spline
            _pathTypes.Add(3); // control point of cubic bezier spline
            _pathTypes.Add(3); // endpoint of cubic bezier spline
        }

        public bool IsVisible(PointF pointF)
        {
            RectF rect = new RectF();
            Path.ComputeBounds(rect, true);

            return rect.Contains(pointF.X, pointF.Y);
        }

        public void Flatten()
        {
            // TODO LX not supported by Android.Graphics.Path
            throw new NotSupportedException();
        }

        public void AddPolygon(PointF[] polygon)
        {
            for (int i = 0; i < polygon.Length; i++)
            {
                if (i == 0)
                {
                    Path.MoveTo(polygon[i].X, polygon[i].Y);
                    _points.Add(polygon[i]);
                    _pathTypes.Add(0); // start point of figure
                }
                else if (i == polygon.Length - 1)
                {
                    Path.Close();
                    _points.Add(polygon[i]);
                    _pathTypes.Add(0x80); // end point of figure
                }
                else
                {
                    Path.LineTo(polygon[i].X, polygon[i].Y);
                    _points.Add(polygon[i]);
                    _pathTypes.Add(1); // TODO LX: ???
                }
            }
        }

        public void Reset()
        {
            Path.Reset();
        }
    }
}