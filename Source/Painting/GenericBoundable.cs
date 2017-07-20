using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if NETFULL
using System.Drawing.Drawing2D;
using System.Drawing;
#else
using System.DrawingCore.Drawing2D;
using System.DrawingCore;
#endif

namespace Svg
{
    internal class GenericBoundable : ISvgBoundable
    {
        private RectangleF _rect;

        public GenericBoundable(RectangleF rect)
        {
            _rect = rect;
        }
        public GenericBoundable(float x, float y, float width, float height)
        {
            _rect = new RectangleF(x, y, width, height);
        }

        public PointF Location
        {
            get { return _rect.Location; }
        }

        public SizeF Size
        {
            get { return _rect.Size; }
        }

        public RectangleF Bounds
        {
            get { return _rect; }
        }
    }
}
