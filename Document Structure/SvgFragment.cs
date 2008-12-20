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
    [SvgElement("svg")]
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
        /// Gets or sets the width of the fragment.
        /// </summary>
        /// <value>The width.</value>
        [SvgAttribute("width")]
        public SvgUnit Width
        {
            get { return this._width; }
            set { this._width = value; }
        }

        /// <summary>
        /// Gets or sets the height of the fragment.
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
        /// Applies the required transforms to <see cref="SvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="SvgRenderer"/> to be transformed.</param>
        protected internal override void PushTransforms(SvgRenderer renderer)
        {
            base.PushTransforms(renderer);

            if (!this.ViewBox.Equals(SvgViewBox.Empty))
            {
                if (this.ViewBox.MinX > 0 || this.ViewBox.MinY > 0)
                {
                    renderer.TranslateTransform(this.ViewBox.MinX, this.ViewBox.MinY, MatrixOrder.Append);
                }

                renderer.ScaleTransform(this.Width.ToDeviceValue() / this.ViewBox.Width, this.Height.ToDeviceValue() / this.ViewBox.Height, MatrixOrder.Append);
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