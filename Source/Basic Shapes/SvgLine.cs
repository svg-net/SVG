using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    /// <summary>
    /// Represents and SVG line element.
    /// </summary>
    [SvgElement("line")]
    public class SvgLine : SvgMarkerElement
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
            set 
            { 
            	if(_startX != value)
            	{
            		this._startX = value;
            		this.IsPathDirty = true;
            		OnAttributeChanged(new AttributeEventArgs{ Attribute = "x1", Value = value });
            	}
            }
        }

        [SvgAttribute("y1")]
        public SvgUnit StartY
        {
            get { return this._startY; }
            set 
            { 
            	if(_startY != value)
            	{
            		this._startY = value;
            		this.IsPathDirty = true;
            		OnAttributeChanged(new AttributeEventArgs{ Attribute = "y1", Value = value });
            	}
            }
        }

        [SvgAttribute("x2")]
        public SvgUnit EndX
        {
            get { return this._endX; }
            set 
            { 
            	if(_endX != value)
            	{
            		this._endX = value;
            		this.IsPathDirty = true;
            		OnAttributeChanged(new AttributeEventArgs{ Attribute = "x2", Value = value });
            	}
            }
        }

        [SvgAttribute("y2")]
        public SvgUnit EndY
        {
            get { return this._endY; }
            set 
            { 
            	if(_endY != value)
            	{
            		this._endY = value;
            		this.IsPathDirty = true;
            		OnAttributeChanged(new AttributeEventArgs{ Attribute = "y2", Value = value });
            	}
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

        public SvgLine()
        {
        }

        public override System.Drawing.Drawing2D.GraphicsPath Path(ISvgRenderer renderer)
        {
            if ((this._path == null || this.IsPathDirty) && base.StrokeWidth > 0)
            {
                PointF start = new PointF(this.StartX.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this), 
                                          this.StartY.ToDeviceValue(renderer, UnitRenderingType.Vertical, this));
                PointF end = new PointF(this.EndX.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this), 
                                        this.EndY.ToDeviceValue(renderer, UnitRenderingType.Vertical, this));

                this._path = new GraphicsPath();

                // If it is to render, don't need to consider stroke width.
                // i.e stroke width only to be considered when calculating boundary
                if (renderer != null)
                {
                  this._path.AddLine(start, end);
                  this.IsPathDirty = false;
                }
                else
                {	 // only when calculating boundary 
                  _path.StartFigure();
                  var radius = base.StrokeWidth / 2;
                  _path.AddEllipse(start.X - radius, start.Y - radius, 2 * radius, 2 * radius);
                  _path.AddEllipse(end.X - radius, end.Y - radius, 2 * radius, 2 * radius);
                  _path.CloseFigure();
                }
            }
            return this._path;
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
