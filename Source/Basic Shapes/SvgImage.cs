using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    /// <summary>
    /// Represents and SVG image
    /// </summary>
    [SvgElement("image")]
    public class SvgImage : SvgVisualElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SvgImage"/> class.
        /// </summary>
        public SvgImage()
        {
            Width = new SvgUnit(0.0f);
            Height = new SvgUnit(0.0f);
        }

        /// <summary>
        /// Gets an <see cref="SvgPoint"/> representing the top left point of the rectangle.
        /// </summary>
        public SvgPoint Location
        {
            get { return new SvgPoint(X, Y); }
        }

        [SvgAttribute("x")]
        public virtual SvgUnit X
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("x"); }
            set { this.Attributes["x"] = value; }
        }

        [SvgAttribute("y")]
        public virtual SvgUnit Y
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("y"); }
            set { this.Attributes["y"] = value; }
        }


        [SvgAttribute("width")]
        public virtual SvgUnit Width
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("width"); }
            set { this.Attributes["width"] = value; }
        }

        [SvgAttribute("height")]
        public virtual SvgUnit Height
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("height"); }
            set { this.Attributes["height"] = value; }
        }

        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public virtual Uri Href
        {
            get { return this.Attributes.GetAttribute<Uri>("href"); }
            set { this.Attributes["href"] = value; }
        }



        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        /// <value>The bounds.</value>
        public override RectangleF Bounds
        {
            get { return new RectangleF(this.Location.ToDeviceValue(), new SizeF(this.Width, this.Height)); }
        }

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        public override GraphicsPath Path
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="Graphics"/> object.
        /// </summary>
        protected override void Render(SvgRenderer renderer)
        {
            if (Width.Value > 0.0f && Height.Value > 0.0f)
            {
                //TODO:
                //base.Render(renderer);
            }
        }


        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgImage>();
        }

        public override SvgElement DeepCopy<T>()
        {
             var newObj = base.DeepCopy<T>() as SvgImage;
            newObj.Height = this.Height;
            newObj.Width = this.Width;
            newObj.X = this.X;
            newObj.Y = this.Y;
            newObj.Href = this.Href;
            return newObj;
        }
    }
}