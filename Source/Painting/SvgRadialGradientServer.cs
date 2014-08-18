using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;
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

        private object _lockObj = new Object();

        public override Brush GetBrush(SvgVisualElement renderingElement, ISvgRenderer renderer, float opacity)
        {
            LoadStops(renderingElement);

            try
            {
                if (this.GradientUnits == SvgCoordinateUnits.ObjectBoundingBox) renderer.SetBoundable(renderingElement);
                
                // Calculate the path and transform it appropriately
                var origin = renderer.GetBoundable().Location;
                var center = new PointF(origin.X + CenterX.ToDeviceValue(renderer, UnitRenderingType.HorizontalOffset, this),
                                         origin.Y + CenterY.ToDeviceValue(renderer, UnitRenderingType.VerticalOffset, this));
                var specifiedRadius = Radius.ToDeviceValue(renderer, UnitRenderingType.Other, this);
                var path = new GraphicsPath();
                path.AddEllipse(
                    origin.X + center.X - specifiedRadius, origin.Y + center.Y - specifiedRadius,
                    specifiedRadius * 2, specifiedRadius * 2
                );
                path.Transform(EffectiveGradientTransform);


                // Calculate any required scaling
                var scale = CalcScale(renderingElement.Bounds, path);

                // Get the color blend and any tweak to the scaling
                var blend = CalculateColorBlend(renderer, opacity, scale, out scale);

                // Transform the path based on the scaling
                var gradBounds = path.GetBounds();
                var transCenter = new PointF(gradBounds.Left + gradBounds.Width / 2, gradBounds.Top + gradBounds.Height / 2);
                using (var scaleMat = new Matrix())
                {
                    scaleMat.Translate(-1 * transCenter.X, -1 * transCenter.Y, MatrixOrder.Append);
                    scaleMat.Scale(scale, scale, MatrixOrder.Append);
                    scaleMat.Translate(transCenter.X, transCenter.Y, MatrixOrder.Append);
                    path.Transform(scaleMat);
                }

                // calculate the brush
                var brush = new PathGradientBrush(path);
                brush.CenterPoint = CalculateFocalPoint(renderer, origin);
                brush.InterpolationColors = blend;

                return brush;
            }
            finally
            {
                if (this.GradientUnits == SvgCoordinateUnits.ObjectBoundingBox) renderer.PopBoundable();
            }
        }

        /// <summary>
        /// Determine how much (approximately) the path must be scaled to contain the rectangle
        /// </summary>
        /// <param name="bounds">Bounds that the path must contain</param>
        /// <param name="path">Path of the gradient</param>
        /// <returns>Scale factor</returns>
        /// <remarks>
        /// This method continually transforms the rectangle (fewer points) until it is contained by the path
        /// and returns the result of the search.  The scale factor is set to a constant 95%
        /// </remarks>
        private float CalcScale(RectangleF bounds, GraphicsPath path)
        {
            var points = new PointF[] {
                new PointF(bounds.Left, bounds.Top), 
                new PointF(bounds.Right, bounds.Top), 
                new PointF(bounds.Right, bounds.Bottom), 
                new PointF(bounds.Left, bounds.Bottom) 
            };
            var pathBounds = path.GetBounds();
            var pathCenter = new PointF(pathBounds.X + pathBounds.Width / 2, pathBounds.Y + pathBounds.Height / 2);
            using (var transform = new Matrix())
            {
                transform.Translate(-1 * pathCenter.X, -1 * pathCenter.Y, MatrixOrder.Append);
                transform.Scale(.95f, .95f, MatrixOrder.Append);
                transform.Translate(pathCenter.X, pathCenter.Y, MatrixOrder.Append);

                var boundsTest = RectangleF.Inflate(bounds, 0, 0);
                while (!(path.IsVisible(points[0]) && path.IsVisible(points[1]) &&
                         path.IsVisible(points[2]) && path.IsVisible(points[3])))
                {
                    transform.TransformPoints(points);
                }
            }
            return bounds.Height / (points[2].Y - points[1].Y);
        }

        private PointF CalculateFocalPoint(ISvgRenderer renderer, PointF origin)
        {
            var deviceFocalX = origin.X + FocalX.ToDeviceValue(renderer, UnitRenderingType.HorizontalOffset, this);
            var deviceFocalY = origin.Y + FocalY.ToDeviceValue(renderer, UnitRenderingType.VerticalOffset, this);
            var transformedFocalPoint = TransformPoint(new PointF(deviceFocalX, deviceFocalY));
            return transformedFocalPoint;
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

        private ColorBlend CalculateColorBlend(ISvgRenderer renderer, float opacity, float scale, out float outScale)
        {
            var colorBlend = GetColorBlend(renderer, opacity, true);
            float newScale;
            List<float> pos;
            List<Color> colors;

            outScale = scale;
            if (scale > 1)
            {
                switch (this.SpreadMethod)
                {
                    case SvgGradientSpreadMethod.Reflect:
                        newScale = (float)Math.Ceiling(scale);
                        pos = (from p in colorBlend.Positions select p / newScale).ToList();
                        colors = colorBlend.Colors.ToList();

                        for (var i = 1; i < newScale; i++)
                        {
                            if (i % 2 == 1)
                            {
                                pos.AddRange(from p in colorBlend.Positions.Reverse().Skip(1) select (1 - p + i) / newScale);
                                colors.AddRange(colorBlend.Colors.Reverse().Skip(1));
                            }
                            else
                            {
                                pos.AddRange(from p in colorBlend.Positions.Skip(1) select (p + i) / newScale);
                                colors.AddRange(colorBlend.Colors.Skip(1));
                            }
                        }

                        colorBlend.Positions = pos.ToArray();
                        colorBlend.Colors = colors.ToArray();
                        outScale = newScale;
                        break;
                    case SvgGradientSpreadMethod.Repeat:
                        newScale = (float)Math.Ceiling(scale);
                        pos = (from p in colorBlend.Positions select p / newScale).ToList();
                        colors = colorBlend.Colors.ToList();

                        for (var i = 1; i < newScale; i++)
                        {
                            pos.AddRange(from p in colorBlend.Positions select (p <= 0 ? 0.001f : p) / newScale);
                            colors.AddRange(colorBlend.Colors);
                        }

                        break;
                    default:
                        for (var i = 0; i < colorBlend.Positions.Length - 1; i++)
                        {
                            colorBlend.Positions[i] = 1 - (1 - colorBlend.Positions[i]) / scale;
                        }

                        colorBlend.Positions = new[] { 0F }.Concat(colorBlend.Positions).ToArray();
                        colorBlend.Colors = new[] { colorBlend.Colors.First() }.Concat(colorBlend.Colors).ToArray();

                        break;
                }
            }

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