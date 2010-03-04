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
                this._offset = value.ToPercentage();
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
    }
}