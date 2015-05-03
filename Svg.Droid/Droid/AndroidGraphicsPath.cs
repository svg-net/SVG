using System;
using System.Drawing;
using Android.Graphics;
using PointF = System.Drawing.PointF;

namespace Svg.Droid
{
    public class AndroidGraphicsPath : GraphicsPath
    {
        private readonly FillMode _fillmode;
        private Android.Graphics.Path _path;

        public AndroidGraphicsPath()
        {
            _path = new Android.Graphics.Path();
        }

        public AndroidGraphicsPath(FillMode fillmode)
        {
            _fillmode = fillmode;
            _path = new Android.Graphics.Path();


            switch (fillmode)
            {
                case FillMode.Alternate:
                    _path.SetFillType(Path.FillType.EvenOdd);
                    break;
                case FillMode.Winding:
                    _path.SetFillType(Path.FillType.Winding);
                    break;
            }
        }


        public void Dispose()
        {
            _path.Dispose();
        }

        public RectangleF GetBounds()
        {
            var r = new RectF();
            _path.ComputeBounds(r, true);
            return new RectangleF(r.Left, r.Top, r.Width(), r.Height());
        }

        public void StartFigure()
        {
            throw new NotSupportedException();
        }

        public void AddEllipse(float x, float y, float width, float height)
        {
            // TODO LX: Which direction is correct?
            _path.AddOval(x, y, x + width, y + height, Path.Direction.Cw);
        }

        public void CloseFigure()
        {
            _path.Close();
        }

        public decimal PointCount { get; private set; }
        public PointF[] PathPoints { get; private set; }
        public FillMode FillMode { get; set; }
        public float[] PathTypes { get; set; }
        public PathData PathData { get; set; }
        public void AddLine(PointF start, PointF end)
        {
            _path.MoveTo(start.X, start.Y);
            _path.LineTo(end.X, end.Y);
        }

        public PointF GetLastPoint()
        {
            // TODO LX: Android.Graphics.Path does not support that
            throw new NotImplementedException();
        }

        public void AddRectangle(RectangleF rectangle)
        {
            // TODO LX: is this the right direction?
            _path.AddRect(new RectF(rectangle.X, rectangle.Y, rectangle.X+rectangle.Width, rectangle.Y+rectangle.Height), Path.Direction.Cw);
        }

        public void AddArc(RectangleF rect, float startAngle, float sweepAngle)
        {
            _path.AddArc(new RectF(rect.X, rect.Y, rect.X+rect.Width, rect.Y+rect.Height), startAngle, sweepAngle);
        }

        public GraphicsPath Clone()
        {
            var cl = new AndroidGraphicsPath();
            cl._path = this._path;
            return cl;
        }

        public void Transform(Matrix transform)
        {
            var m = new Android.Graphics.Matrix();
            m.SetValues(transform.Elements);
            _path.Transform(m);
        }

        public void AddPath(GraphicsPath childPath, bool connect)
        {
            var ap = (AndroidGraphicsPath) childPath;
            // TODO LX: How to connect? And is 0, 0 correct?
            _path.AddPath(ap._path, 0, 0);
        }

        public void AddString(string text, FontFamily fontFamily, int style, float size, PointF location,
            StringFormat createStringFormatGenericTypographic)
        {
            throw new NotSupportedException();
        }

        public void AddBezier(PointF start, PointF point1, PointF point2, PointF point3)
        {
            _path.MoveTo(start.X, start.Y);
            _path.CubicTo(point1.X, point1.Y, point2.X, point2.Y, point3.X, point3.Y);
        }

        public void AddBezier(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            _path.MoveTo(x1, y2);
            _path.CubicTo(x2, y2, x3, y3, x4, y4);
        }

        public bool IsVisible(PointF pointF)
        {
            // TODO LX not supported by Android.Graphics.Path
            throw new NotSupportedException();
        }

        public void Flatten()
        {
            // TODO LX not supported by Android.Graphics.Path
            throw new NotSupportedException();
        }

        public void AddPolygon(PointF[] polygon)
        {
            // TODO LX not supported by Android.Graphics.Path
            throw new NotSupportedException();
        }

        public void Reset()
        {
            _path.Reset();
        }
    }
}