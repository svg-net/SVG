using System;
using Android.Graphics;
using Android.Text;
using Android.Views;

namespace Svg.Droid.Editor.Tools
{
    public class GridTool : ITool
    {
        public static bool IsVisible = true;

        public Paint Paint { get; } = new Paint() { Color = Color.Rgb(238, 238, 238), StrokeWidth = 2 };
        private const int StepSize = 75;

        public GridTool()
        {
            Paint.SetStyle(Paint.Style.Stroke);
        }

        public void OnDraw(Canvas canvas, IPosition anyItemSelected)
        {
            if (!IsVisible)
                return;

            var length = Math.Sqrt((canvas.Width * canvas.Width) + (canvas.Height * canvas.Height));

            for (var i = -canvas.Height; i <= canvas.Height; i += (int) (StepSize/* * ZoomTool.ScaleFactor*/))
            {
                DrawLeftIsoLine(canvas, i, length, 27.3);
                DrawRightIsoLine(canvas, i, length, -41.5);
            }
        }

        private void DrawLeftIsoLine(Canvas canvas, float y, double length, double degrees)
        {
            var endX = ((float) (length * Math.Cos(degrees * (Math.PI / 180))));
            var endY = (y + (float)(length * Math.Sin(degrees * (Math.PI / 180))));

            canvas.DrawLine(
                (0),
                (y),
                (endX),
                (endY),
                Paint);
        }

        private void DrawRightIsoLine(Canvas canvas, float y, double length, double degrees)
        {
            var endX = ((float)(length * Math.Sin(degrees * (Math.PI / 180))));
            var endY = (y + (float)(length * Math.Cos(degrees * (Math.PI / 180))));

            canvas.DrawLine(
                (canvas.Width),
                (y),
                (endX),
                (endY),
                Paint);
        }

        public void OnTouch(MotionEvent ev, SvgWorkspace svgWorkspace)
        {
            
        }

        public void Reset()
        {

        }

        public Action Command()
        {
            return () => { };
        }

        public Action UndoCommand()
        {
            return () => { };
        }
    }
}