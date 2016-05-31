using System;
using Android.Graphics;
using Android.Text;
using Android.Views;
using Svg.Droid.Editor.Interfaces;

namespace Svg.Droid.Editor.Tools
{
    public class SnappingTool : ITool
    {
        public const int StepSize = 60;
        public static bool IsActive = false;

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
                case (int) MotionEventActions.Up:

                    var x = selectionService.SelectedItem.X;
                    var y = selectionService.SelectedItem.Y;

                    selectionService.SelectedItem.X = (x - ((x % 100) % StepSize));
                    selectionService.SelectedItem.Y = (y - ((y % 100) % StepSize));

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