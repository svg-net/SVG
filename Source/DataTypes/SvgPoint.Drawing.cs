#if !NO_SDC
using System.Drawing;

namespace Svg
{
    public partial struct SvgPoint
    {
        public PointF ToDeviceValue(ISvgRenderer renderer, SvgElement owner)
        {
            return SvgUnit.GetDevicePoint(this.X, this.Y, renderer, owner);
        }
    }
}
#endif
