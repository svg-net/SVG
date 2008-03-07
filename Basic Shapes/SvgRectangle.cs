using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;
using System.ComponentModel;

namespace Svg
{
    [Serializable()]
    public class SvgRectangle : SvgGraphicsElement
    {
        private SvgUnit _width;
        private SvgUnit _height;
        private SvgUnit _x;
        private SvgUnit _y;
        private SvgUnit _cornerRadiusX;
        private SvgUnit _cornerRadiusY;
        private GraphicsPath _path;

        public SvgPoint Location
        {
            get { return new SvgPoint(this.X, this.Y); }
        }

        [SvgAttribute("x")]
        public SvgUnit X
        {
            get { return this._x; }
            set { this._x = value; this.IsPathDirty = true; }
        }

        [SvgAttribute("y")]
        public SvgUnit Y
        {
            get { return this._y; }
            set { this._y = value; this.IsPathDirty = true; }
        }

        [SvgAttribute("width")]
        public SvgUnit Width
        {
            get { return this._width; }
            set { this._width = value; this.IsPathDirty = true; }
        }

        [SvgAttribute("height")]
        public SvgUnit Height
        {
            get { return this._height; }
            set { this._height = value; this.IsPathDirty = true; }
        }

        [SvgAttribute("rx")]
        public SvgUnit CornerRadiusX
        {
            get
            {
                // If ry has been set and rx hasn't, use it's value
                if (this._cornerRadiusX.Value == 0.0f && this._cornerRadiusY.Value > 0.0f)
                    return this._cornerRadiusY;

                return this._cornerRadiusX;
            }
            set { this._cornerRadiusX = value; this.IsPathDirty = true; }
        }

        [SvgAttribute("ry")]
        public SvgUnit CornerRadiusY
        {
            get
            {
                // If rx has been set and ry hasn't, use it's value
                if (this._cornerRadiusY.Value == 0.0f && this._cornerRadiusX.Value > 0.0f)
                    return this._cornerRadiusX;

                return this._cornerRadiusY;
            }
            set
            {
                this._cornerRadiusY = value;
                this.IsPathDirty = true;
            }
        }

        protected override bool RequiresSmoothRendering
        {
            get { return (this.CornerRadiusX.Value > 0 || this.CornerRadiusY.Value > 0); }
        }

        public override RectangleF Bounds
        {
            get { return this.Path.GetBounds(); }
        }

        public override GraphicsPath Path
        {
            get
            {
                if (this._path == null || this.IsPathDirty)
                {
                    // If the corners aren't to be rounded just create a rectangle
                    if (this.CornerRadiusX.Value == 0.0f && this.CornerRadiusY.Value == 0.0f)
                    {
                        RectangleF rectangle = new RectangleF(this.Location.ToDeviceValue(), new SizeF(this.Width.ToDeviceValue(), this.Height.ToDeviceValue()));

                        _path = new GraphicsPath();
                        _path.StartFigure();
                        _path.AddRectangle(rectangle);
                        _path.CloseFigure();
                    }
                    else
                    {
                        _path = new GraphicsPath();
                        RectangleF arcBounds = new RectangleF();
                        PointF lineStart = new PointF();
                        PointF lineEnd = new PointF();
                        float width = this.Width.ToDeviceValue();
                        float height = this.Height.ToDeviceValue();
                        float rx = this.CornerRadiusX.ToDeviceValue();
                        float ry = this.CornerRadiusY.ToDeviceValue();
                        PointF location = this.Location.ToDeviceValue();

                        // Start
                        _path.StartFigure();

                        // Add first arc
                        arcBounds.Location = location;
                        arcBounds.Width = rx;
                        arcBounds.Height = ry;
                        _path.AddArc(arcBounds, 180, 90);

                        // Add first line
                        lineStart.X = location.X+rx;
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
                    this.IsPathDirty = false;
                }
                return _path;
            }
        }

        protected override void Render(Graphics graphics)
        {
            if (this.Width.Value > 0.0f && this.Height.Value > 0.0f)
                base.Render(graphics);
        }

        public SvgRectangle()
        {
            this._width = new SvgUnit(0.0f);
            this._height = new SvgUnit(0.0f);
        }
    }
}