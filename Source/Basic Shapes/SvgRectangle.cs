using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    /// <summary>
    /// Represents an SVG rectangle that could also have rounded edges.
    /// </summary>
    [SvgElement("rect")]
    public class SvgRectangle : SvgPathBasedElement
    {
        private SvgUnit _x = 0f;
        private SvgUnit _y = 0f;
        private SvgUnit _width = 0f;
        private SvgUnit _height = 0f;
        private SvgUnit _cornerRadiusX = 0f;
        private SvgUnit _cornerRadiusY = 0f;

        private GraphicsPath _path;

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

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            if (_path == null || IsPathDirty)
            {
                var halfStrokeWidth = base.StrokeWidth / 2;

                // If it is to render, don't need to consider stroke
                if (renderer != null)
                {
                    halfStrokeWidth = 0;
                    this.IsPathDirty = false;
                }

                // If the corners aren't to be rounded just create a rectangle
                if (renderer == null || (CornerRadiusX.Value == 0.0f && CornerRadiusY.Value == 0.0f))
                {
                    var loc_y = Location.Y.ToDeviceValue(renderer, UnitRenderingType.Vertical, this);
                    var loc_x = Location.X.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this);

                    // Starting location which take consideration of stroke width
                    SvgPoint strokedLocation = new SvgPoint(loc_x - halfStrokeWidth, loc_y - halfStrokeWidth);

                    var width = this.Width.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this) + halfStrokeWidth * 2;
                    var height = this.Height.ToDeviceValue(renderer, UnitRenderingType.Vertical, this) + halfStrokeWidth * 2;

                    var location = strokedLocation.ToDeviceValue(renderer, this);
                    var rectangle = new RectangleF(location, new SizeF(width, height));

                    _path = new GraphicsPath();
                    _path.StartFigure();
                    _path.AddRectangle(rectangle);
                    _path.CloseFigure();
                }
                else
                {
                    _path = new GraphicsPath();
                    var arcBounds = new RectangleF();
                    var lineStart = new PointF();
                    var lineEnd = new PointF();
                    var width = Width.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this);
                    var height = Height.ToDeviceValue(renderer, UnitRenderingType.Vertical, this);
                    var rx = Math.Min(CornerRadiusX.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this) * 2, width);
                    var ry = Math.Min(CornerRadiusY.ToDeviceValue(renderer, UnitRenderingType.Vertical, this) * 2, height);
                    var location = Location.ToDeviceValue(renderer, this);

                    // Start
                    _path.StartFigure();

                    // Add first arc
                    arcBounds.Location = location;
                    arcBounds.Width = rx;
                    arcBounds.Height = ry;
                    _path.AddArc(arcBounds, 180, 90);

                    // Add first line
                    lineStart.X = Math.Min(location.X + rx, location.X + width * 0.5f);
                    lineStart.Y = location.Y;
                    lineEnd.X = Math.Max(location.X + width - rx, location.X + width * 0.5f);
                    lineEnd.Y = lineStart.Y;
                    _path.AddLine(lineStart, lineEnd);

                    // Add second arc
                    arcBounds.Location = new PointF(location.X + width - rx, location.Y);
                    _path.AddArc(arcBounds, 270, 90);

                    // Add second line
                    lineStart.X = location.X + width;
                    lineStart.Y = Math.Min(location.Y + ry, location.Y + height * 0.5f);
                    lineEnd.X = lineStart.X;
                    lineEnd.Y = Math.Max(location.Y + height - ry, location.Y + height * 0.5f);
                    _path.AddLine(lineStart, lineEnd);

                    // Add third arc
                    arcBounds.Location = new PointF(location.X + width - rx, location.Y + height - ry);
                    _path.AddArc(arcBounds, 0, 90);

                    // Add third line
                    lineStart.X = Math.Max(location.X + width - rx, location.X + width * 0.5f);
                    lineStart.Y = location.Y + height;
                    lineEnd.X = Math.Min(location.X + rx, location.X + width * 0.5f);
                    lineEnd.Y = lineStart.Y;
                    _path.AddLine(lineStart, lineEnd);

                    // Add third arc
                    arcBounds.Location = new PointF(location.X, location.Y + height - ry);
                    _path.AddArc(arcBounds, 90, 90);

                    // Add fourth line
                    lineStart.X = location.X;
                    lineStart.Y = Math.Max(location.Y + height - ry, location.Y + height * 0.5f);
                    lineEnd.X = lineStart.X;
                    lineEnd.Y = Math.Min(location.Y + ry, location.Y + height * 0.5f);
                    _path.AddLine(lineStart, lineEnd);

                    // Close
                    _path.CloseFigure();
                }
            }
            return _path;
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="Graphics"/> object.
        /// </summary>
        protected override void Render(ISvgRenderer renderer)
        {
            if (Width.Value > 0.0f && Height.Value > 0.0f)
            {
                base.Render(renderer);
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
