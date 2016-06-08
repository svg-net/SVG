using System;
using System.Linq;
using Android.Graphics;
using Android.Views;
using Svg.Droid.Editor.Interfaces;

namespace Svg.Droid.Editor.Tools
{
    public class MoveSvgTool : ITool
    {
        public void OnDraw(Canvas canvas, IPosition anyItemSelected)
        {
            //canvas.Translate(_translatedPosX, _translatedPosY);
        }

        public void OnTouch(MotionEvent ev, SvgWorkspace svgWorkspace, ISelectionService selectionService)
        {
            var selectedItem = selectionService.SelectedItem;
            if (selectedItem == null)
                return;

            int action = (int) ev.Action;
            switch (action & (int) MotionEventActions.Mask)
            {
                case (int) MotionEventActions.Move:
                    if (!SharedMasterTool.Instance.IsScaleDetectorInProgress() && !SnappingTool.IsActive)
                    {
                        var pointerIndex = ev.FindPointerIndex(SharedMasterTool.Instance.ActivePointerId);
                        var x = ev.GetX(pointerIndex);
                        var y = ev.GetY(pointerIndex);

                        var dx = x - SharedMasterTool.Instance.LastTouchX;
                        var dy = y - SharedMasterTool.Instance.LastTouchY;

                        selectedItem.X += (int) (dx / ZoomTool.ScaleFactor);
                        selectedItem.Y += (int) (dy / ZoomTool.ScaleFactor);

                        svgWorkspace.Invalidate();

                        SharedMasterTool.Instance.LastTouchX = x;
                        SharedMasterTool.Instance.LastTouchY = y;
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

        public int DrawOrder => 20;
        public int CommandOrder => 20;
    }
}