using System;
using System.Collections.Generic;
using System.Text;
#if NETFULL
using System.Drawing.Drawing2D;
using System.Drawing;
using SystemColor = System.Drawing.Color;
#else
using System.DrawingCore.Drawing2D;
using System.DrawingCore;
using SystemColor = System.DrawingCore.Color;
#endif

namespace Svg
{
    public sealed class SvgColourServer : SvgPaintServer
    {
    	
    	/// <summary>
        /// An unspecified <see cref="SvgPaintServer"/>.
        /// </summary>
        public static readonly SvgPaintServer NotSet = new SvgColourServer(SystemColor.Black);
        /// <summary>
        /// A <see cref="SvgPaintServer"/> that should inherit from its parent.
        /// </summary>
        public static readonly SvgPaintServer Inherit = new SvgColourServer(SystemColor.Black);

        public SvgColourServer()
            : this(SystemColor.Black)
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
            //is none?
            if (this == SvgPaintServer.None) return new SolidBrush(SystemColor.Transparent);

            // default fill color is black, default stroke color is none
            if (this == SvgColourServer.NotSet) return new SolidBrush(forStroke ? System.Drawing.Color.Transparent : System.Drawing.Color.Black);

            int alpha = (int)Math.Round((opacity * (this.Colour.A/255.0) ) * 255);
            Color colour = SystemColor.FromArgb(alpha, this.Colour);

            return new SolidBrush(colour);
        }

        public override string ToString()
        {
        	if(this == SvgPaintServer.None)
        		return "none";
        	else if(this == SvgColourServer.NotSet)
        		return "";
        	
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

            if ((this == SvgPaintServer.None && obj != SvgPaintServer.None) ||
                (this != SvgPaintServer.None && obj == SvgPaintServer.None) ||
                (this == SvgColourServer.NotSet && obj != SvgColourServer.NotSet) ||
                (this != SvgColourServer.NotSet && obj == SvgColourServer.NotSet) ||
                (this == SvgColourServer.Inherit && obj != SvgColourServer.Inherit) ||
                (this != SvgColourServer.Inherit && obj == SvgColourServer.Inherit)) return false;

            return this.GetHashCode() == objColor.GetHashCode();
        }

        public override int GetHashCode()
        {
            return _colour.GetHashCode();
        }
    }
}
