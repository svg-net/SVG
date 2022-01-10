#if !NO_SDC
using System.Linq;
using System.Drawing;

namespace Svg
{
    public partial class SvgFallbackPaintServer : SvgPaintServer
    {
        public override Brush GetBrush(SvgVisualElement styleOwner, ISvgRenderer renderer, float opacity, bool forStroke = false)
        {
            try
            {
                _primary.GetCallback = () => _fallbacks.FirstOrDefault();
                return _primary.GetBrush(styleOwner, renderer, opacity, forStroke);
            }
            finally
            {
                _primary.GetCallback = null;
            }
        }
    }
}
#endif
