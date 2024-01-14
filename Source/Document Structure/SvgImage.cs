using System;
using System.IO;

namespace Svg
{
    /// <summary>
    /// Represents and SVG image
    /// </summary>
    [SvgElement("image")]
    public partial class SvgImage : SvgVisualElement
    {
        private const string MimeTypeSvg = "image/svg+xml";

        /// <summary>
        /// Gets an <see cref="SvgPoint"/> representing the top left point of the rectangle.
        /// </summary>
        public SvgPoint Location
        {
            get { return new SvgPoint(X, Y); }
        }

        /// <summary>
        /// Gets or sets the aspect of the viewport.
        /// </summary>
        /// <value></value>
        [SvgAttribute("preserveAspectRatio")]
        public SvgAspectRatio AspectRatio
        {
            get { return GetAttribute("preserveAspectRatio", false, new SvgAspectRatio(SvgPreserveAspectRatio.xMidYMid)); }
            set { Attributes["preserveAspectRatio"] = value; }
        }

        [SvgAttribute("x")]
        public virtual SvgUnit X
        {
            get { return GetAttribute<SvgUnit>("x", false); }
            set { Attributes["x"] = value; }
        }

        [SvgAttribute("y")]
        public virtual SvgUnit Y
        {
            get { return GetAttribute<SvgUnit>("y", false); }
            set { Attributes["y"] = value; }
        }

        [SvgAttribute("width")]
        public virtual SvgUnit Width
        {
            get { return GetAttribute<SvgUnit>("width", false); }
            set { Attributes["width"] = value; }
        }

        [SvgAttribute("height")]
        public virtual SvgUnit Height
        {
            get { return GetAttribute<SvgUnit>("height", false); }
            set { Attributes["height"] = value; }
        }

        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public virtual string Href
        {
            get { return GetAttribute<string>("href", false); }
            set { Attributes["href"] = value; }
        }

        internal ExternalType ResolveExternalImages
        {
            get { return SvgDocument.ResolveExternalImages; }
        }

        private SvgDocument LoadSvg(Stream stream, Uri baseUri)
        {
            var document = SvgDocument.Open<SvgDocument>(stream);
            document.BaseUri = baseUri;
            return document;
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgImage>();
        }
    }
}
