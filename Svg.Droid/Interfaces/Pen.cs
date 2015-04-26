
using System;

namespace Svg
{
    public abstract class Pen : IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public float[] DashPattern { get; set; }
        public LineJoin LineJoin { get; set; }
        public float MiterLimit { get; set; }
        public LineCap StartCap { get; set; }
        public LineCap EndCap { get; set; }
    }
}