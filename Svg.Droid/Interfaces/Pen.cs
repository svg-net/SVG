
using System;

namespace Svg
{
    public interface Pen : IDisposable
    {
        float[] DashPattern { get; set; }
        LineJoin LineJoin { get; set; }
        float MiterLimit { get; set; }
        LineCap StartCap { get; set; }
        LineCap EndCap { get; set; }
    }
}