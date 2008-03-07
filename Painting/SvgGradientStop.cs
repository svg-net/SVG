using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace Svg
{
    public class SvgGradientStop : SvgElement
    {
        private SvgUnit _offset;
        private Color _colour;
        private float _opacity;

        [SvgAttribute("offset")]
        public SvgUnit Offset
        {
            get { return this._offset; }
            set { this._offset = value; }
        }

        [SvgAttribute("stop-color")]
        [TypeConverter(typeof(SvgColourConverter))]
        public Color Colour
        {
            get { return this._colour; }
            set { this._colour = value; }
        }

        [SvgAttribute("stop-opacity")]
        public float Opacity
        {
            get { return this._opacity; }
            set { this._opacity = value; }
        }

        protected override string ElementName
        {
            get { return "stop"; }
        }

        public SvgGradientStop()
        {
            this._offset = new SvgUnit(0.0f);
            this._colour = Color.Transparent;
            this._opacity = 1.0f;
        }

        public SvgGradientStop(SvgUnit offset, Color colour)
        {
            this._offset = offset;
            this._colour = colour;
            this._opacity = 1.0f;
        }
    }
}