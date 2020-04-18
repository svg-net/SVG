using System;
using System.ComponentModel;
using System.Drawing;

namespace Svg
{
    /// <summary>
    /// Represents a colour stop in a gradient.
    /// </summary>
    [SvgElement("stop")]
    public class SvgGradientStop : SvgElement
    {
        private SvgUnit _offset;

        /// <summary>
        /// Gets or sets the offset, i.e. where the stop begins from the beginning, of the gradient stop.
        /// </summary>
        [SvgAttribute("offset")]
        public SvgUnit Offset
        {
            get { return _offset; }
            set
            {
                var unit = value;
                if (unit.Type == SvgUnitType.Percentage)
                    unit = new SvgUnit(unit.Type, Math.Min(Math.Max(unit.Value, 0f), 100f));
                else if (unit.Type == SvgUnitType.User)
                    unit = new SvgUnit(unit.Type, Math.Min(Math.Max(unit.Value, 0f), 1f));

                _offset = unit.ToPercentage();
                Attributes["offset"] = unit;
            }
        }

        /// <summary>
        /// Gets or sets the colour of the gradient stop.
        /// </summary>
        [SvgAttribute("stop-color")]
        [TypeConverter(typeof(SvgPaintServerFactory))]
        public SvgPaintServer StopColor
        {
            get { return GetAttribute<SvgPaintServer>("stop-color", true, new SvgColourServer(System.Drawing.Color.Black)); }
            set { Attributes["stop-color"] = value; }
        }

        /// <summary>
        /// Gets or sets the opacity of the gradient stop (0-1).
        /// </summary>
        [SvgAttribute("stop-opacity")]
        public float StopOpacity
        {
            get { return GetAttribute("stop-opacity", true, 1f); }
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

            newObj._offset = _offset;
            return newObj;
        }
    }
}
