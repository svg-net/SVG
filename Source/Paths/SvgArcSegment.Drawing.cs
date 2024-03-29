#if !NO_SDC
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.Pathing
{
    public sealed partial class SvgArcSegment : SvgPathSegment
    {
        private static double CalculateVectorAngle(double ux, double uy, double vx, double vy)
        {
            var ta = Math.Atan2(uy, ux);
            var tb = Math.Atan2(vy, vx);

            if (tb >= ta)
            {
                return tb - ta;
            }

            return DoublePI - (ta - tb);
        }

        public override PointF AddToPath(GraphicsPath graphicsPath, PointF start, SvgPathSegmentList parent)
        {
            var end = ToAbsolute(End, IsRelative, start);

            if (start == end)
            {
                return end;
            }

            if (RadiusX == 0.0f && RadiusY == 0.0f)
            {
                graphicsPath.AddLine(start, end);
                return end;
            }

            var sinPhi = Math.Sin(Angle * RadiansPerDegree);
            var cosPhi = Math.Cos(Angle * RadiansPerDegree);

            var x1dash = cosPhi * (start.X - end.X) / 2.0 + sinPhi * (start.Y - end.Y) / 2.0;
            var y1dash = -sinPhi * (start.X - end.X) / 2.0 + cosPhi * (start.Y - end.Y) / 2.0;

            double root;
            var numerator = RadiusX * RadiusX * RadiusY * RadiusY - RadiusX * RadiusX * y1dash * y1dash - RadiusY * RadiusY * x1dash * x1dash;

            var rx = RadiusX;
            var ry = RadiusY;

            if (numerator < 0.0)
            {
                var s = (float)Math.Sqrt(1.0 - numerator / (RadiusX * RadiusX * RadiusY * RadiusY));

                rx *= s;
                ry *= s;
                root = 0.0;
            }
            else
            {
                root = ((Size == SvgArcSize.Large && Sweep == SvgArcSweep.Positive) || (Size == SvgArcSize.Small && Sweep == SvgArcSweep.Negative) ? -1.0 : 1.0) * Math.Sqrt(numerator / (RadiusX * RadiusX * y1dash * y1dash + RadiusY * RadiusY * x1dash * x1dash));
            }

            var cxdash = root * rx * y1dash / ry;
            var cydash = -root * ry * x1dash / rx;

            var cx = cosPhi * cxdash - sinPhi * cydash + (start.X + end.X) / 2.0;
            var cy = sinPhi * cxdash + cosPhi * cydash + (start.Y + end.Y) / 2.0;

            var theta1 = CalculateVectorAngle(1.0, 0.0, (x1dash - cxdash) / rx, (y1dash - cydash) / ry);
            var dtheta = CalculateVectorAngle((x1dash - cxdash) / rx, (y1dash - cydash) / ry, (-x1dash - cxdash) / rx, (-y1dash - cydash) / ry);

            if (Sweep == SvgArcSweep.Negative && dtheta > 0)
            {
                dtheta -= 2.0 * Math.PI;
            }
            else if (Sweep == SvgArcSweep.Positive && dtheta < 0)
            {
                dtheta += 2.0 * Math.PI;
            }

            var segments = (int)Math.Ceiling((double)Math.Abs(dtheta / (Math.PI / 2.0)));
            var delta = dtheta / segments;
            var t = 8.0 / 3.0 * Math.Sin(delta / 4.0) * Math.Sin(delta / 4.0) / Math.Sin(delta / 2.0);

            var startX = start.X;
            var startY = start.Y;

            for (var i = 0; i < segments; ++i)
            {
                var cosTheta1 = Math.Cos(theta1);
                var sinTheta1 = Math.Sin(theta1);
                var theta2 = theta1 + delta;
                var cosTheta2 = Math.Cos(theta2);
                var sinTheta2 = Math.Sin(theta2);

                var endpointX = cosPhi * rx * cosTheta2 - sinPhi * ry * sinTheta2 + cx;
                var endpointY = sinPhi * rx * cosTheta2 + cosPhi * ry * sinTheta2 + cy;

                var dx1 = t * (-cosPhi * rx * sinTheta1 - sinPhi * ry * cosTheta1);
                var dy1 = t * (-sinPhi * rx * sinTheta1 + cosPhi * ry * cosTheta1);

                var dxe = t * (cosPhi * rx * sinTheta2 + sinPhi * ry * cosTheta2);
                var dye = t * (sinPhi * rx * sinTheta2 - cosPhi * ry * cosTheta2);

                graphicsPath.AddBezier(startX, startY, (float)(startX + dx1), (float)(startY + dy1),
                    (float)(endpointX + dxe), (float)(endpointY + dye), (float)endpointX, (float)endpointY);

                theta1 = theta2;
                startX = (float)endpointX;
                startY = (float)endpointY;
            }

            return end;
        }

        [Obsolete("Use new AddToPath.")]
        public override void AddToPath(GraphicsPath graphicsPath)
        {
            AddToPath(graphicsPath, Start, null);
        }
    }
}
#endif
