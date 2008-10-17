using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SvgClipPath : SvgElement
    {
        private SvgCoordinateUnits _clipPathUnits;
        private bool _pathDirty;
        private Region _region;

        /// <summary>
        /// 
        /// </summary>
        [SvgAttribute("clipPathUnits")]
        public SvgCoordinateUnits ClipPathUnits
        {
            get { return this._clipPathUnits; }
            set { this._clipPathUnits = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgClipPath"/> class.
        /// </summary>
        public SvgClipPath()
        {
            this._clipPathUnits = SvgCoordinateUnits.ObjectBoundingBox;
        }

        /// <summary>
        /// Gets this <see cref="SvgClipPath"/>'s region to be clipped.
        /// </summary>
        /// <returns>A new <see cref="Region"/> containing the area to be clipped.</returns>
        protected internal Region GetClipRegion()
        {
            if (_region == null || _pathDirty)
            {
                _region = new Region();

                foreach (SvgElement element in this.Children)
                {
                    ComplementRegion(_region, element);
                }

                _pathDirty = false;
            }

            return _region;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="element"></param>
        private void ComplementRegion(Region region, SvgElement element)
        {
            SvgGraphicsElement graphicsElement = element as SvgGraphicsElement;

            if (graphicsElement != null && graphicsElement.Path != null)
            {
                region.Complement(graphicsElement.Path);
            }

            foreach (SvgElement child in element.Children)
            {
                ComplementRegion(region, child);
            }
        }

        protected override void AddElement(SvgElement child, int index)
        {
            base.AddElement(child, index);
            this._pathDirty = true;
        }

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
