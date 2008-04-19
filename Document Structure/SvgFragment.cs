using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.ComponentModel;

namespace Svg
{
    /// <summary>
    /// An <see cref="SvgFragment"/> represents an SVG fragment that can be the root element or an embedded fragment of an SVG document.
    /// </summary>
    public class SvgFragment : SvgElement, ISvgViewPort
    {
        private SvgUnit _width;
        private SvgUnit _height;
        private SvgViewBox _viewBox;

        /// <summary>
        /// Gets the SVG namespace string.
        /// </summary>
        public static readonly Uri Namespace = new Uri("http://www.w3.org/2000/svg");

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        [SvgAttribute("width")]
        public SvgUnit Width
        {
            get { return this._width; }
            set { this._width = value; }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        [SvgAttribute("height")]
        public SvgUnit Height
        {
            get { return this._height; }
            set { this._height = value; }
        }

        /// <summary>
        /// Gets or sets the viewport of the element.
        /// </summary>
        /// <value></value>
        [SvgAttribute("viewBox")]
        public SvgViewBox ViewBox
        {
            get { return this._viewBox; }
            set { this._viewBox = value; }
        }

        /// <summary>
        /// Gets the name of the element.
        /// </summary>
        /// <value></value>
        protected override string ElementName
        {
            get { return "svg"; }
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="Graphics"/> object.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> object to render to.</param>
        protected override void Render(Graphics graphics)
        {
            Matrix oldTransform = null;

            if (!this.ViewBox.Equals(SvgViewBox.Empty))
            {
                oldTransform = graphics.Transform;
                Matrix viewBoxTransform = new Matrix();

                if (this.ViewBox.MinX > 0 || this.ViewBox.MinY > 0)
                {
                    viewBoxTransform.Translate(this.ViewBox.MinX, this.ViewBox.MinY, MatrixOrder.Append);
                }

                viewBoxTransform.Scale(this.Width.ToDeviceValue()/this.ViewBox.Width, this.Height.ToDeviceValue()/this.ViewBox.Height);

                graphics.Transform = viewBoxTransform;
            }

            base.Render(graphics);

            if (oldTransform != null)
            {
                graphics.Transform = oldTransform;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgFragment"/> class.
        /// </summary>
        public SvgFragment()
        {
            this._height = new SvgUnit(SvgUnitType.Percentage, 100.0f);
            this._width = 1000.0f;
            this.ViewBox = SvgViewBox.Empty;
        }
    }
}