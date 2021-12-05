using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.Pathing
{
    public abstract class SvgPathSegment
    {
        protected static readonly PointF NaN = new PointF(float.NaN, float.NaN);

        public bool IsRelative { get; set; }

        public PointF End { get; set; }

        protected SvgPathSegment(bool isRelative)
        {
            IsRelative = isRelative;
        }

        protected SvgPathSegment(bool isRelative, PointF end)
            : this(isRelative)
        {
            End = end;
        }

        protected static PointF Reflect(PointF point, PointF mirror)
        {
            var dx = Math.Abs(mirror.X - point.X);
            var dy = Math.Abs(mirror.Y - point.Y);

            var x = mirror.X + (mirror.X >= point.X ? dx : -dx);
            var y = mirror.Y + (mirror.Y >= point.Y ? dy : -dy);

            return new PointF(x, y);
        }

        protected static PointF ToAbsolute(PointF point, bool isRelative, PointF start)
        {
            if (float.IsNaN(point.X))
                point.X = start.X;
            else if (isRelative)
                point.X += start.X;

            if (float.IsNaN(point.Y))
                point.Y = start.Y;
            else if (isRelative)
                point.Y += start.Y;

            return point;
        }

        public abstract PointF AddToPath(GraphicsPath graphicsPath, PointF start, SvgPathSegmentList parent);

        public SvgPathSegment Clone()
        {
            return MemberwiseClone() as SvgPathSegment;
        }

        [Obsolete("Will be removed.")]
        public PointF Start { get; set; }
        [Obsolete("Use new AddToPath.")]
        public abstract void AddToPath(GraphicsPath graphicsPath);
    }
}
