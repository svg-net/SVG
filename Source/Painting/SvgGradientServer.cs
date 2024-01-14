using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Svg.Transforms;

namespace Svg
{
    /// <summary>
    /// Provides the base class for all paint servers that wish to render a gradient.
    /// </summary>
    public abstract partial class SvgGradientServer : SvgPaintServer
    {
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
        public List<SvgGradientStop> Stops { get; } = new List<SvgGradientStop>();

        /// <summary>
        /// Specifies what happens if the gradient starts or ends inside the bounds of the target rectangle.
        /// </summary>
        [SvgAttribute("spreadMethod")]
        public SvgGradientSpreadMethod SpreadMethod
        {
            get { return GetAttribute("spreadMethod", false, SvgDeferredPaintServer.TryGet<SvgGradientServer>(InheritGradient, null)?.SpreadMethod ?? SvgGradientSpreadMethod.Pad); }
            set { Attributes["spreadMethod"] = value; }
        }

        /// <summary>
        /// Gets or sets the coordinate system of the gradient.
        /// </summary>
        [SvgAttribute("gradientUnits")]
        public SvgCoordinateUnits GradientUnits
        {
            get { return GetAttribute("gradientUnits", false, SvgDeferredPaintServer.TryGet<SvgGradientServer>(InheritGradient, null)?.GradientUnits ?? SvgCoordinateUnits.ObjectBoundingBox); }
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
            get { return GetAttribute("gradientTransform", false, SvgDeferredPaintServer.TryGet<SvgGradientServer>(InheritGradient, null)?.GradientTransform); }
            set { Attributes["gradientTransform"] = value; }
        }

        /// <summary>
        /// Gets or sets the colour of the gradient stop.
        /// </summary>
        [SvgAttribute("stop-color")]
        [TypeConverter(typeof(SvgPaintServerFactory))]
        public SvgPaintServer StopColor
        {
            get { return GetAttribute("stop-color", false, SvgDeferredPaintServer.TryGet<SvgGradientServer>(InheritGradient, null)?.StopColor ?? new SvgColourServer(System.Drawing.Color.Black)); }
            set { Attributes["stop-color"] = value; }
        }

        /// <summary>
        /// Gets or sets the opacity of the gradient stop (0-1).
        /// </summary>
        [SvgAttribute("stop-opacity")]
        public float StopOpacity
        {
            get { return GetAttribute("stop-opacity", false, SvgDeferredPaintServer.TryGet<SvgGradientServer>(InheritGradient, null)?.StopOpacity ?? 1f); }
            set { Attributes["stop-opacity"] = FixOpacityValue(value); }
        }

        protected static double CalculateDistance(PointF first, PointF second)
        {
            return Math.Sqrt(Math.Pow(first.X - second.X, 2) + Math.Pow(first.Y - second.Y, 2));
        }

        protected static float CalculateLength(PointF vector)
        {
            return (float)Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
        }

        private void LoadStops(SvgVisualElement parent)
        {
            Stops.RemoveAll(s => s.Parent != this);

            var gradient = this;
            while (gradient?.Stops.Count == 0)
                gradient = SvgDeferredPaintServer.TryGet<SvgGradientServer>(gradient.InheritGradient, parent);

            if (gradient != this && gradient != null)
                Stops.AddRange(gradient.Stops);
        }
    }
}
