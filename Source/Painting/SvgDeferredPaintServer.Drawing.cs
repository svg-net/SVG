using System.Drawing;

namespace Svg
{
    public partial class SvgDeferredPaintServer : SvgPaintServer
    {
        public override Brush GetBrush(SvgVisualElement styleOwner, ISvgRenderer renderer, float opacity, bool forStroke = false)
        {
            EnsureServer(styleOwner);
            return _concreteServer?.GetBrush(styleOwner, renderer, opacity, forStroke) ?? _fallbackServer?.GetBrush(styleOwner, renderer, opacity, forStroke) ?? NotSet?.GetBrush(styleOwner, renderer, opacity, forStroke);
        }
    }
}
