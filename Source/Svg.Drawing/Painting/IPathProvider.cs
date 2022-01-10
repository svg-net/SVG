#if !NO_SDC
using System.Drawing.Drawing2D;
#endif

namespace Svg
{
    /// <summary>
    /// Defines the methods and properties required for an SVG element to provide graphics path.
    /// </summary>
    public interface IPathProvider
    {
#if !NO_SDC
        GraphicsPath Path(ISvgRenderer renderer);
#endif
    }
}
