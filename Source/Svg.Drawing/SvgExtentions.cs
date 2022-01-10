using System.Drawing;

namespace Svg
{
    public  static partial class SvgExtentions
    {
#if !NO_SDC
        public static RectangleF GetRectangle(this SvgRectangle r)
        {
            return new RectangleF(r.X, r.Y, r.Width, r.Height);
        }
#endif
    }
}
