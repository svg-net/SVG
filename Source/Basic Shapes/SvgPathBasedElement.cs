using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    /// <summary>
    /// Represents an element that is using a GraphicsPath as rendering base.
    /// </summary>
    public abstract class SvgPathBasedElement : SvgVisualElement
    {
        public override RectangleF Bounds
        {
            get
            {
                var path = Path(null);
                if (path == null)
                    return new RectangleF();
                if (Transforms == null || Transforms.Count == 0)
                    return path.GetBounds();

                using (path = (GraphicsPath)path.Clone())
                using (var matrix = Transforms.GetMatrix())
                {
                    path.Transform(matrix);
                    return path.GetBounds();
                }
            }
        }
    }
}
