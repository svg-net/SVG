using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Svg.FilterEffects
{
    public partial class SvgFilter : SvgElement
    {
        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="ISvgRenderer"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object to render to.</param>
        protected override void Render(ISvgRenderer renderer)
        {
            RenderChildren(renderer);
        }

        private Matrix GetTransform(SvgVisualElement element)
        {
            var transformMatrix = new Matrix();
            if (element.Transforms != null)
                using (var matrix = element.Transforms.GetMatrix())
                    transformMatrix.Multiply(matrix);
            return transformMatrix;
        }

        private RectangleF GetPathBounds(SvgVisualElement element, ISvgRenderer renderer, Matrix transform)
        {
            var bounds = element is SvgGroup ? element.Path(renderer).GetBounds() : element.Bounds;
            var pts = new PointF[] { bounds.Location, new PointF(bounds.Right, bounds.Bottom) };
            transform.TransformPoints(pts);

            return new RectangleF(Math.Min(pts[0].X, pts[1].X), Math.Min(pts[0].Y, pts[1].Y),
                                  Math.Abs(pts[0].X - pts[1].X), Math.Abs(pts[0].Y - pts[1].Y));
        }

        public void ApplyFilter(SvgVisualElement element, ISvgRenderer renderer, Action<ISvgRenderer> renderMethod)
        {
            using (var transform = GetTransform(element))
            {
                var bounds = GetPathBounds(element, renderer, transform);
                if (bounds.Width == 0f || bounds.Height == 0f)
                    return;

                var inflate = 0.5f;
                using (var buffer = new ImageBuffer(bounds, inflate, renderer, renderMethod) { Transform = transform })
                {
                    foreach (var primitive in Children.OfType<SvgFilterPrimitive>())
                        primitive.Process(buffer);

                    // Render the final filtered image
                    var bufferImg = buffer.Buffer;
                    var imgDraw = RectangleF.Inflate(bounds, inflate * bounds.Width, inflate * bounds.Height);

                    var prevClip = renderer.GetClip();
                    try
                    {
                        renderer.SetClip(new Region(imgDraw));
                        renderer.DrawImage(bufferImg, imgDraw, new RectangleF(bounds.X, bounds.Y, imgDraw.Width, imgDraw.Height), GraphicsUnit.Pixel);
                    }
                    finally
                    {
                        renderer.SetClip(prevClip);
                    }
                }
            }
        }
    }
}
