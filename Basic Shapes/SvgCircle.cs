using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Svg
{
    /// <summary>
    /// An SVG element to render circles to the document.
    /// </summary>
    public class SvgCircle : SvgGraphicsElement
    {
        private GraphicsPath _path;
        private SvgUnit _cx;
        private SvgUnit _cy;
        private SvgUnit _radius;

        /// <summary>
        /// Gets the center point of the circle.
        /// </summary>
        /// <value>The center.</value>
        public SvgPoint Center
        {
            get { return new SvgPoint(this.CenterX, this.CenterY); }
        }

        /// <summary>
        /// Gets or sets the center X co-ordinate.
        /// </summary>
        /// <value>The center X.</value>
        [SvgAttribute("cx")]
        public SvgUnit CenterX
        {
            get { return this._cx; }
            set
            {
                this._cx = value;
                this.IsPathDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the center Y co-ordinate.
        /// </summary>
        /// <value>The center Y.</value>
        [SvgAttribute("cy")]
        public SvgUnit CenterY
        {
            get { return this._cy; }
            set
            {
                this._cy = value;
                this.IsPathDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the radius of the circle.
        /// </summary>
        /// <value>The radius.</value>
        [SvgAttribute("r")]
        public SvgUnit Radius
        {
            get { return this._radius; }
            set
            {
                this._radius = value;
                this.IsPathDirty = true;
            }
        }

        /// <summary>
        /// Gets the name of the element.
        /// </summary>
        /// <value></value>
        protected override string ElementName
        {
            get { return "circle"; }
        }

        /// <summary>
        /// Gets the bounds of the circle.
        /// </summary>
        /// <value>The bounds.</value>
        public override RectangleF Bounds
        {
            get { return this.Path.GetBounds(); }
        }

        /// <summary>
        /// Gets a value indicating whether the circle requires anti-aliasing when being rendered.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the circle requires anti-aliasing; otherwise, <c>false</c>.
        /// </value>
        protected override bool RequiresSmoothRendering
        {
            get { return true; }
        }

        public override GraphicsPath Path
        {
            get
            {
                if (this._path == null || this.IsPathDirty)
                {
                    _path = new GraphicsPath();
                    _path.StartFigure();
                    _path.AddEllipse(this.Center.ToDeviceValue().X - this.Radius.ToDeviceValue(), this.Center.ToDeviceValue().Y - this.Radius.ToDeviceValue(), 2 * this.Radius.ToDeviceValue(), 2 * this.Radius.ToDeviceValue());
                    _path.CloseFigure();
                    this.IsPathDirty = false;
                }
                return _path;
            }
        }

        /// <summary>
        /// Renders the circle to the specified <see cref="Graphics"/> object.
        /// </summary>
        /// <param name="graphics">The graphics object.</param>
        protected override void Render(Graphics graphics)
        {
            // Don't draw if there is no radius set
            if (this.Radius.Value > 0.0f)
            {
                base.Render(graphics);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgCircle"/> class.
        /// </summary>
        public SvgCircle()
        {
            
        }
    }
}