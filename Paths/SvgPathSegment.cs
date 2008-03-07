using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.Pathing
{
    public abstract class SvgPathSegment
    {
        private PointF _start;
        private PointF _end;

        public PointF Start
        {
            get { return this._start; }
            set { this._start = value; }
        }

        public PointF End
        {
            get { return this._end; }
            set { this._end = value; }
        }

        public abstract void AddToPath(GraphicsPath graphicsPath);
    }
}
