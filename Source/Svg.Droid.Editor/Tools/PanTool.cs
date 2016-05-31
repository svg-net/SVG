using System;
using Android.Graphics;
using Android.Views;
using Svg.Droid.Editor.Interfaces;

namespace Svg.Droid.Editor.Tools
{
    public class PanTool : ITool
    {
        public void OnDraw(Canvas canvas, IPosition anyItemSelected)
        {
            canvas.Translate(SharedMasterTool.Instance.CanvasTranslatedPosX, SharedMasterTool.Instance.CanvasTranslatedPosY);
        }

        public void OnTouch(MotionEvent ev, SvgWorkspace svgWorkspace, ISelectionService selectionService)
        {
            if (selectionService.SelectedItem != null)
                return;

            var action = (int) ev.Action;
            switch (action & (int) MotionEventActions.Mask)
            {
                case (int)MotionEventActions.Move:
                    // Only move if the ScaleGestureDetector isn't processing a gesture.
                    if (!SharedMasterTool.Instance.IsScaleDetectorInProgress())
                    {
                        var pointerIndex = ev.FindPointerIndex(SharedMasterTool.Instance.ActivePointerId);
                        var x = ev.GetX(pointerIndex);
                        var y = ev.GetY(pointerIndex);

                        var dx = x - SharedMasterTool.Instance.LastTouchX;
                        var dy = y - SharedMasterTool.Instance.LastTouchY;

                        SharedMasterTool.Instance.CanvasTranslatedPosX += dx / ZoomTool.ScaleFactor;
                        SharedMasterTool.Instance.CanvasTranslatedPosY += dy / ZoomTool.ScaleFactor;

                        svgWorkspace.Invalidate();

                        SharedMasterTool.Instance.LastTouchX = x;
                        SharedMasterTool.Instance.LastTouchY = y;
                    }
                    else
                    {
                        var gx = SharedMasterTool.Instance.ScaleDetector.FocusX;
                        var gy = SharedMasterTool.Instance.ScaleDetector.FocusY;

                        var gdx = gx - SharedMasterTool.Instance.LastGestureX;
                        var gdy = gy - SharedMasterTool.Instance.LastGestureY;

                        SharedMasterTool.Instance.CanvasTranslatedPosX += gdx / ZoomTool.ScaleFactor;
                        SharedMasterTool.Instance.CanvasTranslatedPosY += gdy / ZoomTool.ScaleFactor;

                        svgWorkspace.Invalidate();

                        SharedMasterTool.Instance.LastGestureX = gx;
                        SharedMasterTool.Instance.LastGestureY = gy;
                    }

                    break;
            }
        }

        public void Reset()
        {
            SharedMasterTool.Instance.CanvasTranslatedPosX = 0.0f;
            SharedMasterTool.Instance.CanvasTranslatedPosY = 0.0f;
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