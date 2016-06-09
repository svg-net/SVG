using Android.Graphics;

namespace Svg.Droid
{
    public class AndroidFont : Font
    {
        private FontStyle _style;
        private AndroidFontFamily _fontFamily;
        private Paint _paint;

        public AndroidFont(AndroidFontFamily fontFamily)
        {
            _fontFamily = fontFamily;
            _paint = new Paint();
            _paint.SetTypeface(_fontFamily.Typeface);
        }

        public void Dispose()
        {
            if (_paint != null)
            {
                _paint.Dispose();
                _paint = null;
            }
        }

        public float Size
        {
            get { return _paint.TextSize; }
            set { _paint.TextSize = value; }
        }

        public float SizeInPoints
        {
            get { return _paint.TextSize; }
            set { _paint.TextSize = value; }
        }

        public FontStyle Style
        {
            get { return _style; }
            set
            {
                _style = value;
                _paint.SetTypeface(Typeface.Create(_fontFamily.Typeface, value.ToTypefaceStyle()));
            }
        }

        public FontFamily FontFamily { get { return _fontFamily; } }
    }
}