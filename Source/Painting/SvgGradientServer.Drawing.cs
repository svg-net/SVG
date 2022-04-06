#if !NO_SDC
using System;
using System.Drawing.Drawing2D;

namespace Svg
{
    public abstract partial class SvgGradientServer : SvgPaintServer
    {
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
                position = Math.Min(Math.Max(position, 0f), 1f);
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

        protected SvgUnit NormalizeUnit(SvgUnit orig)
        {
            return orig.Type == SvgUnitType.Percentage && GradientUnits == SvgCoordinateUnits.ObjectBoundingBox ?
                new SvgUnit(SvgUnitType.User, orig.Value / 100f) : orig;
        }
    }
}
#endif
