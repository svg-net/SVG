using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace Svg
{
    public class SvgGroup : SvgGraphicsElement
    {
        public SvgGroup()
        {
        }

        public override System.Drawing.Drawing2D.GraphicsPath Path
        {
            get { return null; }
        }

        public override System.Drawing.RectangleF Bounds
        {
            get { return new System.Drawing.RectangleF(); }
        }

        protected override void Render(System.Drawing.Graphics graphics)
        {
            this.PushTransforms(graphics);
            base.RenderContents(graphics);
            this.PopTransforms(graphics);
        }
    }
}