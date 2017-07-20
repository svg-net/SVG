using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if NETFULL
using System.Drawing.Drawing2D;
using System.Drawing;
#else
using System.DrawingCore.Drawing2D;
using System.DrawingCore;
#endif

namespace Svg
{
    public interface IFontDefn : IDisposable
    {
        float Size { get; }
        float SizeInPoints { get; }
        void AddStringToPath(ISvgRenderer renderer, GraphicsPath path, string text, PointF location);
        float Ascent(ISvgRenderer renderer);
        IList<RectangleF> MeasureCharacters(ISvgRenderer renderer, string text);
        SizeF MeasureString(ISvgRenderer renderer, string text);
    }
}
