using System.Drawing.Drawing2D;

namespace Svg.Pathing
{
    public sealed class SvgClosePathSegment : SvgPathSegment
    {
        public override void AddToPath(GraphicsPath graphicsPath)
        {
            graphicsPath.CloseFigure();
        }
        
        public override string ToString()
        {
            return "z";
        }

    }
}
