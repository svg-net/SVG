using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Svg
{
    [SvgElement("tspan")]
	public class SvgTextSpan : SvgElement
    {
		private SvgUnit _x;
		private SvgUnit _y;
		private SvgUnit _dx;
		private SvgUnit _dy;

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


    }
}