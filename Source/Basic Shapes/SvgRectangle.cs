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
            get { return this.Attributes.GetAttribute<SvgUnit>("x"); }
            set { this.Attributes["x"] = value; this.IsPathDirty = true; }
        }

        /// <summary>
        /// Gets or sets the position where the top point of the rectangle should start.
        /// </summary>
        [SvgAttribute("y")]
        public SvgUnit Y
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("y"); }
            set { this.Attributes["y"] = value; this.IsPathDirty = true; }
        }

        /// <summary>
        /// Gets or sets the width of the rectangle.
        /// </summary>
        [SvgAttribute("width")]
        public SvgUnit Width
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("width"); }
            set { this.Attributes["width"] = value; this.IsPathDirty = true; }
        }

        /// <summary>
        /// Gets or sets the height of the rectangle.
        /// </summary>
        [SvgAttribute("height")]
        public SvgUnit Height
        {
            get { return this.Attributes.GetAttribute<SvgUnit>("height"); }
            set { this.Attributes["height"] = value; this.IsPathDirty = true; }
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
                var rx = this.Attributes.GetAttribute<SvgUnit>("rx");
                var ry = this.Attributes.GetAttribute<SvgUnit>("ry");
                return (rx.Value == 0.0f && ry.Value > 0.0f) ? ry : rx;
            }
            set { this.Attributes["rx"] = value; this.IsPathDirty = true; }
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
                var rx = this.Attributes.GetAttribute<SvgUnit>("rx");
                var ry = this.Attributes.GetAttribute<SvgUnit>("ry");
                return (ry.Value == 0.0f && rx.Value > 0.0f) ? rx : ry;
            }
            set { this.Attributes["ry"] = value; this.IsPathDirty = true; }
        }

        /// <summary>
        /// Gets or sets a value to determine if anti-aliasing should occur when the element is being rendered.
        /// </summary>
        protected override bool RequiresSmoothRendering
        {
            get
            {
                if (base.RequiresSmoothRendering)
                    return (CornerRadiusX.Value > 0 || CornerRadiusY.Value > 0);
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

                    var rectangle = new RectangleF(strokedLocation.ToDeviceValue(renderer, this), new SizeF(width, height));

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
            newObj.CornerRadiusX = this.CornerRadiusX;
            newObj.CornerRadiusY = this.CornerRadiusY;
            newObj.Height = this.Height;
            newObj.Width = this.Width;
            newObj.X = this.X;
            newObj.Y = this.Y;
            return newObj;
        }
    }
}
