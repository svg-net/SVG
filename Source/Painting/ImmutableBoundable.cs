using System.Drawing;

namespace Svg
{
    internal sealed class ImmutableBoundable : ISvgBoundable
    {
        private readonly RectangleF bounds;

        public ImmutableBoundable(ISvgBoundable boundable)
        {
            bounds = boundable.CalculateBounds();
        }

        public RectangleF CalculateBounds()
        {
            return bounds;
        }
    }
}