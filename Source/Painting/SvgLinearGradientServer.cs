using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.ComponentModel;

namespace Svg
{
    [SvgElement("linearGradient")]
    public sealed class SvgLinearGradientServer : SvgGradientServer
    {
        private SvgUnit _x1;
        private SvgUnit _y1;
        private SvgUnit _x2;
        private SvgUnit _y2;

        [DefaultValue(typeof(SvgUnit), "0"), SvgAttribute("x1")]
        public SvgUnit X1
        {
            get { return this._x1; }
            set
            {
                this._x1 = value;
            }
        }

        [DefaultValue(typeof(SvgUnit), "0"), SvgAttribute("y1")]
        public SvgUnit Y1
        {
            get { return this._y1; }
            set
            {
                this._y1 = value;
            }
        }

        [DefaultValue(typeof(SvgUnit), "0"), SvgAttribute("x2")]
        public SvgUnit X2
        {
            get { return this._x2; }
            set
            {
                this._x2 = value;
            }
        }

        [DefaultValue(typeof(SvgUnit), "0"), SvgAttribute("y2")]
        public SvgUnit Y2
        {
            get { return this._y2; }
            set
            {
                this._y2 = value;
            }
        }

        public SvgLinearGradientServer()
        {
            this._x1 = new SvgUnit(0.0f);
            this._y1 = new SvgUnit(0.0f);
            this._x2 = new SvgUnit(0.0f);
            this._y2 = new SvgUnit(0.0f);
        }

        public SvgPoint Start
        {
            get { return new SvgPoint(this.X1, this.Y1); }
        }

        public SvgPoint End
        {
            get { return new SvgPoint(this.X2, this.Y2); }
        }

        public override Brush GetBrush(SvgVisualElement owner, float opacity)
        {
            // Need at least 2 colours to do the gradient fill
            if (this.Stops.Count < 2)
            {
                return null;
            }

            PointF start;
            PointF end;
            RectangleF bounds = (this.GradientUnits == SvgCoordinateUnits.ObjectBoundingBox) ? owner.Bounds : owner.OwnerDocument.GetDimensions();

            // Have start/end points been set? If not the gradient is horizontal
            if (!this.End.IsEmpty())
            {
                // Get the points to work out an angle
                if (this.Start.IsEmpty())
                {
                    start = bounds.Location;
                }
                else
                {
                    start = new PointF(this.Start.X.ToDeviceValue(owner), this.Start.Y.ToDeviceValue(owner, true));
                }

                float x = (this.End.X.IsEmpty) ? start.X : this.End.X.ToDeviceValue(owner);
                end = new PointF(x, this.End.Y.ToDeviceValue(owner, true));
            }
            else
            {
                // Default: horizontal
                start = bounds.Location;
                end = new PointF(bounds.Right, bounds.Top);
            }

            LinearGradientBrush gradient = new LinearGradientBrush(start, end, Color.Transparent, Color.Transparent);
            gradient.InterpolationColors = base.GetColourBlend(owner, opacity);

            // Needed to fix an issue where the gradient was being wrapped when though it had the correct bounds
            gradient.WrapMode = WrapMode.TileFlipX;
            return gradient;
        }
    }
}