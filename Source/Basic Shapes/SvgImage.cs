using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using Svg.Transforms;

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
            protected set
            {
            }
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="Graphics"/> object.
        /// </summary>
        protected override void Render(SvgRenderer renderer)
        {
            if (Width.Value > 0.0f && Height.Value > 0.0f && this.Href != null)
            {
                using (Image b = GetImage(this.Href))
                {
                    if (b != null)
                    {
                        this.PushTransforms(renderer);
                        this.SetClip(renderer);

                        RectangleF srcRect = new RectangleF(0, 0, b.Width, b.Height);
                        var destRect = new RectangleF(this.Location.ToDeviceValue(),
                                        new SizeF(Width.ToDeviceValue(), Height.ToDeviceValue()));

                        renderer.DrawImage(b, destRect, srcRect, GraphicsUnit.Pixel);

                        this.ResetClip(renderer);
                        this.PopTransforms(renderer);
                    }
                }
                // TODO: cache images... will need a shared context for this
                // TODO: support preserveAspectRatio, etc
            }
        }

        protected Image GetImage(Uri uri)
        {
            try
            {
                // should work with http: and file: protocol urls
                var httpRequest = WebRequest.Create(uri);

                using (WebResponse webResponse = httpRequest.GetResponse())
                {
                    MemoryStream ms = BufferToMemoryStream(webResponse.GetResponseStream());
                    Image b = Bitmap.FromStream(ms);
                    return b;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error loading image: '{0}', error: {1} ", uri, ex.Message);
                return null;
            }
        }

        protected static MemoryStream BufferToMemoryStream(Stream input)
        {
            byte[] buffer = new byte[4 * 1024];
            int len;
            MemoryStream ms = new MemoryStream();
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, len);
            }
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
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