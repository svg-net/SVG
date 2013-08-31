using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    /// <summary>
    /// Provides the base class for all paint servers that wish to render a gradient.
    /// </summary>
    public abstract class SvgGradientServer : SvgPaintServer
    {
        private SvgCoordinateUnits _gradientUnits;
        private SvgGradientSpreadMethod _spreadMethod = SvgGradientSpreadMethod.Pad;
        private SvgGradientServer _inheritGradient;
        private readonly List<SvgGradientStop> _stops;

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgGradientServer"/> class.
        /// </summary>
        internal SvgGradientServer()
        {
            this.GradientUnits = SvgCoordinateUnits.ObjectBoundingBox;
            this._stops = new List<SvgGradientStop>();
        }

        /// <summary>
        /// Called by the underlying <see cref="SvgElement"/> when an element has been added to the
        /// <see cref="Children"/> collection.
        /// </summary>
        /// <param name="child">The <see cref="SvgElement"/> that has been added.</param>
        /// <param name="index">An <see cref="int"/> representing the index where the element was added to the collection.</param>
        protected override void AddElement(SvgElement child, int index)
        {
            var stop = child as SvgGradientStop;
            if (stop != null)
            {
                this.Stops.Add(stop);
            }

            base.AddElement(child, index);
        }

        /// <summary>
        /// Called by the underlying <see cref="SvgElement"/> when an element has been removed from the
        /// <see cref="Children"/> collection.
        /// </summary>
        /// <param name="child">The <see cref="SvgElement"/> that has been removed.</param>
        protected override void RemoveElement(SvgElement child)
        {
            var stop = child as SvgGradientStop;
            if (stop != null)
            {
                this.Stops.Remove(stop);
            }

            base.RemoveElement(child);
        }

        /// <summary>
        /// Gets the ramp of colors to use on a gradient.
        /// </summary>
        public List<SvgGradientStop> Stops
        {
            get { return this._stops; }
        }

        /// <summary>
        /// Specifies what happens if the gradient starts or ends inside the bounds of the target rectangle.
        /// </summary>
        [SvgAttribute("spreadMethod")]
        public SvgGradientSpreadMethod SpreadMethod
        {
            get { return this._spreadMethod; }
            set { this._spreadMethod = value; }
        }

        /// <summary>
        /// Gets or sets the coordinate system of the gradient.
        /// </summary>
        [SvgAttribute("gradientUnits")]
        public SvgCoordinateUnits GradientUnits
        {
            get { return this._gradientUnits; }
            set { this._gradientUnits = value; }
        }

        /// <summary>
        /// Gets or sets another gradient fill from which to inherit the stops from.
        /// </summary>
        [SvgAttribute("href")]
        public SvgGradientServer InheritGradient
        {
            get { return this._inheritGradient; }
            set 
            { 
                this._inheritGradient = value;
                this.InheritStops();
            }
        }

        /// <summary>
        /// Gets a <see cref="ColorBlend"/> representing the <see cref="SvgGradientServer"/>'s gradient stops.
        /// </summary>
        /// <param name="owner">The parent <see cref="SvgVisualElement"/>.</param>
        /// <param name="opacity">The opacity of the colour blend.</param>
        protected ColorBlend GetColourBlend(SvgVisualElement owner, float opacity)
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
            float mergedOpacity;
            float position;
            Color colour;

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

        /// <summary>
        // If this gradient contains no stops then it will search any inherited gradients for stops.
        /// </summary>
        protected virtual void InheritStops()
        {
            if (this.Stops.Count == 0 && this.InheritGradient != null)
            {
                _stops.AddRange(this.InheritGradient.Stops);
            }
        }


        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgGradientServer;
            newObj.SpreadMethod = this.SpreadMethod;
            newObj.GradientUnits = this.GradientUnits;
            newObj.InheritGradient = this.InheritGradient;
            return newObj;

        }

    }
}