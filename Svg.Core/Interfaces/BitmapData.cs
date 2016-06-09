
using System;

namespace Svg
{
    public interface BitmapData
    {
        IntPtr Scan0 { get; set; }
        int Stride { get; set; }
        int Width { get; }
        int Height { get; }
    }
}