

namespace Svg
{
    public interface TextureBrush : Brush
    {
        Matrix Transform { get; set; }
    }
}