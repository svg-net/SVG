using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    /// <summary>
    /// Defines a path that can be used by other <see cref="ISvgClipable"/> elements.
    /// </summary>
    [SvgElement("clipPath")]
    public sealed class SvgClipPath : SvgElement
    {
        private bool _pathDirty = true;

        /// <summary>
        /// Specifies the coordinate system for the clipping path.
        /// </summary>
        [SvgAttribute("clipPathUnits")]
        public SvgCoordinateUnits ClipPathUnits { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgClipPath"/> class.
        /// </summary>
        public SvgClipPath()
        {
            this.ClipPathUnits = SvgCoordinateUnits.ObjectBoundingBox;
        }

        /// <summary>
        /// Gets this <see cref="SvgClipPath"/>'s region to be used as a clipping region.
        /// </summary>
        /// <returns>A new <see cref="Region"/> containing the <see cref="Region"/> to be used for clipping.</returns>
        public Region GetClipRegion(SvgVisualElement owner)
        {
            var path = new GraphicsPath();

            if (this._pathDirty)
            {
                foreach (SvgElement element in this.Children)
                {
                    this.CombinePaths(path, element);
                }

                this._pathDirty = false;
            }

            return new Region(path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="element"></param>
        private void CombinePaths(GraphicsPath path, SvgElement element)
        {
            var graphicsElement = element as SvgVisualElement;

            if (graphicsElement != null && graphicsElement.Path != null)
            {
                path.FillMode = (graphicsElement.ClipRule == SvgClipRule.NonZero) ? FillMode.Winding : FillMode.Alternate;
                path.AddPath(graphicsElement.Path, false);
            }

            foreach (SvgElement child in element.Children)
            {
                this.CombinePaths(path, child);
            }
        }

        /// <summary>
        /// Called by the underlying <see cref="SvgElement"/> when an element has been added to the
        /// <see cref="Children"/> collection.
        /// </summary>
        /// <param name="child">The <see cref="SvgElement"/> that has been added.</param>
        /// <param name="index">An <see cref="int"/> representing the index where the element was added to the collection.</param>
        protected override void AddElement(SvgElement child, int index)
        {
            base.AddElement(child, index);
            this._pathDirty = true;
        }

        /// <summary>
        /// Called by the underlying <see cref="SvgElement"/> when an element has been removed from the
        /// <see cref="Children"/> collection.
        /// </summary>
        /// <param name="child">The <see cref="SvgElement"/> that has been removed.</param>
        protected override void RemoveElement(SvgElement child)
        {
            base.RemoveElement(child);
            this._pathDirty = true;
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="SvgRenderer"/> object.
        /// </summary>
        /// <param name="renderer">The <see cref="SvgRenderer"/> object to render to.</param>
        protected override void Render(SvgRenderer renderer)
        {
            // Do nothing
        }
    }
}
