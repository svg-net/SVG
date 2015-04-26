
using System.Drawing;

namespace Svg
{
    public interface PathGradientBrush : Brush
    {
        PointF CenterPoint { get; set; }
        ColorBlend InterpolationColors { get; set; }
    }
}