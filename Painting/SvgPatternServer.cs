using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.ComponentModel;

using Svg.Transforms;

namespace Svg
{
    public sealed class SvgPatternServer : SvgPaintServer, ISvgViewPort
    {
        private SvgUnit _width;
        private SvgUnit _height;
        private SvgUnit _x;
        private SvgUnit _y;
        private SvgViewBox _viewBox;

        [SvgAttribute("viewBox")]
        public SvgViewBox ViewBox
        {
            get { return this._viewBox; }
            set { this._viewBox = value; }
        }

        [SvgAttribute("width")]
        public SvgUnit Width
        {
            get { return this._width; }
            set { this._width = value; }
        }

        [SvgAttribute("height")]
        public SvgUnit Height
        {
            get { return this._height; }
            set { this._height = value; }
        }

        [SvgAttribute("x")]
        public SvgUnit X
        {
            get { return this._x; }
            set { this._x = value; }
        }

        [SvgAttribute("y")]
        public SvgUnit Y
        {
            get { return this._y; }
            set { this._y = value; }
        }

        public SvgPatternServer()
        {
            this._x = new SvgUnit(0.0f);
            this._y = new SvgUnit(0.0f);
            this._width = new SvgUnit(0.0f);
            this._height = new SvgUnit(0.0f);
        }

        public override Brush GetBrush(SvgGraphicsElement renderingElement, float opacity)
        {
            // If there aren't any children, return null
            if (this.Children.Count == 0)
                return null;

            // Can't render if there are no dimensions
            if (this._width.Value == 0.0f || this._height.Value == 0.0f)
                return null;

            float width = this._width.ToDeviceValue(renderingElement);
            float height = this._height.ToDeviceValue(renderingElement, true);

            Bitmap image = new Bitmap((int)width, (int)height);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                Matrix patternMatrix = new Matrix();

                // Apply a translate if needed
                if (this._x.Value > 0.0f || this._y.Value > 0.0f)
                {
                    patternMatrix.Translate(this._x.ToDeviceValue(renderingElement) + -1.0f, this._y.ToDeviceValue(renderingElement, true) + -1.0f);
                }
                else
                {
                    patternMatrix.Translate(-1, -1);
                }

                if (this.ViewBox.Height > 0 || this.ViewBox.Width > 0)
                {
                    patternMatrix.Scale(this.Width.ToDeviceValue() / this.ViewBox.Width, this.Height.ToDeviceValue() / this.ViewBox.Height);
                }

                graphics.Transform = patternMatrix;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.PixelOffsetMode = PixelOffsetMode.Half;

                foreach (SvgElement child in this.Children)
                {
                    child.RenderElement(graphics);
                }

                graphics.Save();
            }

            TextureBrush textureBrush = new TextureBrush(image);

            return textureBrush;
        }
    }
}