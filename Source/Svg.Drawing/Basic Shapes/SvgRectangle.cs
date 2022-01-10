namespace Svg
{
    /// <summary>
    /// Represents an SVG rectangle that could also have rounded edges.
    /// </summary>
    [SvgElement("rect")]
    public partial class SvgRectangle : SvgPathBasedElement
    {
        private SvgUnit _x = 0f;
        private SvgUnit _y = 0f;
        private SvgUnit _width = 0f;
        private SvgUnit _height = 0f;
        private SvgUnit _cornerRadiusX = 0f;
        private SvgUnit _cornerRadiusY = 0f;

        /// <summary>
        /// Gets an <see cref="SvgPoint"/> representing the top left point of the rectangle.
        /// </summary>
        public SvgPoint Location
        {
            get { return new SvgPoint(X, Y); }
        }

        /// <summary>
        /// Gets or sets the position where the left point of the rectangle should start.
        /// </summary>
        [SvgAttribute("x")]
        public SvgUnit X
        {
            get { return _x; }
            set { _x = value; Attributes["x"] = value; IsPathDirty = true; }
        }

        /// <summary>
        /// Gets or sets the position where the top point of the rectangle should start.
        /// </summary>
        [SvgAttribute("y")]
        public SvgUnit Y
        {
            get { return _y; }
            set { _y = value; Attributes["y"] = value; IsPathDirty = true; }
        }

        /// <summary>
        /// Gets or sets the width of the rectangle.
        /// </summary>
        [SvgAttribute("width")]
        public SvgUnit Width
        {
            get { return _width; }
            set { _width = value; Attributes["width"] = value; IsPathDirty = true; }
        }

        /// <summary>
        /// Gets or sets the height of the rectangle.
        /// </summary>
        [SvgAttribute("height")]
        public SvgUnit Height
        {
            get { return _height; }
            set { _height = value; Attributes["height"] = value; IsPathDirty = true; }
        }

        /// <summary>
        /// Gets or sets the X-radius of the rounded edges of this rectangle.
        /// </summary>
        [SvgAttribute("rx")]
        public SvgUnit CornerRadiusX
        {
            get
            {
                // If ry has been set and rx hasn't, use it's value
                return (_cornerRadiusX.Value == 0.0f && _cornerRadiusY.Value > 0.0f) ? _cornerRadiusY : _cornerRadiusX;
            }
            set { _cornerRadiusX = value; Attributes["rx"] = value; IsPathDirty = true; }
        }

        /// <summary>
        /// Gets or sets the Y-radius of the rounded edges of this rectangle.
        /// </summary>
        [SvgAttribute("ry")]
        public SvgUnit CornerRadiusY
        {
            get
            {
                // If rx has been set and ry hasn't, use it's value
                return (_cornerRadiusY.Value == 0.0f && _cornerRadiusX.Value > 0.0f) ? _cornerRadiusX : _cornerRadiusY;
            }
            set { _cornerRadiusY = value; Attributes["ry"] = value; IsPathDirty = true; }
        }

        /// <summary>
        /// Gets or sets a value to determine if anti-aliasing should occur when the element is being rendered.
        /// </summary>
        protected override bool RequiresSmoothRendering
        {
            get
            {
                if (base.RequiresSmoothRendering)
                    return (CornerRadiusX.Value > 0.0f || CornerRadiusY.Value > 0.0f);
                else
                    return false;
            }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgRectangle>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgRectangle;

            newObj._x = _x;
            newObj._y = _y;
            newObj._width = _width;
            newObj._height = _height;
            newObj._cornerRadiusX = _cornerRadiusX;
            newObj._cornerRadiusY = _cornerRadiusY;
            return newObj;
        }
    }
}
