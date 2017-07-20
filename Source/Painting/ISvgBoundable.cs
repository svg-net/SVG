#if NETFULL
using System.Drawing.Drawing2D;
using System.Drawing;
#else
using System.DrawingCore.Drawing2D;
using System.DrawingCore;
#endif


namespace Svg
{
    public interface ISvgBoundable
    {
        PointF Location
        {
            get;
        }

        SizeF Size
        {
            get;
        }

        RectangleF Bounds
        {
            get;
        } 
    }
}