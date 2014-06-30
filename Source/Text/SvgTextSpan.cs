using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using Svg.DataTypes;

namespace Svg
{
    [SvgElement("tspan")]
	public class SvgTextSpan : SvgElement
    {
		private SvgUnit _x;
		private SvgUnit _y;
		private SvgUnit _dx;
		private SvgUnit _dy;
        private FontData _fontData = new FontData();

		/// <summary>
		/// Gets or sets the X.
		/// </summary>
		/// <value>The X.</value>
		[SvgAttribute("x")]
		public SvgUnit X
        {
			get { return this._x; }
			set { this._x = value; }
		}

		/// <summary>
		/// Gets or sets the X.
		/// </summary>
		/// <value>The X.</value>
		[SvgAttribute("y")]
		public SvgUnit Y
        {
			get { return this._y; }
			set { this._y = value; }
		}


		/// <summary>
		/// Gets or sets the deltaX from the containing text.
		/// </summary>
		/// <value>The dX.</value>
		[SvgAttribute("dx")]
		public SvgUnit DX
		{
			get { return this._dx; }
			set { this._dx = value; }
		}

		/// <summary>
		/// Gets or sets the deltaY from the containing text.
		/// </summary>
		/// <value>The dY.</value>
		[SvgAttribute("dy")]
		public SvgUnit DY
		{
			get { return this._dy; }
			set { this._dy = value; }
		}

        /// <summary>
        /// Gets or sets the fill <see cref="SvgPaintServer"/> of this element.
        /// </summary>
        [SvgAttribute("fill")]
        public virtual SvgPaintServer Fill
        {
            get { return (this.Attributes["fill"] == null) ? SvgColourServer.NotSet : (SvgPaintServer)this.Attributes["fill"]; }
            set { this.Attributes["fill"] = value; }
        }

        /// <summary>
        /// Indicates which font family is to be used to render the text.
        /// </summary>
        [SvgAttribute("font-family")]
        public virtual string FontFamily
        {
            get { return this._fontData.FontFamily; }
            set { this._fontData.FontFamily = value; }
        }

        /// <summary>
        /// Refers to the size of the font from baseline to baseline when multiple lines of text are set solid in a multiline layout environment.
        /// </summary>
        [SvgAttribute("font-size")]
        public virtual SvgUnit FontSize
        {
            get { return this._fontData.FontSize; }
            set { this._fontData.FontSize = value; }
        }


        /// <summary>
        /// Refers to the boldness of the font.
        /// </summary>
        [SvgAttribute("font-weight")]
        public virtual SvgFontWeight FontWeight
        {
            get { return this._fontData.FontWeight; }
            set { this._fontData.FontWeight = value; }
        }

        /// <summary>
        /// Set all font information.
        /// </summary>
        [SvgAttribute("font")]
        public virtual string Font
        {
            get { return this._fontData.Font; }
            set { _fontData.Font = value; }
        }
		

		/// <summary>
		/// Gets or sets the text to be rendered.
		/// </summary>
		public virtual string Text
		{
			get { return base.Content; }
			set { base.Content = value; this.Content = value; }
		}



		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgTextSpan>();
		}

		public override SvgElement DeepCopy<T>()
		{
			var newObj = base.DeepCopy<T>() as SvgTextSpan;
			newObj.X = this.X;
			newObj.Y = this.Y;
			newObj.DX = this.DX;
			newObj.DY = this.DY;
			newObj.Text = this.Text;

			return newObj;
		}

        internal FontData FontInfo { get { return _fontData; } }
    }
}