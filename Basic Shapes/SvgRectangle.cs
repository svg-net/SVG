using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    [Serializable]
    public class SvgRectangle : SvgGraphicsElement
    {
        private SvgUnit _cornerRadiusX;
        private SvgUnit _cornerRadiusY;
        private SvgUnit _height;
        private GraphicsPath _path;
        private SvgUnit _width;
        private SvgUnit _x;
        private SvgUnit _y;

        public SvgRectangle()
        {
            _width = new SvgUnit(0.0f);
            _height = new SvgUnit(0.0f);
        }

        public SvgPoint Location
        {
            get { return new SvgPoint(X, Y); }
        }

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

        protected override bool RequiresSmoothRendering
        {
            get { return (CornerRadiusX.Value > 0 || CornerRadiusY.Value > 0); }
        }

        public override RectangleF Bounds
        {
            get { return Path.GetBounds(); }
        }

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

        protected override void Render(Graphics graphics)
        {
            if (Width.Value > 0.0f && Height.Value > 0.0f)
                base.Render(graphics);
        }
    }
}