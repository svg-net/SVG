using System;
using Android.Graphics;
using Android.Text;
using Android.Views;
using Svg.Droid.Editor.Interfaces;

namespace Svg.Droid.Editor.Tools
{
    public class SnappingTool : ITool
    {
        private const float StepSize = GridTool.StepSize;
        public static bool IsActive = false;
        private float _initX;
        private float _initY;

        public SnappingTool()
        {
            IsActive = true;
        }

        public void OnDraw(Canvas canvas, IPosition anyItemSelected)
        {
            
        }

        public void OnTouch(MotionEvent ev, SvgWorkspace svgWorkspace, ISelectionService selectionService)
        {
            if (selectionService.SelectedItem == null)
                return;

            int action = (int) ev.Action;
            switch (action & (int) MotionEventActions.Mask)
            {
                case (int) MotionEventActions.Move:

                    var pointerIndex = ev.FindPointerIndex(SharedMasterTool.Instance.ActivePointerId);
                    var x = ev.GetX(pointerIndex);
                    var y = ev.GetY(pointerIndex);

                    var dx = x - SharedMasterTool.Instance.LastTouchX;
                    var dy = y - SharedMasterTool.Instance.LastTouchY;

                    selectionService.SelectedItem.X += (int) (dx / ZoomTool.ScaleFactor);
                    selectionService.SelectedItem.Y += (int) (dy / ZoomTool.ScaleFactor);

                    svgWorkspace.Invalidate();

                    SharedMasterTool.Instance.LastTouchX = x;
                    SharedMasterTool.Instance.LastTouchY = y;

                    //selectionService.SelectedItem.X = (int) (Math.Round((selectionService.SelectedItem.X) / StepSize) * StepSize);
                    //selectionService.SelectedItem.Y = (int) (Math.Round((selectionService.SelectedItem.Y) / StepSize) * StepSize);

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