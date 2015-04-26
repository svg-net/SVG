using System;

namespace Svg
{
    public interface Image : IDisposable
    {
        float Width { get; set; }
        float Height { get; set; }
    }
}