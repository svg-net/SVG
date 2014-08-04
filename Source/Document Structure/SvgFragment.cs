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
        /// Refers to the size of the font from baseline to baseline when multiple lines of text are set solid in a multiline layout environment.
        /// </summary>
        [SvgAttribute("font-size")]
        public virtual SvgUnit FontSize
        {
            get { return (this.Attributes["font-size"] == null) ? SvgUnit.Empty : (SvgUnit)this.Attributes["font-size"]; }
            set { this.Attributes["font-size"] = value; }
        }

        /// <summary>
        /// Indicates which font family is to be used to render the text.
        /// </summary>
        [SvgAttribute("font-family")]
        public virtual string FontFamily
        {
            get { return this.Attributes["font-family"] as string; }
            set { this.Attributes["font-family"] = value; }
        }

        /// <summary>
        /// Applies the required transforms to <see cref="SvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="SvgRenderer"/> to be transformed.</param>
        protected internal override bool PushTransforms(SvgRenderer renderer)
        {
            if (!base.PushTransforms(renderer)) return false;

            if (!this.ViewBox.Equals(SvgViewBox.Empty))
            {
                var width = this.Width.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this);
                var height = this.Height.ToDeviceValue(renderer, UnitRenderingType.Vertical, this);

                var fScaleX = width / this.ViewBox.Width;
                var fScaleY = height / this.ViewBox.Height;
                var fMinX = -this.ViewBox.MinX;
                var fMinY = -this.ViewBox.MinY;

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
                    float fMidX = width / 2;
                    float fMidY = height / 2;

                    switch (AspectRatio.Align)
                    {
                        case SvgPreserveAspectRatio.xMinYMin:
                            break;
                        case SvgPreserveAspectRatio.xMidYMin:
                            fMinX += fMidX - fViewMidX;
                            break;
                        case SvgPreserveAspectRatio.xMaxYMin:
                            fMinX += width - this.ViewBox.Width * fScaleX;
                            break;
                        case SvgPreserveAspectRatio.xMinYMid:
                            fMinY += fMidY - fViewMidY;
                            break;
                        case SvgPreserveAspectRatio.xMidYMid:
                            fMinX += fMidX - fViewMidX;
                            fMinY += fMidY - fViewMidY;
                            break;
                        case SvgPreserveAspectRatio.xMaxYMid:
                            fMinX += width - this.ViewBox.Width * fScaleX;
                            fMinY += fMidY - fViewMidY;
                            break;
                        case SvgPreserveAspectRatio.xMinYMax:
                            fMinY += height - this.ViewBox.Height * fScaleY;
                            break;
                        case SvgPreserveAspectRatio.xMidYMax:
                            fMinX += fMidX - fViewMidX;
                            fMinY += height - this.ViewBox.Height * fScaleY;
                            break;
                        case SvgPreserveAspectRatio.xMaxYMax:
                            fMinX += width - this.ViewBox.Width * fScaleX;
                            fMinY += height - this.ViewBox.Height * fScaleY;
                            break;
                        default:
                            break;
                    }
                }

                var x = _x.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this);
                var y = _y.ToDeviceValue(renderer, UnitRenderingType.Vertical, this);

                renderer.AddClip(new Region(new RectangleF(x, y, width, height)));
                renderer.ScaleTransform(fScaleX, fScaleY, MatrixOrder.Prepend);
                renderer.TranslateTransform(x,y);
                renderer.TranslateTransform(fMinX, fMinY);
            }

            return true;
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
            float w, h;
            var isWidthperc = Width.Type == SvgUnitType.Percentage;
            var isHeightperc = Height.Type == SvgUnitType.Percentage;

            RectangleF bounds = new RectangleF();
            if (isWidthperc || isHeightperc)
            {
                if (ViewBox.Width > 0 && ViewBox.Height > 0)
                {
                    bounds = new RectangleF(ViewBox.MinX, ViewBox.MinY, ViewBox.Width, ViewBox.Height);
                }
                else
                {
                    bounds = this.Bounds; //do just one call to the recursive bounds property
                }
            }

            if (isWidthperc) 
            {
                w = (bounds.Width + bounds.X) * (Width.Value * 0.01f);
            }
            else
            {
                w = Width.ToDeviceValue(null, UnitRenderingType.Horizontal, this);
            }
            if (isHeightperc) 
            {
                h = (bounds.Height + bounds.Y) * (Height.Value * 0.01f);
            }
            else 
            {
                h = Height.ToDeviceValue(null, UnitRenderingType.Vertical, this);
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