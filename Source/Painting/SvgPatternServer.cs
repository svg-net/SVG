using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.ComponentModel;

using Svg.Transforms;

namespace Svg
{
    /// <summary>
    /// A pattern is used to fill or stroke an object using a pre-defined graphic object which can be replicated ("tiled") at fixed intervals in x and y to cover the areas to be painted.
    /// </summary>
    [SvgElement("pattern")]
    public sealed class SvgPatternServer : SvgPaintServer, ISvgViewPort
    {
        private SvgUnit _width;
        private SvgUnit _height;
        private SvgUnit _x;
        private SvgUnit _y;
        private SvgViewBox _viewBox;

        /// <summary>
        /// Specifies a supplemental transformation which is applied on top of any 
        /// transformations necessary to create a new pattern coordinate system.
        /// </summary>
        [SvgAttribute("viewBox")]
        public SvgViewBox ViewBox
        {
            get { return this._viewBox; }
            set { this._viewBox = value; }
        }

        /// <summary>
        /// Gets or sets the width of the pattern.
        /// </summary>
        [SvgAttribute("width")]
        public SvgUnit Width
        {
            get { return this._width; }
            set { this._width = value; }
        }

        /// <summary>
        /// Gets or sets the height of the pattern.
        /// </summary>
        [SvgAttribute("height")]
        public SvgUnit Height
        {
            get { return this._height; }
            set { this._height = value; }
        }

        /// <summary>
        /// Gets or sets the X-axis location of the pattern.
        /// </summary>
        [SvgAttribute("x")]
        public SvgUnit X
        {
            get { return this._x; }
            set { this._x = value; }
        }

        /// <summary>
        /// Gets or sets the Y-axis location of the pattern.
        /// </summary>
        [SvgAttribute("y")]
        public SvgUnit Y
        {
            get { return this._y; }
            set { this._y = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgPatternServer"/> class.
        /// </summary>
        public SvgPatternServer()
        {
            this._x = new SvgUnit(0.0f);
            this._y = new SvgUnit(0.0f);
            this._width = new SvgUnit(0.0f);
            this._height = new SvgUnit(0.0f);
        }

        /// <summary>
        /// Gets a <see cref="Brush"/> representing the current paint server.
        /// </summary>
        /// <param name="styleOwner">The owner <see cref="SvgVisualElement"/>.</param>
        /// <param name="opacity">The opacity of the brush.</param>
        public override Brush GetBrush(SvgVisualElement renderingElement, float opacity)
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
            using (SvgRenderer renderer = SvgRenderer.FromImage(image))
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

                renderer.Transform = patternMatrix;
                renderer.CompositingQuality = CompositingQuality.HighQuality;
                renderer.SmoothingMode = SmoothingMode.AntiAlias;
                renderer.PixelOffsetMode = PixelOffsetMode.Half;

                foreach (SvgElement child in this.Children)
                {
                    child.RenderElement(renderer);
                }

                renderer.Save();
            }

            TextureBrush textureBrush = new TextureBrush(image);

            return textureBrush;
        }
    }
}