using System;
using System.Collections.Generic;
using System.Text;

namespace Svg.Pathing
{
    public sealed class SvgClosePathSegment : SvgPathSegment
    {
        public override void AddToPath(System.Drawing.Drawing2D.GraphicsPath graphicsPath)
        {
            if (graphicsPath.PointCount == 0)
            {
                return;
            }

            // Important for custom line caps.  Force the path the close with an explicit line, not just an implicit close of the figure.
            var pathPoints = graphicsPath.PathPoints;

            if (!pathPoints[0].Equals(pathPoints[pathPoints.Length - 1]))
            {
                var pathTypes = graphicsPath.PathTypes;
                int i = pathPoints.Length - 1;
                while (i >= 0 && pathTypes[i] > 0) i--;
                if (i < 0) i = 0;
                graphicsPath.AddLine(pathPoints[pathPoints.Length - 1], pathPoints[i]);
            }

            graphicsPath.CloseFigure();
        }

        public override string ToString()
        {
            return "z";
        }

    }
}