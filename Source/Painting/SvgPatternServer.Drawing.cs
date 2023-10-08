#if !NO_SDC
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Svg
{
    public partial class SvgPatternServer : SvgPaintServer, ISvgViewPort
    {
        private Matrix EffectivePatternTransform
        {
            get
            {
                var transform = new Matrix();
                if (PatternTransform != null)
                    using (var matrix = PatternTransform.GetMatrix())
                        transform.Multiply(matrix);

                return transform;
            }
        }

        /// <summary>
        /// Gets a <see cref="Brush"/> representing the current paint server.
        /// </summary>
        /// <param name="renderingElement">The owner <see cref="SvgVisualElement"/>.</param>
        /// <param name="renderer">The renderer object.</param>
        /// <param name="opacity">The opacity of the brush.</param>
        /// <param name="forStroke">Not used.</param>
        public override Brush GetBrush(SvgVisualElement renderingElement, ISvgRenderer renderer, float opacity, bool forStroke = false)
        {
            var chain = new List<SvgPatternServer>();

            var curr = this;
            do
            {
                chain.Add(curr);
                curr = SvgDeferredPaintServer.TryGet<SvgPatternServer>(curr.InheritGradient, renderingElement);
            } while (curr != null);

            var firstChildren = chain.Find(p => p.Children.Count > 0);
            if (firstChildren == null)
                return null;
            var firstX = chain.Find(p => p.X != SvgUnit.None);
            var firstY = chain.Find(p => p.Y != SvgUnit.None);
            var firstWidth = chain.Find(p => p.Width != SvgUnit.None);
            var firstHeight = chain.Find(p => p.Height != SvgUnit.None);
            if (firstWidth == null || firstHeight == null)
                return null;
            var firstPatternUnit = chain.Find(p => p._patternUnits.HasValue);
            var firstPatternContentUnit = chain.Find(p => p._patternContentUnits.HasValue);
            var firstViewBox = chain.Find(p => p.ViewBox != SvgViewBox.Empty);

            var xUnit = firstX == null ? new SvgUnit(0f) : firstX.X;
            var yUnit = firstY == null ? new SvgUnit(0f) : firstY.Y;
            var widthUnit = firstWidth.Width;
            var heightUnit = firstHeight.Height;

            var patternUnits = firstPatternUnit == null ? SvgCoordinateUnits.ObjectBoundingBox : firstPatternUnit.PatternUnits;
            var patternContentUnits = firstPatternContentUnit == null ? SvgCoordinateUnits.UserSpaceOnUse : firstPatternContentUnit.PatternContentUnits;
            var viewBox = firstViewBox == null ? SvgViewBox.Empty : firstViewBox.ViewBox;

            var isPatternObjectBoundingBox = patternUnits == SvgCoordinateUnits.ObjectBoundingBox;
            try
            {
                if (isPatternObjectBoundingBox)
                    renderer.SetBoundable(renderingElement);

                var x = xUnit.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this);
                var y = yUnit.ToDeviceValue(renderer, UnitRenderingType.Vertical, this);
                var width = widthUnit.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this);
                var height = heightUnit.ToDeviceValue(renderer, UnitRenderingType.Vertical, this);

                if (isPatternObjectBoundingBox)
                {
                    var bounds = renderer.GetBoundable().Bounds;    // Boundary without stroke is expect...

                    if (xUnit.Type != SvgUnitType.Percentage)
                        x *= bounds.Width;
                    if (yUnit.Type != SvgUnitType.Percentage)
                        y *= bounds.Height;
                    if (widthUnit.Type != SvgUnitType.Percentage)
                        width *= bounds.Width;
                    if (heightUnit.Type != SvgUnitType.Percentage)
                        height *= bounds.Height;
                    x += bounds.X;
                    y += bounds.Y;
                }

                if (width <= 0f || height <= 0f)
                    return null;

                var tile = new Bitmap((int)Math.Ceiling(width), (int)Math.Ceiling(height));
                using (var tileRenderer = SvgRenderer.FromImage(tile))
                {
                    tileRenderer.SetBoundable(renderingElement);
                    if (viewBox != SvgViewBox.Empty)
                    {
                        var bounds = tileRenderer.GetBoundable().Bounds;
                        tileRenderer.ScaleTransform(width / viewBox.Width, height / viewBox.Height);
                    }
                    else if (patternContentUnits == SvgCoordinateUnits.ObjectBoundingBox)
                    {
                        var bounds = tileRenderer.GetBoundable().Bounds;
                        tileRenderer.ScaleTransform(bounds.Width, bounds.Height);
                    }

                    foreach (var child in firstChildren.Children)
                        child.RenderElement(tileRenderer);
                }

                using (var transform = EffectivePatternTransform)
                {
                    var textureBrush = new TextureBrush(tile, new RectangleF(0f, 0f, width, height))
                    {
                        Transform = transform
                    };
                    textureBrush.TranslateTransform(x, y);
                    return textureBrush;
                }
            }
            finally
            {
                if (isPatternObjectBoundingBox)
                    renderer.PopBoundable();
            }
        }
    }
}
#endif
