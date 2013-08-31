using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    /// <summary>
    /// Represents and SVG line element.
    /// </summary>
    [SvgElement("line")]
    public class SvgLine : SvgVisualElement
    {
        private SvgUnit _startX;
        private SvgUnit _startY;
        private SvgUnit _endX;
        private SvgUnit _endY;
        private GraphicsPath _path;

        [SvgAttribute("x1")]
        public SvgUnit StartX
        {
            get { return this._startX; }
            set { this._startX = value; this.IsPathDirty = true; }
        }

        [SvgAttribute("y1")]
        public SvgUnit StartY
        {
            get { return this._startY; }
            set { this._startY = value; this.IsPathDirty = true; }
        }

        [SvgAttribute("x2")]
        public SvgUnit EndX
        {
            get { return this._endX; }
            set { this._endX = value; this.IsPathDirty = true; }
        }

        [SvgAttribute("y2")]
        public SvgUnit EndY
        {
            get { return this._endY; }
            set { this._endY = value; this.IsPathDirty = true; }
        }

        public override SvgPaintServer Fill
        {
            get { return null; /* Line can't have a fill */ }
            set
            {
                // Do nothing
            }
        }

        public SvgLine()
        {
        }

        public override GraphicsPath Path
        {
            get
            {
                if (this._path == null || this.IsPathDirty)
                {
                    PointF start = new PointF(this.StartX.ToDeviceValue(this), this.StartY.ToDeviceValue(this, true));
                    PointF end = new PointF(this.EndX.ToDeviceValue(this), this.EndY.ToDeviceValue(this, true));

                    this._path = new GraphicsPath();
                    this._path.AddLine(start, end);
                    this.IsPathDirty = false;
                }
                return this._path;
            }
        }

        public override RectangleF Bounds
        {
            get { return this.Path.GetBounds(); }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgLine>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgLine;
            newObj.StartX = this.StartX;
            newObj.EndX = this.EndX;
            newObj.StartY = this.StartY;
            newObj.EndY = this.EndY;
            if (this.Fill != null)
                newObj.Fill = this.Fill.DeepCopy() as SvgPaintServer;

            return newObj;
        }

    }
}
