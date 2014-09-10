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

        RectangleF ISvgBoundable.CalculateBounds()
        {
            return new RectangleF(PointF.Empty, GetDimensions());
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
        /// Applies the required transforms to <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to be transformed.</param>
        protected internal override bool PushTransforms(ISvgRenderer renderer)
        {
            if (!base.PushTransforms(renderer)) return false;
            this.ViewBox.AddViewBoxTransform(this.AspectRatio, renderer, this);
            return true;
        }

        protected override void Render(ISvgRenderer renderer)
        {
            switch (this.Overflow)
            {
                case SvgOverflow.auto:
                case SvgOverflow.visible:
                case SvgOverflow.scroll:
                    base.Render(renderer);
                    break;
                default:
                    var prevClip = renderer.GetClip();
                    try
                    {
                        var size = (this.Parent == null ? renderer.GetBoundable().Bounds.Size : GetDimensions());
                        var clip = new RectangleF(this.X.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this),
                                                  this.Y.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this),
                                                  size.Width, size.Height);
                        renderer.SetClip(new Region(clip), CombineMode.Intersect);
                        base.Render(renderer);
                    }
                    finally
                    {
                        renderer.SetClip(prevClip, CombineMode.Replace);
                    }
                    break;
            }
        }

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        /// <value></value>
        public GraphicsPath CreatePath()
        {
            var path = new GraphicsPath();

            AddPaths(this, path);

            return path;
        }

        /// <summary>
        /// Gets the bounds of the svg element.
        /// </summary>
        /// <returns>The bounds.</returns>
        public RectangleF CalculateBounds()
        {
            using (var path = CreatePath())
            {
                return path.GetBounds();
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
                    bounds = this.CalculateBounds(); //do just one call to the expensive bounds calculation method
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