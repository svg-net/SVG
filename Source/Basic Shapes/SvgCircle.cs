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
    [SvgElement("circle")]
    public class SvgCircle : SvgVisualElement
    {
        private GraphicsPath _path;

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
            get { return this.Attributes.GetAttribute<SvgUnit>("cx"); }
            set
            {
                if (this.Attributes.GetAttribute<SvgUnit>("cx") != value)
                {
                    this.Attributes["cx"] = value;
                    this.IsPathDirty = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the center Y co-ordinate.
        /// </summary>
        /// <value>The center Y.</value>
        [SvgAttribute("cy")]
        public SvgUnit CenterY
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("cy"); }
            set
            {
                if (this.Attributes.GetAttribute<SvgUnit>("cy") != value)
                {
                    this.Attributes["cy"] = value;
                    this.IsPathDirty = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the radius of the circle.
        /// </summary>
        /// <value>The radius.</value>
        [SvgAttribute("r")]
        public SvgUnit Radius
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("r"); }
            set
            {
                if (this.Attributes.GetAttribute<SvgUnit>("r") != value)
                {
                    this.Attributes["r"] = value;
                    this.IsPathDirty = true;
                }
            }
        }

        /// <summary>
        /// Gets the bounds of the circle.
        /// </summary>
        /// <value>The rectangular bounds of the circle.</value>
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

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> representing this element.
        /// </summary>
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
        protected override void Render(SvgRenderer renderer)
        {
            // Don't draw if there is no radius set
            if (this.Radius.Value > 0.0f)
            {
                base.Render(renderer);
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