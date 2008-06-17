using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    [TypeConverter(typeof(SvgPaintServerFactory))]
    public abstract class SvgPaintServer : SvgElement
    {
        public static readonly SvgPaintServer None = new SvgColourServer();

        public SvgPaintServer()
        {
            
        }

        protected override void Render(SvgRenderer renderer)
        {
            // Never render paint servers or their children
        }

        public abstract Brush GetBrush(SvgGraphicsElement styleOwner, float opacity);

        public override string ToString()
        {
            return String.Format("url(#{0})", this.ID);
        }
    }
}