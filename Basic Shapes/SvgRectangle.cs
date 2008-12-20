using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    /// <summary>
    /// Represents and SVG rectangle that could also have reounded edges.
    /// </summary>
    [SvgElement("rect")]
    public class SvgRectangle : SvgVisualElement
    {
        private SvgUnit _cornerRadiusX;
        private SvgUnit _cornerRadiusY;
        private SvgUnit _height;
        private GraphicsPath _path;
        private SvgUnit _width;
        private SvgUnit _x;
        private SvgUnit _y;

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgRectangle"/> class.
        /// </summary>
        public SvgRectangle()
        {
            _width = new SvgUnit(0.0f);
            _height = new SvgUnit(0.0f);
        }

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
            set
            {
                _x = value;
                IsPathDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the position where the top point of the rectangle should start.
        /// </summary>
        [SvgAttribute("y")]
        public SvgUnit Y
        {
            get { return _y; }
            set
            {
                _y = value;
                IsPathDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the width of the rectangle.
        /// </summary>
        [SvgAttribute("width")]
        public SvgUnit Width
        {
            get { return _width; }
            set
            {
                _width = value;
                IsPathDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the height of the rectangle.
        /// </summary>
        [SvgAttribute("height")]
        public SvgUnit Height
        {
            get { return _height; }
            set
            {
                _height = value;
                IsPathDirty = true;
            }
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
                if (_cornerRadiusX.Value == 0.0f && _cornerRadiusY.Value > 0.0f)
                    return _cornerRadiusY;

                return _cornerRadiusX;
            }
            set
            {
                _cornerRadiusX = value;
                IsPathDirty = true;
            }
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
                if (_cornerRadiusY.Value == 0.0f && _cornerRadiusX.Value > 0.0f)
                    return _cornerRadiusX;

                return _cornerRadiusY;
            }
            set
            {
                _cornerRadiusY = value;
                IsPathDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets a value to determine if anti-aliasing should occur when the element is being rendered.
        /// </summary>
        protected override bool RequiresSmoothRendering
        {
            get { return (CornerRadiusX.Value > 0 || CornerRadiusY.Value > 0); }
        }

        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        /// <value>The bounds.</value>
        public override RectangleF Bounds
        {
            get { return Path.GetBounds(); }
        }

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        public override GraphicsPath Path
        {
            get
            {
                if (_path == null || IsPathDirty)
                {
                    // If the corners aren't to be rounded just create a rectangle
                    if (CornerRadiusX.Value == 0.0f && CornerRadiusY.Value == 0.0f)
                    {
                        var rectangle = new RectangleF(Location.ToDeviceValue(),
                            new SizeF(Width.ToDeviceValue(), Height.ToDeviceValue()));

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
                        var width = Width.ToDeviceValue();
                        var height = Height.ToDeviceValue();
                        var rx = CornerRadiusX.ToDeviceValue();
                        var ry = CornerRadiusY.ToDeviceValue();
                        var location = Location.ToDeviceValue();

                        // Start
                        _path.StartFigure();

                        // Add first arc
                        arcBounds.Location = location;
                        arcBounds.Width = rx;
                        arcBounds.Height = ry;
                        _path.AddArc(arcBounds, 180, 90);

                        // Add first line
                        lineStart.X = location.X + rx;
                        lineStart.Y = location.Y;
                        lineEnd.X = location.X + width - rx;
                        lineEnd.Y = lineStart.Y;
                        _path.AddLine(lineStart, lineEnd);

                        // Add second arc
                        arcBounds.Location = new PointF(location.X + width - rx, location.Y);
                        _path.AddArc(arcBounds, 270, 90);

                        // Add second line
                        lineStart.X = location.X + width;
                        lineStart.Y = location.Y + ry;
                        lineEnd.X = lineStart.X;
                        lineEnd.Y = location.Y + height - ry;
                        _path.AddLine(lineStart, lineEnd);

                        // Add third arc
                        arcBounds.Location = new PointF(location.X + width - rx, location.Y + height - ry);
                        _path.AddArc(arcBounds, 0, 90);

                        // Add third line
                        lineStart.X = location.X + width - rx;
                        lineStart.Y = location.Y + height;
                        lineEnd.X = location.X + rx;
                        lineEnd.Y = lineStart.Y;
                        _path.AddLine(lineStart, lineEnd);

                        // Add third arc
                        arcBounds.Location = new PointF(location.X, location.Y + height - ry);
                        _path.AddArc(arcBounds, 90, 90);

                        // Add fourth line
                        lineStart.X = location.X;
                        lineStart.Y = location.Y + height - ry;
                        lineEnd.X = lineStart.X;
                        lineEnd.Y = location.Y + ry;
                        _path.AddLine(lineStart, lineEnd);

                        // Close
                        _path.CloseFigure();
                    }
                    IsPathDirty = false;
                }
                return _path;
            }
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="Graphics"/> object.
        /// </summary>
        protected override void Render(SvgRenderer renderer)
        {
            if (Width.Value > 0.0f && Height.Value > 0.0f)
            {
                base.Render(renderer);
            }
        }
    }
}