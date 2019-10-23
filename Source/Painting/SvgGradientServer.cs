using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using Svg.Transforms;

namespace Svg
{
    /// <summary>
    /// Provides the base class for all paint servers that wish to render a gradient.
    /// </summary>
    public abstract class SvgGradientServer : SvgPaintServer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SvgGradientServer"/> class.
        /// </summary>
        internal SvgGradientServer()
        {
            Stops = new List<SvgGradientStop>();
        }

        /// <summary>
        /// Called by the underlying <see cref="SvgElement"/> when an element has been added to the
        /// 'Children' collection.
        /// </summary>
        /// <param name="child">The <see cref="SvgElement"/> that has been added.</param>
        /// <param name="index">An <see cref="int"/> representing the index where the element was added to the collection.</param>
        protected override void AddElement(SvgElement child, int index)
        {
            if (child is SvgGradientStop)
                Stops.Add((SvgGradientStop)child);

            base.AddElement(child, index);
        }

        /// <summary>
        /// Called by the underlying <see cref="SvgElement"/> when an element has been removed from the
        /// 'Children' collection.
        /// </summary>
        /// <param name="child">The <see cref="SvgElement"/> that has been removed.</param>
        protected override void RemoveElement(SvgElement child)
        {
            if (child is SvgGradientStop)
                Stops.Remove((SvgGradientStop)child);

            base.RemoveElement(child);
        }

        /// <summary>
        /// Gets the ramp of colors to use on a gradient.
        /// </summary>
        public List<SvgGradientStop> Stops { get; private set; }

        /// <summary>
        /// Specifies what happens if the gradient starts or ends inside the bounds of the target rectangle.
        /// </summary>
        [SvgAttribute("spreadMethod")]
        public SvgGradientSpreadMethod SpreadMethod
        {
            get { return GetAttribute("spreadMethod", false, SvgGradientSpreadMethod.Pad); }
            set { Attributes["spreadMethod"] = value; }
        }

        /// <summary>
        /// Gets or sets the coordinate system of the gradient.
        /// </summary>
        [SvgAttribute("gradientUnits")]
        public SvgCoordinateUnits GradientUnits
        {
            get { return GetAttribute("gradientUnits", false, SvgCoordinateUnits.ObjectBoundingBox); }
            set { Attributes["gradientUnits"] = value; }
        }

        /// <summary>
        /// Gets or sets another gradient fill from which to inherit the stops from.
        /// </summary>
        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public SvgDeferredPaintServer InheritGradient
        {
            get { return GetAttribute<SvgDeferredPaintServer>("href", false); }
            set { Attributes["href"] = value; }
        }

        [SvgAttribute("gradientTransform")]
        public SvgTransformCollection GradientTransform
        {
            get { return GetAttribute<SvgTransformCollection>("gradientTransform", false); }
            set { Attributes["gradientTransform"] = value; }
        }

        /// <summary>
        /// Gets or sets the colour of the gradient stop.
        /// </summary>
        [SvgAttribute("stop-color")]
        [TypeConverter(typeof(SvgPaintServerFactory))]
        public SvgPaintServer StopColor
        {
            get { return GetAttribute<SvgPaintServer>("stop-color", false, new SvgColourServer(System.Drawing.Color.Black)); }
            set { Attributes["stop-color"] = value; }
        }

        /// <summary>
        /// Gets or sets the opacity of the gradient stop (0-1).
        /// </summary>
        [SvgAttribute("stop-opacity")]
        public float StopOpacity
        {
            get { return GetAttribute("stop-opacity", false, 1f); }
            set { Attributes["stop-opacity"] = FixOpacityValue(value); }
        }

        protected Matrix EffectiveGradientTransform
        {
            get
            {
                var transform = new Matrix();

                if (GradientTransform != null)
                    using (var matrix = GradientTransform.GetMatrix())
                        transform.Multiply(matrix);

                return transform;
            }
        }

        /// <summary>
        /// Gets a <see cref="ColorBlend"/> representing the <see cref="SvgGradientServer"/>'s gradient stops.
        /// </summary>
        /// <param name="renderer">The renderer <see cref="ISvgRenderer"/>.</param>
        /// <param name="opacity">The opacity of the colour blend.</param>
        /// <param name="radial">True if it's a radial gradiant.</param>
        protected ColorBlend GetColorBlend(ISvgRenderer renderer, float opacity, bool radial)
        {
            var colourBlends = Stops.Count;
            var insertStart = false;
            var insertEnd = false;

            //gradient.Transform = renderingElement.Transforms.Matrix;

            //stops should be processed in reverse order if it's a radial gradient

            // May need to increase the number of colour blends because the range *must* be from 0.0 to 1.0.
            // E.g. 0.5 - 0.8 isn't valid therefore the rest need to be calculated.

            // If the first stop doesn't start at zero
            if (Stops[0].Offset.Value > 0f)
            {
                colourBlends++;

                if (radial)
                    insertEnd = true;
                else
                    insertStart = true;
            }

            // If the last stop doesn't end at 1 a stop
            var lastValue = Stops[Stops.Count - 1].Offset.Value;
            if (lastValue < 100f || lastValue < 1f)
            {
                colourBlends++;
                if (radial)
                    insertStart = true;
                else
                    insertEnd = true;
            }

            var blend = new ColorBlend(colourBlends);

            // Set positions and colour values
            var actualStops = 0;

            for (var i = 0; i < colourBlends; i++)
            {
                var currentStop = Stops[radial ? Stops.Count - 1 - actualStops : actualStops];
                var boundWidth = renderer.GetBoundable().Bounds.Width;

                var mergedOpacity = opacity * currentStop.StopOpacity;
                var position =
                    radial
                    ? 1 - (currentStop.Offset.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this) / boundWidth)
                    : (currentStop.Offset.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this) / boundWidth);
                position = (float)Math.Round(position, 1, MidpointRounding.AwayFromZero);
                var colour = System.Drawing.Color.FromArgb((int)Math.Round(mergedOpacity * 255), currentStop.GetColor(this));

                actualStops++;

                // Insert this colour before itself at position 0
                if (insertStart && i == 0)
                {
                    blend.Positions[i] = 0.0f;
                    blend.Colors[i] = colour;

                    i++;
                }

                blend.Positions[i] = position;
                blend.Colors[i] = colour;

                // Insert this colour after itself at position 0
                if (insertEnd && i == colourBlends - 2)
                {
                    i++;

                    blend.Positions[i] = 1.0f;
                    blend.Colors[i] = colour;
                }
            }

            return blend;
        }

        protected void LoadStops(SvgVisualElement parent)
        {
            var core = SvgDeferredPaintServer.TryGet<SvgGradientServer>(InheritGradient, parent);
            if (Stops.Count == 0 && core != null)
                Stops.AddRange(core.Stops);
        }

        protected static double CalculateDistance(PointF first, PointF second)
        {
            return Math.Sqrt(Math.Pow(first.X - second.X, 2) + Math.Pow(first.Y - second.Y, 2));
        }

        protected static float CalculateLength(PointF vector)
        {
            return (float)Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
        }
    }
}
