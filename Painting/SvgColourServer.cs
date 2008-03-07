using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Svg
{
    public sealed class SvgColourServer : SvgPaintServer
    {
        public SvgColourServer() : this(Color.Transparent)
        {
        }

        public SvgColourServer(Color colour)
        {
            this._colour = colour;
        }

        private Color _colour;

        public Color Colour
        {
            get { return this._colour; }
            set { this._colour = value; }
        }

        public override Brush GetBrush(SvgGraphicsElement styleOwner, float opacity)
        {
            int alpha = (int)((opacity * (this.Colour.A/255) ) * 255);
            Color colour = Color.FromArgb(alpha, this.Colour);
            SolidBrush brush = new SolidBrush(colour);

            return brush;
        }

        public override string ToString()
        {
            Color c = this.Colour;

            // Return the name if it exists
            if (c.IsKnownColor)
                return c.Name;

            // Return the hex value
            return String.Format("#{0}", c.ToArgb().ToString("x").Substring(2));
        }
    }
}
