using System.Drawing;
#if !NO_SDC
using System.Drawing.Drawing2D;
#endif
using Svg.Pathing;

namespace Svg
{
    /// <summary>
    /// Represents an SVG path element.
    /// </summary>
    [SvgElement("path")]
    public partial class SvgPath : SvgMarkerElement, ISvgPathElement
    {
#if !NO_SDC
        private GraphicsPath _path;
#endif

        /// <summary>
        /// Gets or sets a <see cref="SvgPathSegmentList"/> of path data.
        /// </summary>
        [SvgAttribute("d")]
        public SvgPathSegmentList PathData
        {
            get { return GetAttribute<SvgPathSegmentList>("d", false); }
            set
            {
                var old = PathData;
                if (old != null)
                    old.Owner = null;
                Attributes["d"] = value;
                value.Owner = this;
                IsPathDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the length of the path.
        /// </summary>
        [SvgAttribute("pathLength")]
        public float PathLength
        {
            get { return GetAttribute<float>("pathLength", false); }
            set { Attributes["pathLength"] = value; }
        }

#if !NO_SDC
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
#endif

        public void OnPathUpdated()
        {
            IsPathDirty = true;
            OnAttributeChanged(new AttributeEventArgs { Attribute = "d", Value = Attributes.GetAttribute<SvgPathSegmentList>("d") });
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgPath>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgPath;

            if (newObj.PathData != null)
            {
                newObj.PathData.Owner = newObj;
                newObj.IsPathDirty = true;
            }
            return newObj;
        }
    }
}
