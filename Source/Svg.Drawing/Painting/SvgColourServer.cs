using System;
using System.Drawing;

namespace Svg
{
    public partial class SvgColourServer : SvgPaintServer
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
