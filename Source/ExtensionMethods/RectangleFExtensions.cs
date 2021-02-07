using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace Svg.ExtensionMethods
{
    internal static class RectangleFExtensions
    {
        internal static Rectangle Round(this RectangleF rectangle)
        {
            return new Rectangle((int)Math.Round(rectangle.X), (int)Math.Round(rectangle.Y), (int)Math.Round(rectangle.Width), (int)Math.Round(rectangle.Height));
        }

        internal static RectangleF GetIntersection(this RectangleF rectangle, RectangleF anotherRectangle)
        {
            var x1 = Math.Max(rectangle.X, anotherRectangle.X);
            var x2 = Math.Min(rectangle.X + rectangle.Width, anotherRectangle.X + anotherRectangle.Width);

            var y1 = Math.Max(rectangle.Y, anotherRectangle.Y);
            var y2 = Math.Min(rectangle.Y + rectangle.Height, anotherRectangle.Y + anotherRectangle.Height);

            var width = x2 - x1;
            var height = y2 - y1;

            if (width < 0)
            {
                width = 0;
            }

            if (height < 0)
            {
                height = 0;
            }

            return new RectangleF(x1, y1, width, height);
        }

        internal static RectangleF GetIntersection(this RectangleF rectangle, SizeF size)
        {
            return GetIntersection(rectangle, new RectangleF(new PointF(0, 0), size));
        }

        internal static PointF[] GetPoints(this RectangleF rectangle)
        {
            return new[]
            {
                rectangle.Location,
                rectangle.Location + rectangle.Size
            };
        }

        internal static RectangleF Transform(this RectangleF rectangle, Matrix matrix)
        {
            var points = GetPoints(rectangle);
            matrix.TransformPoints(points);
            return points.GetBounds();
        }
    }
}
