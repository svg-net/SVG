#if !NO_SDC
using System.Drawing;
using System.Drawing.Drawing2D;
using Svg.Pathing;

namespace Svg
{
    public partial class SvgGlyph : SvgPathBasedElement
    {
        private GraphicsPath _path;

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            if (_path == null || IsPathDirty)
            {
                _path = new GraphicsPath();

                if (PathData != null)
                {
                    var start = PointF.Empty;
                    foreach (var segment in PathData)
                        start = segment.AddToPath(_path, start, PathData);
                }

                IsPathDirty = false;
            }
            return _path;
        }
    }
}
#endif
