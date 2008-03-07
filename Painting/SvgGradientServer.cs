using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    public abstract class SvgGradientServer : SvgPaintServer
    {
        private SvgGradientUnit _gradientUnits;
        private SvgGradientSpreadMethod _spreadMethod = SvgGradientSpreadMethod.Pad;
        private SvgGradientServer _inheritGradient;
        private List<SvgGradientStop> _stops;

        internal SvgGradientServer()
        {
            this.GradientUnits = SvgGradientUnit.ObjectBoundingBox;
            this._stops = new List<SvgGradientStop>();
        }

        protected override void ElementAdded(SvgElement child, int index)
        {
            if (child is SvgGradientStop)
                this.Stops.Add((SvgGradientStop)child);
            base.ElementAdded(child, index);
        }

        protected override void ElementRemoved(SvgElement child)
        {
            if (child is SvgGradientStop)
                this.Stops.Add((SvgGradientStop)child);
            base.ElementRemoved(child);
        }

        public List<SvgGradientStop> Stops
        {
            get { return this._stops; }
        }

        [SvgAttribute("spreadMethod")]
        public SvgGradientSpreadMethod SpreadMethod
        {
            get { return this._spreadMethod; }
            set { this._spreadMethod = value; }
        }

        [SvgAttribute("gradientUnits")]
        public SvgGradientUnit GradientUnits
        {
            get { return this._gradientUnits; }
            set { this._gradientUnits = value; }
        }

        public SvgGradientServer InheritGradient
        {
            get { return this._inheritGradient; }
            set { this._inheritGradient = value; }
        }

        protected ColorBlend GetColourBlend(SvgGraphicsElement owner, float opacity)
        {
            ColorBlend blend = new ColorBlend();
            int colourBlends = this.Stops.Count;
            bool insertStart = false;
            bool insertEnd = false;

            //gradient.Transform = renderingElement.Transforms.Matrix;

            // May need to increase the number of colour blends because the range *must* be from 0.0 to 1.0.
            // E.g. 0.5 - 0.8 isn't valid therefore the rest need to be calculated.

            // If the first stop doesn't start at zero
            if (this.Stops[0].Offset.Value > 0)
            {
                colourBlends++;
                // Indicate that a colour has to be dynamically added at the start
                insertStart = true;
            }

            // If the last stop doesn't end at 1 a stop
            float lastValue = this.Stops[this.Stops.Count - 1].Offset.Value;
            if (lastValue < 100 || lastValue < 1)
            {
                colourBlends++;
                // Indicate that a colour has to be dynamically added at the end
                insertEnd = true;
            }

            blend.Positions = new float[colourBlends];
            blend.Colors = new Color[colourBlends];

            // Set positions and colour values
            int actualStops = 0;
            float mergedOpacity = 0.0f;
            float position = 0.0f;
            Color colour = Color.Black;

            for (int i = 0; i < colourBlends; i++)
            {
                mergedOpacity = opacity * this.Stops[actualStops].Opacity;
                position = (this.Stops[actualStops].Offset.ToDeviceValue(owner) / owner.Bounds.Width);
                colour = Color.FromArgb((int)(mergedOpacity * 255), this.Stops[actualStops++].Colour);

                // Insert this colour before itself at position 0
                if (insertStart && i == 0)
                {
                    blend.Positions[i] = 0.0f;
                    blend.Colors[i++] = colour;
                }

                blend.Positions[i] = position;
                blend.Colors[i] = colour;

                // Insert this colour after itself at position 0
                if (insertEnd && i == colourBlends - 2)
                {
                    blend.Positions[i + 1] = 1.0f;
                    blend.Colors[++i] = colour;
                }
            }

            return blend;
        }

        protected virtual List<SvgGradientStop> InheritStops()
        {
            List<SvgGradientStop> stops = new List<SvgGradientStop>();

            if (this.Stops.Count > 0)
                return stops;

            if (this.InheritGradient != null)
            {
                List<SvgGradientStop> ownerStops = this.InheritGradient.InheritStops();
                stops.AddRange(ownerStops);
            }

            return stops;
        }


    }
}