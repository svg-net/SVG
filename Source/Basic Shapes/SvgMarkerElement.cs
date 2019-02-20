using Svg.ExtensionMethods;
using System;

namespace Svg
{
    /// <summary>
    /// Represents a path based element that can have markers.
    /// </summary>
    public abstract class SvgMarkerElement : SvgPathBasedElement
    {
        /// <summary>
        /// Gets or sets the marker (end cap) of the path.
        /// </summary>
        [SvgAttribute("marker-end", true)]
        public Uri MarkerEnd
        {
            get { return this.Attributes.GetAttribute<Uri>("marker-end").ReplaceWithNullIfNone(); }
            set { this.Attributes["marker-end"] = value; }
        }


        /// <summary>
        /// Gets or sets the marker (mid points) of the path.
        /// </summary>
        [SvgAttribute("marker-mid", true)]
        public Uri MarkerMid
        {
            get { return this.Attributes.GetAttribute<Uri>("marker-mid").ReplaceWithNullIfNone(); }
            set { this.Attributes["marker-mid"] = value; }
        }


        /// <summary>
        /// Gets or sets the marker (start cap) of the path.
        /// </summary>
        [SvgAttribute("marker-start", true)]
        public Uri MarkerStart
        {
            get { return this.Attributes.GetAttribute<Uri>("marker-start").ReplaceWithNullIfNone(); }
            set { this.Attributes["marker-start"] = value; }
        }

        /// <summary>
        /// Renders the stroke of the element to the specified <see cref="ISvgRenderer"/>.
        /// Includes rendering of all markers defined in attributes.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> object to render to.</param>
        protected internal override bool RenderStroke(ISvgRenderer renderer)
        {
            var result = base.RenderStroke(renderer);
            var path = this.Path(renderer);
            var pathLength = path.PathPoints.Length;

            if (this.MarkerStart != null)
            {
                var refPoint1 = path.PathPoints[0];
                var index = 1;
                while (index < pathLength && path.PathPoints[index] == refPoint1)
                {
                    ++index;
                }
                var refPoint2 = path.PathPoints[index];
                SvgMarker marker = this.OwnerDocument.GetElementById<SvgMarker>(this.MarkerStart.ToString());
                marker.RenderMarker(renderer, this, refPoint1, refPoint1, refPoint2);
            }

            if (this.MarkerMid != null)
            {
                SvgMarker marker = this.OwnerDocument.GetElementById<SvgMarker>(this.MarkerMid.ToString());
                int bezierIndex = -1;
                for (int i = 1; i <= path.PathPoints.Length - 2; i++)
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

            if (this.MarkerEnd != null)
            {
                var index = pathLength - 1;
                var refPoint1 = path.PathPoints[index];
                --index;
                while (index > 0 && path.PathPoints[index] == refPoint1)
                {
                    --index;
                }
                var refPoint2 = path.PathPoints[index];
                SvgMarker marker = this.OwnerDocument.GetElementById<SvgMarker>(this.MarkerEnd.ToString());
                marker.RenderMarker(renderer, this, refPoint1, refPoint2, path.PathPoints[path.PathPoints.Length - 1]);
            }

            return result;
        }
    }
}
