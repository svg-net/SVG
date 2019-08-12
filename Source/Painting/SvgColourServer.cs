using System;
using System.Drawing;

namespace Svg
{
    public sealed class SvgColourServer : SvgPaintServer
    {
        public SvgColourServer()
            : this(System.Drawing.Color.Black)
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

        public override Brush GetBrush(SvgVisualElement styleOwner, ISvgRenderer renderer, float opacity, bool forStroke = false)
        {
            // is none?
            if (this == None) return new SolidBrush(System.Drawing.Color.Transparent);

            // default fill color is black, default stroke color is none
            if (this == NotSet && forStroke) return new SolidBrush(System.Drawing.Color.Transparent);

            int alpha = (int)Math.Round((opacity * (this.Colour.A / 255.0)) * 255);
            Color colour = System.Drawing.Color.FromArgb(alpha, this.Colour);

            return new SolidBrush(colour);
        }

        public override string ToString()
        {
            if (this == None)
                return "none";
            else if (this == NotSet)
                return string.Empty;
            else if (this == Inherit)
                return "inherit";

            Color c = this.Colour;
#if !NETSTANDARD20
            // Return the name if it exists
            if (c.IsKnownColor)
                return c.Name;
#endif
            // Return the hex value
            return String.Format("#{0}", c.ToArgb().ToString("x8").Substring(2));
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgColourServer>();
        }

        public override SvgElement DeepCopy<T>()
        {
            if (this == None || this == Inherit || this == NotSet)
                return this;

            var newObj = base.DeepCopy<T>() as SvgColourServer;

            newObj.Colour = Colour;
            return newObj;
        }

        public override bool Equals(object obj)
        {
            var objColor = obj as SvgColourServer;
            if (objColor == null)
                return false;

            if ((this == None && obj != None) || (this != None && obj == None) ||
                (this == NotSet && obj != NotSet) || (this != NotSet && obj == NotSet) ||
                (this == Inherit && obj != Inherit) || (this != Inherit && obj == Inherit))
                return false;

            return this.GetHashCode() == objColor.GetHashCode();
        }

        public override int GetHashCode()
        {
            return _colour.GetHashCode();
        }
    }
}
