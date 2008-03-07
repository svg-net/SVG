using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    public sealed class SvgClipPath : SvgElement
    {
        private SvgCoordinateSystem _clipPathUnits;
        private bool _pathDirty;
        private Region _region;

        [SvgAttribute("clipPathUnits")]
        public SvgCoordinateSystem ClipPathUnits
        {
            get { return this._clipPathUnits; }
            set { this._clipPathUnits = value; }
        }

        public SvgClipPath()
        {
            this._clipPathUnits = SvgCoordinateSystem.UserSpaceOnUse;
        }

        public override string ElementName
        {
            get { return "clipPath"; }
        }

        public override object Clone()
        {
            SvgClipPath path = new SvgClipPath();
            path._clipPathUnits = this._clipPathUnits;
            return path;
        }

        public Region GetClipRegion()
        {
            if (_region == null || _pathDirty)
            {
                _region = new Region();

                foreach (SvgElement element in this.Children)
                    ComplementRegion(_region, element);

                _pathDirty = false;
            }

            return _region;
        }

        private void ComplementRegion(Region region, SvgElement element)
        {
            SvgGraphicsElement graphicsElement = element as SvgGraphicsElement;

            if (graphicsElement != null)
                region.Complement(graphicsElement.Path);

            foreach (SvgElement child in element.Children)
                ComplementRegion(region, element);
        }

        protected override void AddedElement(SvgElement child, int index)
        {
            base.AddedElement(child, index);
            this._pathDirty = true;
        }

        protected override void RemovedElement(SvgElement child)
        {
            base.RemovedElement(child);
            this._pathDirty = true;
        }

        protected override void Render(System.Drawing.Graphics graphics)
        {
            // Do nothing
        }
    }
}
