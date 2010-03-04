using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;
using System.ComponentModel;

namespace Svg
{
    /// <summary>
    /// Represents and SVG ellipse element.
    /// </summary>
    [SvgElement("ellipse")]
    public class SvgEllipse : SvgVisualElement
    {
        private SvgUnit _radiusX;
        private SvgUnit _radiusY;
        private SvgUnit _centerX;
        private SvgUnit _centerY;
        private GraphicsPath _path;

        [SvgAttribute("cx")]
        public virtual SvgUnit CenterX
        {
            get { return this._centerX; }
            set
            {
                this._centerX = value;
                this.IsPathDirty = true;
            }
        }

        [SvgAttribute("cy")]
        public virtual SvgUnit CenterY
        {
            get { return this._centerY; }
            set
            {
                this._centerY = value;
                this.IsPathDirty = true;
            }
        }

        [SvgAttribute("rx")]
        public virtual SvgUnit RadiusX
        {
            get { return this._radiusX; }
            set { this._radiusX = value; this.IsPathDirty = true; }
        }

        [SvgAttribute("ry")]
        public virtual SvgUnit RadiusY
        {
            get { return this._radiusY; }
            set { this._radiusY = value; this.IsPathDirty = true; }
        }

        /// <summary>
        /// Gets or sets a value to determine if anti-aliasing should occur when the element is being rendered.
        /// </summary>
        /// <value></value>
        protected override bool RequiresSmoothRendering
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        /// <value>The bounds.</value>
        public override RectangleF Bounds
        {
            get { return this.Path.GetBounds(); }
        }

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        /// <value></value>
        public override GraphicsPath Path
        {
            get
            {
                if (this._path == null || this.IsPathDirty)
                {
                    PointF center = new PointF(this._centerX.ToDeviceValue(this), this._centerY.ToDeviceValue(this, true));
                    PointF radius = new PointF(this._radiusX.ToDeviceValue(this), this._radiusY.ToDeviceValue(this, true));

                    this._path = new GraphicsPath();
                    _path.StartFigure();
                    _path.AddEllipse(center.X - radius.X, center.Y - radius.Y, 2 * radius.X, 2 * radius.Y);
                    _path.CloseFigure();
                    this.IsPathDirty = false;
                }
                return _path;
            }
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="Graphics"/> object.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> object to render to.</param>
        protected override void Render(SvgRenderer renderer)
        {
            if (this._radiusX.Value > 0.0f && this._radiusY.Value > 0.0f)
            {
                base.Render(renderer);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgEllipse"/> class.
        /// </summary>
        public SvgEllipse()
        {
        }
    }
}