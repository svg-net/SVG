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
        private SvgUnit _offset = 0f;

        /// <summary>
        /// Gets or sets the offset, i.e. where the stop begins from the beginning, of the gradient stop.
        /// </summary>
        [SvgAttribute("offset")]
        public SvgUnit Offset
        {
            get { return _offset; }
            set
            {
                SvgUnit unit = value;

                if (value.Type == SvgUnitType.Percentage)
                {
                    if (value.Value > 100f)
                    {
                        unit = new SvgUnit(value.Type, 100f);
                    }
                    else if (value.Value < 0f)
                    {
                        unit = new SvgUnit(value.Type, 0f);
                    }
                }
                else if (value.Type == SvgUnitType.User)
                {
                    if (value.Value > 1f)
                    {
                        unit = new SvgUnit(value.Type, 1f);
                    }
                    else if (value.Value < 0f)
                    {
                        unit = new SvgUnit(value.Type, 0f);
                    }
                }

                _offset = unit.ToPercentage();
                Attributes["offset"] = unit;
            }
        }

        /// <summary>
        /// Gets or sets the colour of the gradient stop.
        /// </summary>
        [SvgAttribute("stop-color")]
        [TypeConverter(typeof(SvgPaintServerFactory))]
        public override SvgPaintServer StopColor
        {
            get { return GetAttribute("stop-color", Inherited, SvgColourServer.NotSet); }
            set { Attributes["stop-color"] = value; }
        }

        /// <summary>
        /// Gets or sets the opacity of the gradient stop (0-1).
        /// </summary>
        [SvgAttribute("stop-opacity")]
        public override float Opacity
        {
            get { return GetAttribute("stop-opacity", Inherited, 1f); }
            set { Attributes["stop-opacity"] = FixOpacityValue(value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgGradientStop"/> class.
        /// </summary>
        public SvgGradientStop()
        {
            this._offset = new SvgUnit(0.0f);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgGradientStop"/> class.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="colour">The colour.</param>
        public SvgGradientStop(SvgUnit offset, Color colour)
        {
            this._offset = offset;
        }

        public Color GetColor(SvgElement parent)
        {
            var core = SvgDeferredPaintServer.TryGet<SvgColourServer>(this.StopColor, parent);
            if (core == null) throw new InvalidOperationException("Invalid paint server for gradient stop detected.");
            return core.Colour;
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgGradientStop>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgGradientStop;
            newObj.Offset = this.Offset;
            return newObj;
        }
    }
}
