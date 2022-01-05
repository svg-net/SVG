using System.Drawing;

namespace Svg
{
    public interface IGraphicsProvider
    {
#if !NO_SDC
        Graphics GetGraphics();
#endif
    }
}
