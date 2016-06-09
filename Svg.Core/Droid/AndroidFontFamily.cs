using System;
using Android.Graphics;

namespace Svg.Droid
{
    public class AndroidFontFamily : FontFamily
    {
        private readonly Typeface _typeface;
        private readonly string _name;

        public AndroidFontFamily(Typeface typeface, string name)
        {
            if (typeface == null) throw new ArgumentNullException("typeface");
            if (name == null) throw new ArgumentNullException("name");
            _typeface = typeface;
            _name = name;
        }

        public float GetCellAscent(FontStyle style)
        {
            using (var paint = new Paint())
            {
                paint.SetTypeface(Typeface.Create(_typeface, style.ToTypefaceStyle()));
                using (var metrics = paint.GetFontMetrics())
                {
                    return metrics.Ascent;
                }
            }
        }

        public float GetEmHeight(FontStyle style)
        {
            using (var paint = new Paint())
            {
                paint.SetTypeface(Typeface.Create(_typeface, style.ToTypefaceStyle()));
                using (var metrics = paint.GetFontMetrics())
                {
                    return metrics.Top;
                }
            }
        }

        public bool IsStyleAvailable(FontStyle fontStyle)
        {
            // TODO LX how to implement
            return true;
        }

        public string Name { get { return _name; } }

        public Typeface Typeface
        {
            get { return _typeface; }
        }
    }
}