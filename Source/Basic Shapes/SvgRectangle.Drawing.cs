using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    public partial class SvgRectangle : SvgPathBasedElement
    {
        private GraphicsPath _path;

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
    }
}
