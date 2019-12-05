using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.Pathing
{
    public sealed class SvgQuadraticCurveSegment : SvgPathSegment
    {
        public PointF ControlPoint { get; set; }

        private PointF FirstControlPoint
        {
            get
            {
                var x1 = Start.X + (ControlPoint.X - Start.X) * 2 / 3;
                var y1 = Start.Y + (ControlPoint.Y - Start.Y) * 2 / 3;

                return new PointF(x1, y1);
            }
        }

        private PointF SecondControlPoint
        {
            get
            {
                var x2 = ControlPoint.X + (End.X - ControlPoint.X) / 3;
                var y2 = ControlPoint.Y + (End.Y - ControlPoint.Y) / 3;

                return new PointF(x2, y2);
            }
        }

        public SvgQuadraticCurveSegment(PointF start, PointF controlPoint, PointF end)
            : base(start, end)
        {
            ControlPoint = controlPoint;
        }

        public override void AddToPath(GraphicsPath graphicsPath)
        {
            graphicsPath.AddBezier(Start, FirstControlPoint, SecondControlPoint, End);
        }

        public override string ToString()
        {
            return "Q" + ControlPoint.ToSvgString() + " " + End.ToSvgString();
        }
    }
}
