using System;

namespace Svg
{
    /// <summary>
    /// An <see cref="SvgFragment"/> represents an SVG fragment that can be the root element or an embedded fragment of an SVG document.
    /// </summary>
    [SvgElement("svg")]
    public partial class SvgFragment : SvgElement, ISvgViewPort
    {
        private SvgUnit _x = 0f;
        private SvgUnit _y = 0f;

        /// <summary>
        /// Gets the SVG namespace string.
        /// </summary>
        public static readonly Uri Namespace = new Uri(SvgNamespaces.SvgNamespace);

        /// <summary>
        /// Gets or sets the position where the left point of the svg should start.
        /// </summary>
        [SvgAttribute("x")]
        public virtual SvgUnit X
        {
            get { return _x; }
            set
            {
                if (_x != value)
                    _x = value;
                Attributes["x"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the position where the top point of the svg should start.
        /// </summary>
        [SvgAttribute("y")]
        public virtual SvgUnit Y
        {
            get { return _y; }
            set
            {
                if (_y != value)
                    _y = value;
                Attributes["y"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the width of the fragment.
        /// </summary>
        /// <value>The width.</value>
        [SvgAttribute("width")]
        public SvgUnit Width
        {
            get { return GetAttribute("width", false, new SvgUnit(SvgUnitType.Percentage, 100f)); }
            set { Attributes["width"] = value; }
        }

        /// <summary>
        /// Gets or sets the height of the fragment.
        /// </summary>
        /// <value>The height.</value>
        [SvgAttribute("height")]
        public SvgUnit Height
        {
            get { return GetAttribute("height", false, new SvgUnit(SvgUnitType.Percentage, 100f)); }
            set { Attributes["height"] = value; }
        }

        [SvgAttribute("overflow")]
        public virtual SvgOverflow Overflow
        {
            get { return GetAttribute("overflow", false, SvgOverflow.Hidden); }
            set { Attributes["overflow"] = value; }
        }

        /// <summary>
        /// Gets or sets the viewport of the element.
        /// </summary>
        /// <value></value>
        [SvgAttribute("viewBox")]
        public SvgViewBox ViewBox
        {
            get { return GetAttribute("viewBox", false, SvgViewBox.Empty); }
            set { Attributes["viewBox"] = value; }
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

        /// <summary>
        /// Refers to the size of the font from baseline to baseline when multiple lines of text are set solid in a multiline layout environment.
        /// </summary>
        [SvgAttribute("font-size")]
        public override SvgUnit FontSize
        {
            get { return GetAttribute("font-size", true, SvgUnit.Empty); }
            set { Attributes["font-size"] = value; }
        }

        /// <summary>
        /// Indicates which font family is to be used to render the text.
        /// </summary>
        [SvgAttribute("font-family")]
        public override string FontFamily
        {
            get { return GetAttribute<string>("font-family", true); }
            set { Attributes["font-family"] = value; }
        }

        public override XmlSpaceHandling SpaceHandling
        {
            get { return GetAttribute("space", true, XmlSpaceHandling.Default); }
            set { base.SpaceHandling = value; IsPathDirty = true; }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgFragment>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgFragment;

            newObj._x = _x;
            newObj._y = _y;
            return newObj;
        }
    }
}
