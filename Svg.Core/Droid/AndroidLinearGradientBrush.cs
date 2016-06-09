using System.Drawing.Drawing2D;
using System.Linq;
using Android.Graphics;

namespace Svg.Droid
{
    public class AndroidLinearGradientBrush : LinearGradientBrush, IAndroidShader
    {
        private readonly PointF _start;
        private readonly PointF _end;
        private readonly Color _colorStart;
        private readonly Color _colorEnd;
        private LinearGradient _shader;

        public AndroidLinearGradientBrush(PointF start, PointF end, Color colorStart, Color colorEnd)
        {
            _start = start;
            _end = end;
            _colorStart = colorStart;
            _colorEnd = colorEnd;
        }

        public ColorBlend InterpolationColors { get; set; }

        public WrapMode WrapMode { get; set; }

        public void ApplyTo(Paint paint)
        {
            Shader.TileMode tileMode = Shader.TileMode.Clamp;
            switch (WrapMode)
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

            if(_shader != null)_shader.Dispose();

            if(InterpolationColors == null)
                _shader = new LinearGradient(_start.X, _start.Y, _end.X, _end.Y, _colorStart, _colorEnd, tileMode);
            else
            {
                _shader = new LinearGradient(_start.X, _start.Y, _end.X, _end.Y, InterpolationColors.Colors.Select(c => c.ToArgb()).ToArray(), InterpolationColors.Positions, tileMode);
            }

            paint.SetShader(_shader);
        }

        public void Dispose()
        {
            if (_shader != null)
            {
                _shader.Dispose();
                _shader = null;
            }
            
        }
    }
}