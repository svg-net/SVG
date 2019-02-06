using Svg.ExtensionMethods;
using System;
using System.Drawing;

namespace Svg
{
    /// <summary>
    /// An element used to group SVG shapes.
    /// </summary>
    [SvgElement("g")]
    public class SvgGroup : SvgMarkerElement
    {
        bool markersSet = false;

        /// <summary>
        /// If the group has marker attributes defined, add them to all children
        /// that are able to display markers. Only done once.
        /// </summary>
        private void AddMarkers()
        {
            if (!markersSet)
            {
                if (this.MarkerStart != null || this.MarkerMid != null || this.MarkerEnd != null)
                {
                    foreach (var c in this.Children)
                    {
                        if (c is SvgMarkerElement)
                        {
                            if (this.MarkerStart != null && ((SvgMarkerElement)c).MarkerStart == null)
                            {
                                ((SvgMarkerElement)c).MarkerStart = this.MarkerStart;
                            }
                            if (this.MarkerMid != null && ((SvgMarkerElement)c).MarkerMid == null)
                            {
                                ((SvgMarkerElement)c).MarkerMid = this.MarkerMid;
                            }
                            if (this.MarkerEnd != null && ((SvgMarkerElement)c).MarkerEnd == null)
                            {
                                ((SvgMarkerElement)c).MarkerEnd = this.MarkerEnd;
                            }
                        }
                    }
                }
                markersSet = true;
            }
        }

        /// <summary>
        /// Add group markers to children before rendering them.
        /// This is only done on first rendering.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to render the child <see cref="SvgElement"/>s to.</param>
        protected override void Render(ISvgRenderer renderer)
        {
            AddMarkers();
            base.Render(renderer);
        }

        /// <summary>
        /// Gets the <see cref="System.Drawing.Drawing2D.GraphicsPath"/> for this element.
        /// </summary>
        /// <value></value>
        public override System.Drawing.Drawing2D.GraphicsPath Path(ISvgRenderer renderer)
        {
            return GetPaths(this, renderer);
        }

        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        /// <value>The bounds.</value>
        public override System.Drawing.RectangleF Bounds
        {
            get 
            { 
                var r = new RectangleF();
                foreach(var c in this.Children)
                {
                    if (c is SvgVisualElement)
                    {
                        // First it should check if rectangle is empty or it will return the wrong Bounds.
                        // This is because when the Rectangle is Empty, the Union method adds as if the first values where X=0, Y=0
                        if (r.IsEmpty)
                        {
                            r = ((SvgVisualElement)c).Bounds;
                        }
                        else
                        {
                            var childBounds = ((SvgVisualElement)c).Bounds;
                            if (!childBounds.IsEmpty)
                            {
                                r = RectangleF.Union(r, childBounds);
                            }
                        }
                    }
                }
                return TransformedBounds(r);
            }
        }

        protected override bool Renderable { get { return false; } }
                
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgGroup>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgGroup;
            if (this.Fill != null)
                newObj.Fill = this.Fill.DeepCopy() as SvgPaintServer;
            return newObj;
        }
    }
}
