using System;

namespace Svg
{
    public interface Image : IDisposable
    {
        int Width { get; }
        int Height { get; }
    }
}