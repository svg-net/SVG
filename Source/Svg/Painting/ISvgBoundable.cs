using System.Drawing;

namespace Svg
{
    public interface ISvgBoundable
    {
#if !NO_SDC
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
#endif
    }
}
