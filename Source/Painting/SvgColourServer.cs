using System;
using System.Drawing;

namespace Svg
{
    public sealed class SvgColourServer : SvgPaintServer
    {
        public SvgColourServer() : this(Color.Black)
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

        public override Brush GetBrush(SvgVisualElement styleOwner, float opacity)
        {
            int alpha = (int)((opacity * (this.Colour.A/255.0f) ) * 255);
            Color colour = Color.FromArgb(alpha, this.Colour);

            return new SolidBrush(colour);
        }

        public override string ToString()
        {
            Color c = this.Colour;

            // Return the name if it exists
            if (c.IsKnownColor)
            {
                return c.Name;
            }

            // Return the hex value
            return String.Format("#{0}", c.ToArgb().ToString("x").Substring(2));
        }


        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgColourServer>();
        }


        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgColourServer;
            newObj.Colour = this.Colour;
            return newObj;

        }

        public override bool Equals(object obj)
        {
            var objColor = obj as SvgColourServer;
            if (objColor == null)
                return false;

            return this.GetHashCode() == objColor.GetHashCode();
        }

        public override int GetHashCode()
        {
            return _colour.GetHashCode();
        }
    }
}
