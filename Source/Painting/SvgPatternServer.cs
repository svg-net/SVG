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
    public sealed class SvgPatternServer : SvgPaintServer, ISvgViewPort, ISvgSupportsCoordinateUnits
    {
        private SvgUnit _width;
        private SvgUnit _height;
        private SvgUnit _x;
        private SvgUnit _y;
        private SvgViewBox _viewBox;
        private SvgCoordinateUnits _patternUnits = SvgCoordinateUnits.ObjectBoundingBox;
        private SvgCoordinateUnits _patternContentUnits = SvgCoordinateUnits.UserSpaceOnUse;

		[SvgAttribute("overflow")]
		public SvgOverflow Overflow
		{
			get { return this.Attributes.GetAttribute<SvgOverflow>("overflow"); }
			set { this.Attributes["overflow"] = value; }
		}


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
        /// Gets or sets the aspect of the viewport.
        /// </summary>
        /// <value></value>
        [SvgAttribute("preserveAspectRatio")]
        public SvgAspectRatio AspectRatio 
		{
			get;
			set;
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
        /// Gets or sets the width of the pattern.
        /// </summary>
        [SvgAttribute("patternUnits")]
        public SvgCoordinateUnits PatternUnits
        {
            get { return this._patternUnits; }
            set { this._patternUnits = value; }
        }

        /// <summary>
        /// Gets or sets the width of the pattern.
        /// </summary>
        [SvgAttribute("patternUnits")]
        public SvgCoordinateUnits PatternContentUnits
        {
            get { return this._patternContentUnits; }
            set { this._patternContentUnits = value; }
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
        /// <param name="renderingElement">The owner <see cref="SvgVisualElement"/>.</param>
        /// <param name="opacity">The opacity of the brush.</param>
        public override Brush GetBrush(SvgVisualElement renderingElement, ISvgRenderer renderer, float opacity)
        {
            // If there aren't any children, return null
            if (this.Children.Count == 0)
                return null;

            // Can't render if there are no dimensions
            if (this._width.Value == 0.0f || this._height.Value == 0.0f)
                return null;

            try
            {
                if (this.PatternUnits == SvgCoordinateUnits.ObjectBoundingBox) renderer.SetBoundable(renderingElement);

                float width = this._width.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this);
                float height = this._height.ToDeviceValue(renderer, UnitRenderingType.Vertical, this);

                Matrix patternMatrix = new Matrix();
                // Apply a translate if needed
                if (this._x.Value > 0.0f || this._y.Value > 0.0f)
                {
                    float x = this._x.ToDeviceValue(renderer, UnitRenderingType.HorizontalOffset, this);
                    float y = this._y.ToDeviceValue(renderer, UnitRenderingType.VerticalOffset, this);

                    patternMatrix.Translate(x, y);
                }

                if (this.ViewBox.Height > 0 || this.ViewBox.Width > 0)
                {
                    patternMatrix.Scale(this.Width.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this) / this.ViewBox.Width,
                                        this.Height.ToDeviceValue(renderer, UnitRenderingType.Vertical, this) / this.ViewBox.Height);
                }

                Bitmap image = new Bitmap((int)width, (int)height);
                using (var iRenderer = SvgRenderer.FromImage(image))
                {
                    iRenderer.SetBoundable((_patternContentUnits == SvgCoordinateUnits.ObjectBoundingBox) ? new GenericBoundable(0, 0, width, height) : renderer.GetBoundable());
                    iRenderer.Transform = patternMatrix;
                    iRenderer.SmoothingMode = SmoothingMode.AntiAlias;
                    
                    foreach (SvgElement child in this.Children)
                    {
                        child.RenderElement(iRenderer);
                    }
                }

                image.Save(string.Format(@"C:\test{0:D3}.png", imgNumber++));

                TextureBrush textureBrush = new TextureBrush(image);

                return textureBrush;
            }
            finally
            {
                if (this.PatternUnits == SvgCoordinateUnits.ObjectBoundingBox) renderer.PopBoundable();
            }
        }

        private static int imgNumber = 0;



		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgPatternServer>();
		}


		public override SvgElement DeepCopy<T>()
		{
			var newObj = base.DeepCopy<T>() as SvgPatternServer;
			newObj.Overflow = this.Overflow;
			newObj.ViewBox = this.ViewBox;
			newObj.AspectRatio = this.AspectRatio;
			newObj.X = this.X;
			newObj.Y = this.Y;
			newObj.Width = this.Width;
			newObj.Height = this.Height;
			return newObj;

		}

        public SvgCoordinateUnits GetUnits()
        {
            return _patternUnits;
        }
    }
}