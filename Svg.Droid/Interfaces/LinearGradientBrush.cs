using System.Drawing.Drawing2D;

namespace Svg
{
    public interface LinearGradientBrush : Brush
    {
        ColorBlend InterpolationColors { get; set; }
        WrapMode WrapMode { get; set; }
    }
}