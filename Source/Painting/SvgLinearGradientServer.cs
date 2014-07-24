using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Svg
{
    [SvgElement("linearGradient")]
    public sealed class SvgLinearGradientServer : SvgGradientServer
    {
        [SvgAttribute("x1")]
        public SvgUnit X1
        {
            get
            {
                return this.Attributes.GetAttribute<SvgUnit>("x1");
            }
            set
            {
                Attributes["x1"] = value;
            }
        }

        [SvgAttribute("y1")]
        public SvgUnit Y1
        {
            get
            {
                return this.Attributes.GetAttribute<SvgUnit>("y1");
            }
            set
            {
                this.Attributes["y1"] = value;
            }
        }

        [SvgAttribute("x2")]
        public SvgUnit X2
        {
            get
            {
                return this.Attributes.GetAttribute<SvgUnit>("x2");
            }
            set
            {
                Attributes["x2"] = value;
            }
        }

        [SvgAttribute("y2")]
        public SvgUnit Y2
        {
            get
            {
                return this.Attributes.GetAttribute<SvgUnit>("y2");
            }
            set
            {
                this.Attributes["y2"] = value;
            }
        }

        private bool IsInvalid
        {
            get
            {
                // Need at least 2 colours to do the gradient fill
                return this.Stops.Count < 2;
            }
        }

        public SvgLinearGradientServer()
        {
            X1 = new SvgUnit(SvgUnitType.Percentage, 0F);
            Y1 = new SvgUnit(SvgUnitType.Percentage, 0F);
            X2 = new SvgUnit(SvgUnitType.Percentage, 100F);
            Y2 = new SvgUnit(SvgUnitType.Percentage, 0F);
        }

        public override Brush GetBrush(SvgVisualElement renderingElement, float opacity)
        {
            LoadStops();
            if (IsInvalid)
            {
                return null;
            }

            var boundable = CalculateBoundable(renderingElement);

            var specifiedStart = CalculateStart(boundable);
            var specifiedEnd = CalculateEnd(boundable);

            var effectiveStart = specifiedStart;
            var effectiveEnd = specifiedEnd;

            if (NeedToExpandGradient(renderingElement, specifiedStart, specifiedEnd))
            {
                var expansion = ExpandGradient(renderingElement, specifiedStart, specifiedEnd);
                effectiveStart = expansion.Item1;
                effectiveEnd = expansion.Item2;
            }

            return new LinearGradientBrush(effectiveStart, effectiveEnd, Color.Transparent, Color.Transparent)
            {
                InterpolationColors = CalculateColorBlend(renderingElement, opacity, specifiedStart, effectiveStart, specifiedEnd, effectiveEnd),
                WrapMode = WrapMode.TileFlipX
            };
        }

        private PointF CalculateStart(ISvgBoundable boundable)
        {
            return TransformPoint(new PointF(this.X1.ToDeviceValue(boundable), this.Y1.ToDeviceValue(boundable, true)));
        }

        private PointF CalculateEnd(ISvgBoundable boundable)
        {
            return TransformPoint(new PointF(this.X2.ToDeviceValue(boundable), this.Y2.ToDeviceValue(boundable, true)));
        }

        private bool NeedToExpandGradient(ISvgBoundable boundable, PointF specifiedStart, PointF specifiedEnd)
        {
            return SpreadMethod == SvgGradientSpreadMethod.Pad && (boundable.Bounds.Contains(specifiedStart) || boundable.Bounds.Contains(specifiedEnd));
        }

        private Tuple<PointF, PointF> ExpandGradient(ISvgBoundable boundable, PointF specifiedStart, PointF specifiedEnd)
        {
            if (!NeedToExpandGradient(boundable, specifiedStart, specifiedEnd))
            {
                Debug.Fail("Unexpectedly expanding gradient when not needed!");
                return new Tuple<PointF, PointF>(specifiedStart, specifiedEnd);
            }

            var specifiedLength = CalculateDistance(specifiedStart, specifiedEnd);
            var specifiedUnitVector = new PointF((specifiedEnd.X - specifiedStart.X) / (float)specifiedLength, (specifiedEnd.Y - specifiedStart.Y) / (float)specifiedLength);

            var effectiveStart = specifiedStart;
            var effectiveEnd = specifiedEnd;

            var elementDiagonal = (float)CalculateDistance(new PointF(boundable.Bounds.Left, boundable.Bounds.Top), new PointF(boundable.Bounds.Right, boundable.Bounds.Bottom));

            var expandedStart = MovePointAlongVector(effectiveStart, specifiedUnitVector, -elementDiagonal);
            var expandedEnd = MovePointAlongVector(effectiveEnd, specifiedUnitVector, elementDiagonal);

            var intersectionPoints = new LineF(expandedStart.X, expandedStart.Y, expandedEnd.X, expandedEnd.Y).Intersection(boundable.Bounds);

            if (boundable.Bounds.Contains(specifiedStart))
            {
                effectiveStart = CalculateClosestIntersectionPoint(expandedStart, intersectionPoints);

                effectiveStart = MovePointAlongVector(effectiveStart, specifiedUnitVector, -1);
            }

            if (boundable.Bounds.Contains(specifiedEnd))
            {
                effectiveEnd = CalculateClosestIntersectionPoint(effectiveEnd, intersectionPoints);

                effectiveEnd = MovePointAlongVector(effectiveEnd, specifiedUnitVector, 1);
            }

            return new Tuple<PointF, PointF>(effectiveStart, effectiveEnd);
        }

        private ColorBlend CalculateColorBlend(SvgVisualElement owner, float opacity, PointF specifiedStart, PointF effectiveStart, PointF specifiedEnd, PointF effectiveEnd)
        {
            var colorBlend = GetColorBlend(owner, opacity, false);

            var startDelta = CalculateDistance(specifiedStart, effectiveStart);
            var endDelta = CalculateDistance(specifiedEnd, effectiveEnd);

            if (!(startDelta > 0) && !(endDelta > 0))
            {
                return colorBlend;
            }

            var specifiedLength = CalculateDistance(specifiedStart, specifiedEnd);
            var specifiedUnitVector = new PointF((specifiedEnd.X - specifiedStart.X) / (float)specifiedLength, (specifiedEnd.Y - specifiedStart.Y) / (float)specifiedLength);

            var effectiveLength = CalculateDistance(effectiveStart, effectiveEnd);

            for (var i = 0; i < colorBlend.Positions.Length; i++)
            {
                var originalPoint = MovePointAlongVector(specifiedStart, specifiedUnitVector, (float) specifiedLength * colorBlend.Positions[i]);

                var distanceFromEffectiveStart = CalculateDistance(effectiveStart, originalPoint);

                colorBlend.Positions[i] = (float) Math.Max(0F, Math.Min((distanceFromEffectiveStart / effectiveLength), 1.0F));
            }

            if (startDelta > 0)
            {
                colorBlend.Positions = new[] { 0F }.Concat(colorBlend.Positions).ToArray();
                colorBlend.Colors = new[] { colorBlend.Colors.First() }.Concat(colorBlend.Colors).ToArray();
            }

            if (endDelta > 0)
            {
                colorBlend.Positions = colorBlend.Positions.Concat(new[] { 1F }).ToArray();
                colorBlend.Colors = colorBlend.Colors.Concat(new[] { colorBlend.Colors.Last() }).ToArray();
            }

            return colorBlend;
        }

        private static PointF CalculateClosestIntersectionPoint(PointF sourcePoint, IList<PointF> targetPoints)
        {
            Debug.Assert(targetPoints.Count == 2, "Unexpected number of intersection points!");

            return CalculateDistance(sourcePoint, targetPoints[0]) < CalculateDistance(sourcePoint, targetPoints[1]) ? targetPoints[0] : targetPoints[1];
        }

        private static PointF MovePointAlongVector(PointF start, PointF unitVector, float distance)
        {
            return start + new SizeF(unitVector.X * distance, unitVector.Y * distance);
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgLinearGradientServer>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgLinearGradientServer;
            newObj.X1 = this.X1;
            newObj.Y1 = this.Y1;
            newObj.X2 = this.X2;
            newObj.Y2 = this.Y2;
            return newObj;

        }

        private sealed class LineF
        {
            private float X1
            {
                get;
                set;
            }

            private float Y1
            {
                get; 
                set;
            }

            private float X2
            {
                get;
                set;
            }

            private float Y2
            {
                get;
                set;
            }

            public LineF(float x1, float y1, float x2, float y2)
            {
                X1 = x1;
                Y1 = y1;
                X2 = x2;
                Y2 = y2;
            }

            public List<PointF> Intersection(RectangleF rectangle)
            {
                var result = new List<PointF>();

                AddIfIntersect(this, new LineF(rectangle.X, rectangle.Y, rectangle.Right, rectangle.Y), result);
                AddIfIntersect(this, new LineF(rectangle.Right, rectangle.Y, rectangle.Right, rectangle.Bottom), result);
                AddIfIntersect(this, new LineF(rectangle.Right, rectangle.Bottom, rectangle.X, rectangle.Bottom), result);
                AddIfIntersect(this, new LineF(rectangle.X, rectangle.Bottom, rectangle.X, rectangle.Y), result);

                return result;
            }

            private PointF? Intersection(LineF other)
            {
                var a1 = Y2 - Y1;
                var b1 = X1 - X2;
                var c1 = X2 * Y1 - X1 * Y2;

                var r3 = a1 * other.X1 + b1 * other.Y1 + c1;
                var r4 = a1 * other.X2 + b1 * other.Y2 + c1;

                if (r3 != 0 && r4 != 0 && Math.Sign(r3) == Math.Sign(r4))
                {
                    return null;
                }

                var a2 = other.Y2 - other.Y1;
                var b2 = other.X1 - other.X2;
                var c2 = other.X2 * other.Y1 - other.X1 * other.Y2;

                var r1 = a2 * X1 + b2 * Y1 + c2;
                var r2 = a2 * X2 + b2 * Y2 + c2;

                if (r1 != 0 && r2 != 0 && Math.Sign(r1) == Math.Sign(r2))
                {
                    return (null);
                }

                var denom = a1 * b2 - a2 * b1;

                if (denom == 0)
                {
                    return null;
                }

                var offset = denom < 0 ? -denom / 2 : denom / 2;

                var num = b1 * c2 - b2 * c1;
                var x = (num < 0 ? num - offset : num + offset) / denom;

                num = a2 * c1 - a1 * c2;
                var y = (num < 0 ? num - offset : num + offset) / denom;

                return new PointF(x, y);
            }

            private static void AddIfIntersect(LineF first, LineF second, ICollection<PointF> result)
            {
                var intersection = first.Intersection(second);

                if (intersection != null)
                {
                    result.Add(intersection.Value);
                }
            }
        }
    }
}