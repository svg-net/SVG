using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Svg
{
    [SvgElement("radialGradient")]
    public sealed class SvgRadialGradientServer : SvgGradientServer
    {
        [SvgAttribute("cx")]
        public SvgUnit CenterX
        {
            get
            {
                return this.Attributes.GetAttribute<SvgUnit>("cx");
            }
            set
            {
                this.Attributes["cx"] = value;
            }
        }

        [SvgAttribute("cy")]
        public SvgUnit CenterY
        {
            get
            {
                return this.Attributes.GetAttribute<SvgUnit>("cy");
            }
            set
            {
                this.Attributes["cy"] = value;
            }
        }

        [SvgAttribute("r")]
        public SvgUnit Radius
        {
            get
            {
                return this.Attributes.GetAttribute<SvgUnit>("r");
            }
            set
            {
                this.Attributes["r"] = value;
            }
        }

        [SvgAttribute("fx")]
        public SvgUnit FocalX
        {
            get
            {
                var value = this.Attributes.GetAttribute<SvgUnit>("fx");

                if (value.IsEmpty || value.IsNone)
                {
                    value = this.CenterX;
                }

                return value;
            }
            set
            {
                this.Attributes["fx"] = value;
            }
        }

        [SvgAttribute("fy")]
        public SvgUnit FocalY
        {
            get
            {
                var value = this.Attributes.GetAttribute<SvgUnit>("fy");

                if (value.IsEmpty || value.IsNone)
                {
                    value = this.CenterY;
                }

                return value;
            }
            set
            {
                this.Attributes["fy"] = value;
            }
        }

        public SvgRadialGradientServer()
        {
            CenterX = new SvgUnit(SvgUnitType.Percentage, 50F);
            CenterY = new SvgUnit(SvgUnitType.Percentage, 50F);
            Radius = new SvgUnit(SvgUnitType.Percentage, 50F);
        }

        public override Brush GetBrush(SvgVisualElement renderingElement, SvgRenderer renderer, float opacity)
        {
            LoadStops(renderingElement);

            try
            {
                if (this.GradientUnits == SvgCoordinateUnits.ObjectBoundingBox) renderer.Boundable(renderingElement);
                var origin = renderer.Boundable().Location;
                var centerPoint = CalculateCenterPoint(renderer, origin);
                var focalPoint = CalculateFocalPoint(renderer, origin);

                var specifiedRadius = CalculateRadius(renderer);
                var effectiveRadius = CalculateEffectiveRadius(renderingElement, centerPoint, specifiedRadius);

                var brush = new PathGradientBrush(CreateGraphicsPath(origin, centerPoint, effectiveRadius))
                {
                    InterpolationColors = CalculateColorBlend(renderer, opacity, specifiedRadius, effectiveRadius),
                    CenterPoint = focalPoint
                };

                Debug.Assert(brush.Rectangle.Contains(renderingElement.Bounds), "Brush rectangle does not contain rendering element bounds!");

                return brush;
            }
            finally
            {
                if (this.GradientUnits == SvgCoordinateUnits.ObjectBoundingBox) renderer.PopBoundable();
            }
        }

        private PointF CalculateCenterPoint(SvgRenderer renderer, PointF origin)
        {
            var deviceCenterX = origin.X + CenterX.ToDeviceValue(renderer, UnitRenderingType.HorizontalOffset, this);
            var deviceCenterY = origin.Y + CenterY.ToDeviceValue(renderer, UnitRenderingType.VerticalOffset, this);
            var transformedCenterPoint = TransformPoint(new PointF(deviceCenterX, deviceCenterY));
            return transformedCenterPoint;
        }

        private PointF CalculateFocalPoint(SvgRenderer renderer, PointF origin)
        {
            var deviceFocalX = origin.X + FocalX.ToDeviceValue(renderer, UnitRenderingType.HorizontalOffset, this);
            var deviceFocalY = origin.Y + FocalY.ToDeviceValue(renderer, UnitRenderingType.VerticalOffset, this);
            var transformedFocalPoint = TransformPoint(new PointF(deviceFocalX, deviceFocalY));
            return transformedFocalPoint;
        }

        private float CalculateRadius(SvgRenderer renderer)
        {
            var radius = Radius.ToDeviceValue(renderer, UnitRenderingType.Other, this);
            var transformRadiusVector = TransformVector(new PointF(radius, 0));
            var transformedRadius = CalculateLength(transformRadiusVector);
            return transformedRadius;
        }

        private float CalculateEffectiveRadius(ISvgBoundable boundable, PointF centerPoint, float specifiedRadius)
        {
            if (SpreadMethod != SvgGradientSpreadMethod.Pad)
            {
                return specifiedRadius;
            }

            var topLeft = new PointF(boundable.Bounds.Left, boundable.Bounds.Top);
            var topRight = new PointF(boundable.Bounds.Right, boundable.Bounds.Top);
            var bottomRight = new PointF(boundable.Bounds.Right, boundable.Bounds.Bottom);
            var bottomLeft = new PointF(boundable.Bounds.Left, boundable.Bounds.Bottom);

            var effectiveRadius = (float)Math.Ceiling(
                Math.Max(
                    Math.Max(
                        CalculateDistance(centerPoint, topLeft),
                        CalculateDistance(centerPoint, topRight)
                    ),
                    Math.Max(
                        CalculateDistance(centerPoint, bottomRight),
                        CalculateDistance(centerPoint, bottomLeft)
                    )
                )
            );

            effectiveRadius = Math.Max(effectiveRadius, specifiedRadius);

            return effectiveRadius;
        }

        private static GraphicsPath CreateGraphicsPath(PointF origin, PointF centerPoint, float effectiveRadius)
        {
            var path = new GraphicsPath();

            path.AddEllipse(
                origin.X + centerPoint.X - effectiveRadius,
                origin.Y + centerPoint.Y - effectiveRadius,
                effectiveRadius * 2,
                effectiveRadius * 2
            );

            return path;
        }

        private ColorBlend CalculateColorBlend(SvgRenderer renderer, float opacity, float specifiedRadius, float effectiveRadius)
        {
            var colorBlend = GetColorBlend(renderer, opacity, true);

            if (specifiedRadius >= effectiveRadius)
            {
                return colorBlend;
            }

            for (var i = 0; i < colorBlend.Positions.Length - 1; i++)
            {
                colorBlend.Positions[i] = 1 - (specifiedRadius / effectiveRadius) * (1 - colorBlend.Positions[i]);
            }

            colorBlend.Positions = new[] { 0F }.Concat(colorBlend.Positions).ToArray();
            colorBlend.Colors = new[] { colorBlend.Colors.First() }.Concat(colorBlend.Colors).ToArray();

            return colorBlend;
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgRadialGradientServer>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgRadialGradientServer;

            newObj.CenterX = this.CenterX;
            newObj.CenterY = this.CenterY;
            newObj.Radius = this.Radius;
            newObj.FocalX = this.FocalX;
            newObj.FocalY = this.FocalY;

            return newObj;
        }
    }
}