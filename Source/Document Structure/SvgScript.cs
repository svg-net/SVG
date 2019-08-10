using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    /// <summary>
    /// An element used to define scripts within SVG documents.
    /// </summary>
    [SvgElement("script")]
    public class SvgScript : SvgElement
    {
        public override SvgElement DeepCopy()
        {
            throw new System.NotImplementedException();
        }
    }
}