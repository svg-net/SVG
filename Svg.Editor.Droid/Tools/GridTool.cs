using System;
using Android.Graphics;
using Android.Text;
using Android.Views;
using MvvmCross.Platform;
using Svg.Droid.Editor.Interfaces;

namespace Svg.Droid.Editor.Tools
{
    public class GridTool : ITool
    {
        public static bool IsVisible = true;

        public Paint Paint { get; } = new Paint() { Color = Color.Rgb(210, 210, 210), StrokeWidth = 1 };
        public Paint Paint2 { get; } = new Paint() { Color = Color.Rgb(210, 100, 210), StrokeWidth = 1 };

        public const float StepSize = 40;
        private double _length = 0;
        private const float MaxZoom = 1f;//ZoomTool.MaxScale;
        private static double A;
        private static double B;
        private static double C;
        private static float StepSizeX;
        private const double Alpha = 27.3f;
        private const double Gamma = 90f;
        private static double Beta;

        public GridTool()
        {
            // using triangle calculation to determine the x and y steps based on stepsize (y) and angle (alpha)
            // http://www.arndt-bruenner.de/mathe/scripts/Dreiecksberechnung.htm
            A = StepSize;
            Beta = 180f - (Alpha + Gamma);
            B = (A * SinDegree(Beta)) / SinDegree(Alpha);
            C = (A * SinDegree(Gamma)) / SinDegree(Alpha);
            StepSizeX = (float)B;

            Paint.SetStyle(Paint.Style.Stroke);
        }

        private static double SinDegree(double value)
        {
            return RadianToDegree(Math.Sin(DegreeToRadian(value)));
        }
        private static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
        private static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
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


            var canvasx = -SharedMasterTool.Instance.CanvasTranslatedPosX;
            var canvasy = -SharedMasterTool.Instance.CanvasTranslatedPosY;

            //for (var i = -canvas.Width * MaxZoom; i <= canvas.Width * MaxZoom; i += StepSize - 2.5f)
            //    DrawTopDownIsoLine(canvas, i, canvasx, canvasy);      /* | */

            var relativeCanvasTranslationX = (canvasx) % StepSizeX;
            var relativeCanvasTranslationY = (canvasy) % StepSize;

            var dist = Math.Max(canvas.Width, canvas.Height)*MaxZoom*2;
            var stepSize = (int) Math.Round(StepSize, 0);

            for (var i = -dist; i <= dist; i += stepSize)
            {
                DrawLineLeftToTop(canvas, i, canvasx - relativeCanvasTranslationX, canvasy - relativeCanvasTranslationY);       /* / */
                DrawLineLeftToBottom(canvas, i, canvasx - relativeCanvasTranslationX, canvasy - relativeCanvasTranslationY);    /* \ */
            }

            //canvas.DrawCircle(0, 0, 200, Paint2);
            //canvas.DrawCircle(canvasx, canvasy, 100, Paint2);
            //canvas.DrawCircle((-canvasx)+canvas.Width, (-canvasy)+canvas.Height, 100, Paint2);
        }

        // line looks like this -> /
        private void DrawLineLeftToTop(Canvas canvas, float y, float canvasX, float canvasY)
        {
            var startX = -(canvas.Width * MaxZoom) + canvasX;
            var startY = y + canvasY;
            var stopX = (-(canvas.Width * MaxZoom) + ((float)(_length * Math.Cos(Alpha * (Math.PI / 180))))) + canvasX;
            var stopY = (y - (float)(_length * Math.Sin(Alpha * (Math.PI / 180)))) + canvasY;

            canvas.DrawLine(
                startX,
                startY,
                stopX,
                stopY,
                Paint);
        }

        // line looks like this -> \
        private void DrawLineLeftToBottom(Canvas canvas, float y, float canvasX, float canvasY)
        {
            var startX = (-(canvas.Width * MaxZoom)) + canvasX;
            var startY = y + canvasY;
            var endX = (-(canvas.Width * MaxZoom) + ((float)(_length * Math.Cos(Alpha * (Math.PI / 180))))) + canvasX;
            var endY = (y + (float)(_length * Math.Sin(Alpha * (Math.PI / 180)))) + canvasY;

            canvas.DrawLine(
                startX,
                startY,
                endX,
                endY,
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
            return NullCommand;
        }

        public int DrawOrder => 100;
        public int CommandOrder => 10;
    }
}