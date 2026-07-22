#if !NO_SDC
using System.Drawing;

namespace Svg
{
    public partial class SvgCssVariablePaintServer : SvgPaintServer
    {
        /// <inheritdoc/>
        public override Brush GetBrush(SvgVisualElement styleOwner, ISvgRenderer renderer, float opacity, bool forStroke = false)
        {
            var resolved = Resolve(styleOwner);
            return resolved.GetBrush(styleOwner, renderer, opacity, forStroke);
        }
    }
}
#endif
