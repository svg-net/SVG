using System.Drawing;

namespace Svg
{
    public interface IGraphicsProvider
    {
        Graphics GetGraphics();
    }
}
