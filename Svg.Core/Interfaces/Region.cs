using System.Drawing;

namespace Svg
{
    public class Region
    {
        private readonly RectangleF _rect;

        public Region(RectangleF rect)
        {
            _rect = rect;
        }

        public Region(GraphicsPath path)
        {
            _rect = path.GetBounds();
        }

        public RectangleF Rect
        {
            get { return _rect; }
        }

        public Region Clone()
        {
            return new Region(Rect);
        }

        public void Exclude(GraphicsPath path)
        {
            throw new System.NotImplementedException();
        }

        public RectangleF GetBounds(Graphics graphics)
        {
            throw new System.NotImplementedException();
        }
    }
}