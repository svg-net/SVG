namespace Svg
{
    /// <summary>
    /// Represents and SVG line element.
    /// </summary>
    [SvgElement("line")]
    public partial class SvgLine : SvgMarkerElement
    {
        private SvgUnit _startX = 0f;
        private SvgUnit _startY = 0f;
        private SvgUnit _endX = 0f;
        private SvgUnit _endY = 0f;

        [SvgAttribute("x1")]
        public SvgUnit StartX
        {
            get { return _startX; }
            set
            {
                if (_startX != value)
                {
                    _startX = value;
                    IsPathDirty = true;
                }
                Attributes["x1"] = value;
            }
        }

        [SvgAttribute("y1")]
        public SvgUnit StartY
        {
            get { return _startY; }
            set
            {
                if (_startY != value)
                {
                    _startY = value;
                    IsPathDirty = true;
                }
                Attributes["y1"] = value;
            }
        }

        [SvgAttribute("x2")]
        public SvgUnit EndX
        {
            get { return _endX; }
            set
            {
                if (_endX != value)
                {
                    _endX = value;
                    IsPathDirty = true;
                }
                Attributes["x2"] = value;
            }
        }

        [SvgAttribute("y2")]
        public SvgUnit EndY
        {
            get { return _endY; }
            set
            {
                if (_endY != value)
                {
                    _endY = value;
                    IsPathDirty = true;
                }
                Attributes["y2"] = value;
            }
        }

        public override SvgPaintServer Fill
        {
            get { return null; /* Line can't have a fill */ }
            set
            {
                // Do nothing
            }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgLine>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgLine;

            newObj._startX = _startX;
            newObj._startY = _startY;
            newObj._endX = _endX;
            newObj._endY = _endY;
            return newObj;
        }
    }
}
