#if !NO_SDC
using Svg.ExtensionMethods;

namespace Svg
{
    public abstract partial class SvgMarkerElement : SvgPathBasedElement
    {
        /// <summary>
        /// Renders the stroke of the element to the specified <see cref="ISvgRenderer"/>.
        /// Includes rendering of all markers defined in attributes.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object to render to.</param>
        protected internal override bool RenderStroke(ISvgRenderer renderer)
        {
            var result = base.RenderStroke(renderer);
            var path = Path(renderer);
            var pathLength = path.PointCount;

            var markerStart = MarkerStart.ReplaceWithNullIfNone();
            if (markerStart != null)
            {
                var refPoint1 = path.PathPoints[0];
                var index = 1;
                while (index < pathLength && path.PathPoints[index] == refPoint1)
                {
                    ++index;
                }
                if (index < pathLength)
                {
                    var refPoint2 = path.PathPoints[index];
                    var marker = OwnerDocument.GetElementById<SvgMarker>(markerStart.ToString());
                    marker.RenderMarker(renderer, this, refPoint1, refPoint1, refPoint2, true);
                }
            }

            var markerMid = MarkerMid.ReplaceWithNullIfNone();
            if (markerMid != null)
            {
                var marker = OwnerDocument.GetElementById<SvgMarker>(markerMid.ToString());
                var bezierIndex = -1;
                for (var i = 1; i <= path.PathPoints.Length - 2; i++)
                {
                    // for Bezier curves, the marker shall only been shown at the last point
                    if ((path.PathTypes[i] & 7) == 3)
                        bezierIndex = (bezierIndex + 1) % 3;
                    else
                        bezierIndex = -1;
                    if (bezierIndex == -1 || bezierIndex == 2)
                        marker.RenderMarker(renderer, this, path.PathPoints[i], path.PathPoints[i - 1], path.PathPoints[i], path.PathPoints[i + 1]);

                }
            }

            var markerEnd = MarkerEnd.ReplaceWithNullIfNone();
            if (markerEnd != null)
            {
                var index = pathLength - 1;
                var refPoint1 = path.PathPoints[index];
                --index;
                while (index > 0 && path.PathPoints[index] == refPoint1)
                {
                    --index;
                }
                var refPoint2 = path.PathPoints[index];
                var marker = OwnerDocument.GetElementById<SvgMarker>(markerEnd.ToString());
                marker.RenderMarker(renderer, this, refPoint1, refPoint2, path.PathPoints[path.PathPoints.Length - 1], false);
            }

            return result;
        }
    }
}
#endif
