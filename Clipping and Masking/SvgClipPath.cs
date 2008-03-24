using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    public sealed class SvgClipPath : SvgElement
    {
        private SvgCoordinateUnits _clipPathUnits;
        private bool _pathDirty;
        private Region _region;

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
        /// Gets the name of the element.
        /// </summary>
        protected override string ElementName
        {
            get { return "clipPath"; }
        }

        private Region GetClipRegion()
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

        private void ComplementRegion(Region region, SvgElement element)
        {
            SvgGraphicsElement graphicsElement = element as SvgGraphicsElement;

            if (graphicsElement != null)
            {
                region.Complement(graphicsElement.Path);
            }

            foreach (SvgElement child in element.Children)
            {
                ComplementRegion(region, element);
            }
        }

        protected override void ElementAdded(SvgElement child, int index)
        {
            base.ElementAdded(child, index);
            this._pathDirty = true;
        }

        protected override void ElementRemoved(SvgElement child)
        {
            base.ElementRemoved(child);
            this._pathDirty = true;
        }

        protected override void Render(System.Drawing.Graphics graphics)
        {
            // Do nothing
        }
    }
}
