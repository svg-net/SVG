using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace Svg
{
    /// <summary>
    /// Represents a colour stop in a gradient.
    /// </summary>
    [SvgElement("stop")]
    public class SvgGradientStop : SvgElement
    {
        private SvgUnit _offset;
        private Color _colour;
        private float _opacity;

        /// <summary>
        /// Gets or sets the offset, i.e. where the stop begins from the beginning, of the gradient stop.
        /// </summary>
        [SvgAttribute("offset")]
        public SvgUnit Offset
        {
            get { return this._offset; }
            set
            {
                SvgUnit unit = value;

                if (value.Type == SvgUnitType.Percentage)
                {
                    if (value.Value > 100)
                    {
                        unit = new SvgUnit(value.Type, 100);
                    }
                    else if (value.Value < 0)
                    {
                        unit = new SvgUnit(value.Type, 0);
                    }
                }
                else if (value.Type == SvgUnitType.User)
                {
                    if (value.Value > 1)
                    {
                        unit = new SvgUnit(value.Type, 1);
                    }
                    else if (value.Value < 0)
                    {
                        unit = new SvgUnit(value.Type, 0);
                    }
                }

                this._offset = unit.ToPercentage();
            }
        }

        /// <summary>
        /// Gets or sets the colour of the gradient stop.
        /// </summary>
        [SvgAttribute("stop-color")]
        [TypeConverter(typeof(SvgColourConverter))]
        public Color Colour
        {
            get { return this._colour; }
            set { this._colour = value; }
        }

        /// <summary>
        /// Gets or sets the opacity of the gradient stop (0-1).
        /// </summary>
        [SvgAttribute("stop-opacity")]
        public float Opacity
        {
            get { return this._opacity; }
            set { this._opacity = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgGradientStop"/> class.
        /// </summary>
        public SvgGradientStop()
        {
            this._offset = new SvgUnit(0.0f);
            this._colour = Color.Transparent;
            this._opacity = 1.0f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgGradientStop"/> class.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="colour">The colour.</param>
        public SvgGradientStop(SvgUnit offset, Color colour)
        {
            this._offset = offset;
            this._colour = colour;
            this._opacity = 1.0f;
        }


		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgGradientStop>();
		}

		public override SvgElement DeepCopy<T>()
		{
			var newObj = base.DeepCopy<T>() as SvgGradientStop;
			newObj.Offset = this.Offset;
			newObj.Colour = this.Colour;
			newObj.Opacity = this.Opacity;

			return newObj;
		}
    }
}