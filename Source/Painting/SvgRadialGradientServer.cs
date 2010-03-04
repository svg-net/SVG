using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    [SvgElement("radialGradient")]
    public sealed class SvgRadialGradientServer : SvgGradientServer
    {
        [SvgAttribute("cx")]
        public SvgUnit CenterX
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("cx"); }
            set { this.Attributes["cx"] = value; }
        }

        [SvgAttribute("cy")]
        public SvgUnit CenterY
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("cy"); }
            set { this.Attributes["cy"] = value; }
        }

        [SvgAttribute("r")]
        public SvgUnit Radius
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("r"); }
            set { this.Attributes["r"] = value; }
        }

        [SvgAttribute("fx")]
        public SvgUnit FocalX
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("fx"); }
            set { this.Attributes["fx"] = value; }
        }

        [SvgAttribute("fy")]
        public SvgUnit FocalY
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("fy"); }
            set { this.Attributes["fy"] = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgRadialGradientServer"/> class.
        /// </summary>
        public SvgRadialGradientServer()
        {
        }

        public override Brush GetBrush(SvgVisualElement renderingElement, float opacity)
        {
            GraphicsPath path = new GraphicsPath();
            float left = this.CenterX.ToDeviceValue(renderingElement);
            float top = this.CenterY.ToDeviceValue(renderingElement, true);
            float radius = this.Radius.ToDeviceValue(renderingElement);
            RectangleF boundingBox = (this.GradientUnits == SvgCoordinateUnits.ObjectBoundingBox) ? renderingElement.Bounds : renderingElement.OwnerDocument.GetDimensions();

            path.AddEllipse(left-radius, top-radius, radius*2, radius*2);

            PathGradientBrush brush = new PathGradientBrush(path);
            ColorBlend blend = base.GetColourBlend(renderingElement, opacity);

            brush.InterpolationColors = blend;
            brush.CenterPoint = new PointF(this.FocalX.ToDeviceValue(renderingElement), this.FocalY.ToDeviceValue(renderingElement, true));
          
            return brush;
        }
    }
}