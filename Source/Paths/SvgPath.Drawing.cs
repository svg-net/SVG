﻿#if !NO_SDC
using System.Drawing;
using System.Drawing.Drawing2D;
using Svg.Pathing;

namespace Svg
{
    public partial class SvgPath : SvgMarkerElement
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

                if (PathData != null && PathData.Count > 0 && PathData.First is SvgMoveToSegment)
                {
                    var start = PointF.Empty;
                    foreach (var segment in PathData)
                        start = segment.AddToPath(_path, start, PathData);

                    if (_path.PointCount == 0)
                    {
                        if (PathData.Count > 0)
                        {
                            // special case with one move command only, see #223
                            // make sure the case is valid, but nothing is drawn
                            var segment = PathData.Last;
                            _path.AddLine(segment.End, segment.End);
                            Fill = SvgPaintServer.None;
                            Stroke = SvgPaintServer.None;
                        }
                        else
                            _path = null;
                    }
                    else if (renderer == null)
                    {
                        // Calculate boundary including stroke width.
                        var radius = StrokeWidth * 2;
                        var bounds = _path.GetBounds();
                        _path.AddEllipse(bounds.Left - radius, bounds.Top - radius, 2 * radius, 2 * radius);
                        _path.AddEllipse(bounds.Right - radius, bounds.Bottom - radius, 2 * radius, 2 * radius);
                    }
                }

                if (renderer != null)
                    IsPathDirty = false;
            }
            return _path;
        }
    }
}
#endif
