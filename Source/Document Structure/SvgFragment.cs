using System;
using System.Drawing;
using System.Drawing.Drawing2D;

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
        private SvgAspectRatio _aspect;

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

        [SvgAttribute("overflow")]
        public virtual SvgOverflow Overflow
        {
            get { return this.Attributes.GetAttribute<SvgOverflow>("overflow"); }
            set { this.Attributes["overflow"] = value; }
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
        /// Gets or sets the aspect of the viewport.
        /// </summary>
        /// <value></value>
        [SvgAttribute("preserveAspectRatio")]
        public SvgAspectRatio AspectRatio
        {
            get { return this._aspect; }
            set { this._aspect = value; }
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
                renderer.TranslateTransform(-this.ViewBox.MinX, -this.ViewBox.MinY, MatrixOrder.Append);

                renderer.ScaleTransform(this.Width.ToDeviceValue() / this.ViewBox.Width, this.Height.ToDeviceValue() / this.ViewBox.Height, MatrixOrder.Append);
            }
        }
        
        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        /// <value></value>
        public GraphicsPath Path
        {
            get 
            { 
                var path = new GraphicsPath();

                AddPaths(this, path);
  
                return path;
            }
        }
        
        /// <summary>
        /// Gets the bounds of the svg element.
        /// </summary>
        /// <value>The bounds.</value>
        public RectangleF Bounds 
        { 
            get
            {
                return this.Path.GetBounds();
            }
                
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgFragment"/> class.
        /// </summary>
        public SvgFragment()
        {
            this._height = new SvgUnit(SvgUnitType.Percentage, 100.0f);
            this._width = new SvgUnit(SvgUnitType.Percentage, 100.0f);
            this.ViewBox = SvgViewBox.Empty;
            this.AspectRatio = new SvgAspectRatio(SvgPreserveAspectRatio.None);
        }


        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgFragment>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgFragment;
            newObj.Height = this.Height;
            newObj.Width = this.Width;
            newObj.Overflow = this.Overflow;
            newObj.ViewBox = this.ViewBox;
            newObj.AspectRatio = this.AspectRatio;
            return newObj;
        }
    }
}