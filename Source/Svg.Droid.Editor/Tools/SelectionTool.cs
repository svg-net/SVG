using System;
using System.Linq;
using Android.Graphics;
using Android.Text;
using Android.Views;
using Android.Widget;

namespace Svg.Droid.Editor.Tools
{
    public class SelectionTool : ITool
    {
        private const int MaxClickDuration = 200;
        public const int SelectionBoxPadding = 40;

        private long _startClickTime;
        public Paint Paint { get; } = new Paint() { Color = Color.Rgb(33, 150, 243), StrokeWidth = 5 };

        public SelectionTool()
        {
            Paint.SetStyle(Paint.Style.Stroke);
            Paint.SetPathEffect(new DashPathEffect(new float[] {20, 40}, 0));
        }

        public void OnDraw(Canvas canvas, IPosition sItem)
        {
            if (sItem != null)
            {
                var cX = SharedMasterTool.Instance.CanvasTranslatedPosX;
                var cY = SharedMasterTool.Instance.CanvasTranslatedPosY;

                var left = cX + sItem.X;
                var top = cY + sItem.Y;
                var right = cX + sItem.X + sItem.Width;
                var bottom = cY + sItem.Y + sItem.Height;

                left -= SelectionBoxPadding;
                top -= SelectionBoxPadding;
                right += SelectionBoxPadding;
                bottom += SelectionBoxPadding;

                canvas.DrawRect(left, top, right, bottom, Paint);
            }
        }

        public void OnTouch(MotionEvent ev, SvgWorkspace svgWorkspace)
        {
            // if moving and something is selected ignore
            if (SvgWorkspaceModel.SelectedItem != null && ev.Action == MotionEventActions.Move)
                return;

            int action = (int) ev.Action;
            switch (action & (int) MotionEventActions.Mask)
            {
                case (int) MotionEventActions.Down:
                    // IsInRageOfAnyObject => QuadSearch
                    _startClickTime = Java.Lang.JavaSystem.CurrentTimeMillis();
                    break;
                case (int) MotionEventActions.Up:
                    long clickDuration = Java.Lang.JavaSystem.CurrentTimeMillis() - _startClickTime;
                    if (clickDuration < MaxClickDuration)
                    {
                        var relevantSvg = svgWorkspace.ViewModel.UpdateSelectedItem(
                            ev.GetX() - SharedMasterTool.Instance.CanvasTranslatedPosX,
                            ev.GetY() - SharedMasterTool.Instance.CanvasTranslatedPosY);

                        if (relevantSvg == null)
                        {
                            svgWorkspace.ViewModel.UnselectAll();
                            svgWorkspace.Invalidate();
                            return;
                        }

                        svgWorkspace.ViewModel.Select(relevantSvg);
                        svgWorkspace.Invalidate();
                    }

                    break;
            }
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