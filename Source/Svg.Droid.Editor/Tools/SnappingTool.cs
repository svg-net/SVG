using System;
using Android.Graphics;
using Android.Text;
using Android.Views;
using Svg.Droid.Editor.Interfaces;

namespace Svg.Droid.Editor.Tools
{
    public class SnappingTool : ITool
    {
        public const float StepSize = GridTool.StepSize;
        public const float StepSizeW = 38.749f;
        public static bool IsActive = false;
        private float _downX, _downY;
        private int _downSelectedItemX;
        private int _downSelectedItemY;

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
                case (int)MotionEventActions.Down:
                    _downX = ev.GetX();
                    _downY = ev.GetY();
                    _downSelectedItemX = selectionService.SelectedItem.X;
                    _downSelectedItemY = selectionService.SelectedItem.Y;

                    break;

                case (int) MotionEventActions.Move:
                    var currentX = ev.GetX();
                    var currentY = ev.GetY();

                    selectionService.SelectedItem.X = (int) (Math.Round((currentX - _downX) / StepSizeW) * StepSizeW) + _downSelectedItemX;
                    selectionService.SelectedItem.Y = (int) (Math.Round((currentY - _downY) / StepSize) * StepSize) + _downSelectedItemY;

                    svgWorkspace.Invalidate();

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