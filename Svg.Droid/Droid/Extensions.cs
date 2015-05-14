using System.Drawing;
using Android.Graphics;

namespace Svg.Droid
{
    public static class Extensions
    {
        public static Rect ToRect(this Rectangle rect)
        {
            return new Rect((int)rect.X, (int)rect.Y, (int)(rect.X + rect.Width), (int)(rect.Y + rect.Height));
        }

        public static Rect ToRect(this RectangleF rect)
        {
            return new Rect((int)rect.X, (int)rect.Y, (int)(rect.X + rect.Width), (int)(rect.Y + rect.Height));
        }

        public static RectF ToRectF(this RectangleF rect)
        {
            return new RectF(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
        }

        public static Android.Graphics.Color ToColor(this System.Drawing.Color color)
        {
            return new Android.Graphics.Color(color.R, color.G, color.B, color.A);
        }

        public static Shader.TileMode ToTileMode(this WrapMode wrapMode)
        {
            Shader.TileMode tileMode = Shader.TileMode.Clamp;
            switch (wrapMode)
            {
                case WrapMode.Clamp:
                    tileMode = Shader.TileMode.Clamp;
                    break;
                case WrapMode.Tile:
                    tileMode = Shader.TileMode.Repeat;
                    break;
                case WrapMode.TileFlipX:
                case WrapMode.TileFlipXY:
                case WrapMode.TileFlipY:
                    tileMode = Shader.TileMode.Mirror;
                    break;
            }
            return tileMode;
        }

        public static Android.Graphics.PointF ToPointF(this System.Drawing.PointF point)
        {
            return new Android.Graphics.PointF(point.X, point.Y);
        }

        public static Android.Graphics.TypefaceStyle ToTypefaceStyle(this FontStyle value)
        {
            var tfs = TypefaceStyle.Normal;

            if ((value & FontStyle.Bold) == FontStyle.Bold &&
                (value & FontStyle.Italic) == FontStyle.Italic)
                tfs = TypefaceStyle.BoldItalic;
            else if ((value & FontStyle.Bold) == FontStyle.Bold)
                tfs = TypefaceStyle.Bold;
            else if ((value & FontStyle.Italic) == FontStyle.Italic)
                tfs = TypefaceStyle.Italic;
            else if ((value & FontStyle.Regular) == FontStyle.Regular)
                tfs = TypefaceStyle.Normal;

            return tfs;
        }
    }
}