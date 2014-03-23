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
            get
            {
                var value = this.Attributes.GetAttribute<SvgUnit>("fx");

                if (value.IsEmpty || value.IsNone)
                {
                    value = this.CenterX;
                }

                return value;
            }

            set { this.Attributes["fx"] = value; }
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

            set { this.Attributes["fy"] = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgRadialGradientServer"/> class.
        /// </summary>
        public SvgRadialGradientServer()
        {
            //Apply default values of 50% to cX,cY and r
            CenterX = new SvgUnit(SvgUnitType.Percentage, 50);
            CenterY = new SvgUnit(SvgUnitType.Percentage, 50);
            Radius = new SvgUnit(SvgUnitType.Percentage, 50);
        }

        public override Brush GetBrush(SvgVisualElement renderingElement, float opacity)
        {
            float radius = this.Radius.ToDeviceValue(renderingElement);

            if (radius <= 0)
            {
                return null;
            }
            
            GraphicsPath path = new GraphicsPath();
            float left = this.CenterX.ToDeviceValue(renderingElement);
            float top = this.CenterY.ToDeviceValue(renderingElement, true);
            
            RectangleF boundingBox = (this.GradientUnits == SvgCoordinateUnits.ObjectBoundingBox) ? renderingElement.Bounds : renderingElement.OwnerDocument.GetDimensions();

            path.AddEllipse(
                boundingBox.Left + left - radius,
                boundingBox.Top + top - radius,
                radius * 2,
                radius * 2);

            PathGradientBrush brush = new PathGradientBrush(path);
            ColorBlend blend = base.GetColourBlend(renderingElement, opacity);

            brush.InterpolationColors = blend;
            brush.CenterPoint =
                new PointF(
                    boundingBox.Left + this.FocalX.ToDeviceValue(renderingElement),
                    boundingBox.Top + this.FocalY.ToDeviceValue(renderingElement, true));

            return brush;
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