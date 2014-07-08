using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    /// <summary>
    /// An <see cref="SvgFragment"/> represents an SVG fragment that can be the root element or an embedded fragment of an SVG document.
    /// </summary>
    [SvgElement("svg")]
    public class SvgFragment : SvgElement, ISvgViewPort, ISvgBoundable
    {
        /// <summary>
        /// Gets the SVG namespace string.
        /// </summary>
        public static readonly Uri Namespace = new Uri("http://www.w3.org/2000/svg");

        PointF ISvgBoundable.Location
        {
            get
            {
                return PointF.Empty;
            }
        }

        SizeF ISvgBoundable.Size
        {
            get
            {
                return GetDimensions();
            }
        }

        RectangleF ISvgBoundable.Bounds
        {
            get
            {
                return new RectangleF(((ISvgBoundable)this).Location, ((ISvgBoundable)this).Size);
            }
        }

        private SvgUnit _x;
        private SvgUnit _y;
        
        /// <summary>
        /// Gets or sets the position where the left point of the svg should start.
        /// </summary>
        [SvgAttribute("x")]
        public SvgUnit X
        {
            get { return _x; }
            set
            {
                if(_x != value)
                {
                    _x = value;
                    OnAttributeChanged(new AttributeEventArgs{ Attribute = "x", Value = value });
                }
            }
        }

        /// <summary>
        /// Gets or sets the position where the top point of the svg should start.
        /// </summary>
        [SvgAttribute("y")]
        public SvgUnit Y
        {
            get { return _y; }
            set
            {
                if(_y != value)
                {
                    _y = value;
                    OnAttributeChanged(new AttributeEventArgs{ Attribute = "y", Value = value });
                }
            }
        }

        /// <summary>
        /// Gets or sets the width of the fragment.
        /// </summary>
        /// <value>The width.</value>
        [SvgAttribute("width")]
        public SvgUnit Width
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("width"); }
            set { this.Attributes["width"] = value; }
        }

        /// <summary>
        /// Gets or sets the height of the fragment.
        /// </summary>
        /// <value>The height.</value>
        [SvgAttribute("height")]
        public SvgUnit Height
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("height"); }
            set { this.Attributes["height"] = value; }
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
            get { return this.Attributes.GetAttribute<SvgViewBox>("viewBox"); }
            set { this.Attributes["viewBox"] = value; }
        }
        
        /// <summary>
        /// Gets or sets the aspect of the viewport.
        /// </summary>
        /// <value></value>
        [SvgAttribute("preserveAspectRatio")]
        public SvgAspectRatio AspectRatio
        {
            get { return this.Attributes.GetAttribute<SvgAspectRatio>("preserveAspectRatio"); }
            set { this.Attributes["preserveAspectRatio"] = value; }
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
                float fScaleX = this.Width.ToDeviceValue(this, false) / this.ViewBox.Width;
                float fScaleY = this.Height.ToDeviceValue(this, true) / this.ViewBox.Height;
                float fMinX = -this.ViewBox.MinX;
                float fMinY = -this.ViewBox.MinY;

                if (AspectRatio.Align != SvgPreserveAspectRatio.none)
                {
                    if (AspectRatio.Slice)
                    {
                        fScaleX = Math.Max(fScaleX, fScaleY);
                        fScaleY = Math.Max(fScaleX, fScaleY);
                    }
                    else
                    {
                        fScaleX = Math.Min(fScaleX, fScaleY);
                        fScaleY = Math.Min(fScaleX, fScaleY);
                    }
                    float fViewMidX = (this.ViewBox.Width / 2) * fScaleX;
                    float fViewMidY = (this.ViewBox.Height / 2) * fScaleY;
                    float fMidX = this.Width.ToDeviceValue(this, false) / 2;
                    float fMidY = this.Height.ToDeviceValue(this, true) / 2;

                    switch (AspectRatio.Align)
                    {
                        case SvgPreserveAspectRatio.xMinYMin:
                            break;
                        case SvgPreserveAspectRatio.xMidYMin:
                            fMinX += (fMidX - fViewMidX) / fScaleX;
                            break;
                        case SvgPreserveAspectRatio.xMaxYMin:
                            fMinX += (this.Width.ToDeviceValue(this, false) / fScaleX) - this.ViewBox.Width;
                            break;
                        case SvgPreserveAspectRatio.xMinYMid:
                            fMinY += (fMidY - fViewMidY) / fScaleY;
                            break;
                        case SvgPreserveAspectRatio.xMidYMid:
                            fMinX += (fMidX - fViewMidX) / fScaleX;
                            fMinY += (fMidY - fViewMidY) / fScaleY;
                            break;
                        case SvgPreserveAspectRatio.xMaxYMid:
                            fMinX += (this.Width.ToDeviceValue(this, false) / fScaleX) - this.ViewBox.Width;
                            fMinY += (fMidY - fViewMidY) / fScaleY;
                            break;
                        case SvgPreserveAspectRatio.xMinYMax:
                            fMinY += (this.Height.ToDeviceValue(this, true) / fScaleY) - this.ViewBox.Height;
                            break;
                        case SvgPreserveAspectRatio.xMidYMax:
                            fMinX += (fMidX - fViewMidX) / fScaleX;
                            fMinY += (this.Height.ToDeviceValue(this, true) / fScaleY) - this.ViewBox.Height;
                            break;
                        case SvgPreserveAspectRatio.xMaxYMax:
                            fMinX += (this.Width.ToDeviceValue(this, false) / fScaleX) - this.ViewBox.Width;
                            fMinY += (this.Height.ToDeviceValue(this, true) / fScaleY) - this.ViewBox.Height;
                            break;
                        default:
                            break;
                    }
                }

                renderer.TranslateTransform(_x, _y);
                renderer.TranslateTransform(fMinX, fMinY);
                renderer.ScaleTransform(fScaleX, fScaleY);
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
            _x = 0.0f;
            _y = 0.0f;
            this.Height = new SvgUnit(SvgUnitType.Percentage, 100.0f);
            this.Width = new SvgUnit(SvgUnitType.Percentage, 100.0f);
            this.ViewBox = SvgViewBox.Empty;
            this.AspectRatio = new SvgAspectRatio(SvgPreserveAspectRatio.xMidYMid);
        }

        public SizeF GetDimensions()
        {
            var w = Width.ToDeviceValue();
            var h = Height.ToDeviceValue();

            RectangleF bounds = new RectangleF();
            var isWidthperc = Width.Type == SvgUnitType.Percentage;
            var isHeightperc = Height.Type == SvgUnitType.Percentage;

            if (isWidthperc || isHeightperc)
            {
                bounds = this.Bounds; //do just one call to the recursive bounds property
                if (isWidthperc) w = (bounds.Width + bounds.X) * (w * 0.01f);
                if (isHeightperc) h = (bounds.Height + bounds.Y) * (h * 0.01f);
            }

            return new SizeF(w, h);
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