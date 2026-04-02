#if !NO_SDC
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;

namespace Svg
{
    public partial class SvgSwitch : SvgVisualElement
    {
        private readonly string _systemLanguageName = CultureInfo.CurrentCulture.Name.ToLower();
        private readonly string _systemLanguageShortName = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        /// <value></value>
        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            return GetPaths(this, renderer);
        }

        /// <inheritdoc/>
        public override RectangleF Bounds => BoundsFromChildren(e => e.Bounds, TransformedBounds);
        /// <inheritdoc/>
        public override RectangleF BoundsRelativeToTop => BoundsFromChildren(e => e.BoundsRelativeToTop, r => r);

        /// <summary>
        /// Renders the first <see cref="SvgElement"/> that either matches the system language,
        /// or has no "systemLanguage" attribute.
        /// Any "requiredExtensions" or "requiredFeatures" attribute is ignored.
        /// </summary>
        /// <param name="renderer">The <see cref="Graphics"/> object to render to.</param>
        protected override void Render(ISvgRenderer renderer)
        {
            if (!Visible || !Displayable)
                return;

            try
            {
                if (!PushTransforms(renderer))
                    return;

                SetClip(renderer);
                foreach (var element in Children)
                {
                    if (element.CustomAttributes.ContainsKey("systemLanguage"))
                    {
                        var languages = element.CustomAttributes["systemLanguage"].Split(',');
                        if (!languages.Contains(_systemLanguageName) && !languages.Contains(_systemLanguageShortName))
                        {
                            continue;
                        }
                    }

                    // only the first matching child element shall be rendered
                    element.RenderElement(renderer);
                    break;
                }

                ResetClip(renderer);
            }
            finally
            {
                PopTransforms(renderer);
            }
        }
    }
}
#endif
