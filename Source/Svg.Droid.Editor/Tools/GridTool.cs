using System;
using Android.Graphics;
using Android.Text;
using Android.Views;
using Svg.Droid.Editor.Interfaces;

namespace Svg.Droid.Editor.Tools
{
    public class GridTool : ITool
    {
        public static bool IsVisible = true;

        public Paint Paint { get; } = new Paint() { Color = Color.Rgb(210, 210, 210), StrokeWidth = 1 };
        public const int StepSize = 80;
        private double _length = 0;
        private const float MaxZoom = ZoomTool.MaxScale;
        private const float Degrees = 27.3f;

        public GridTool()
        {
            Paint.SetStyle(Paint.Style.Stroke);
        }

        public void OnDraw(Canvas canvas, IPosition anyItemSelected)
        {
            //--------------------------------------------------
            // TODO DO THIS ONLY ONCE; NOT FOR EVERY ON DRAW
            //--------------------------------------------------

            if (!IsVisible)
                return;

            if(_length <= 0) // compute this only once
                _length = Math.Sqrt((canvas.Width * canvas.Width) + (canvas.Height * canvas.Height)) * MaxZoom * 2;

            for (var i = -canvas.Width * MaxZoom; i <= canvas.Width * MaxZoom; i += StepSize - 2.5f)
                DrawTopDownIsoLine(canvas, i);      /* | */

            for (var i = -canvas.Height * MaxZoom * 2; i <= canvas.Height * 2 * MaxZoom; i += (int) (StepSize))
            {
                DrawLineLeftToTop(canvas, i);       /* / */
                DrawLineLeftToBottom(canvas, i);    /* \ */
            }
        }

        // line looks like this -> /
        private void DrawLineLeftToTop(Canvas canvas, float y)
        {
            var endX = -(canvas.Width * MaxZoom) + ((float)(_length * Math.Cos(Degrees * (Math.PI / 180))));
            var endY = (y - (float)(_length * Math.Sin(Degrees * (Math.PI / 180))));

            canvas.DrawLine(
                (-(canvas.Width * MaxZoom)),
                (y),
                (endX),
                (endY),
                Paint);
        }

        // line looks like this -> \
        private void DrawLineLeftToBottom(Canvas canvas, float y)
        {
            var endX = -(canvas.Width * MaxZoom) + ((float)(_length * Math.Cos(Degrees * (Math.PI / 180))));
            var endY = (y + (float)(_length * Math.Sin(Degrees * (Math.PI / 180))));

            canvas.DrawLine(
                (-(canvas.Width * MaxZoom)),
                (y),
                (endX),
                (endY),
                Paint);
        }

        // line looks like this -> |
        private void DrawTopDownIsoLine(Canvas canvas, float y)
        {
            if(ZoomTool.ScaleFactor < 0.85f)
                return;

            canvas.DrawLine(
                (y),
                (-(canvas.Height * MaxZoom)),
                (y),
                (canvas.Height * MaxZoom),
                Paint);
        }

        public void OnTouch(MotionEvent ev, SvgWorkspace svgWorkspace, ISelectionService selectionService)
        {
            // You know nothing Jon Snow
        }

        public void Reset()
        {
            // You know nothing Jon Snow
        }

        public static Action NullCommand = () => { };

        public Action Command()
        {
            return NullCommand;
        }

        public Action UndoCommand()
        {
            return () => { };
        }
    }
}