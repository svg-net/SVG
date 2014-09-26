using System.Drawing;

namespace Svg
{
    public interface ISvgBoundable
    {
        RectangleF CalculateBounds();
    }
}