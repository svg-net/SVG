using System.Linq;

namespace Svg.Droid
{
    public class AndroidColorMatrix : ColorMatrix
    {
        private Android.Graphics.ColorMatrix _matrix;
        public AndroidColorMatrix(float[][] elements)
        {
            _matrix = new Android.Graphics.ColorMatrix(elements.SelectMany(x => x).ToArray());
        }
    }
}