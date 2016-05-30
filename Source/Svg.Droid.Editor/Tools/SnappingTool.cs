using System;
using Android.Graphics;
using Android.Text;
using Android.Views;

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

        public void OnTouch(MotionEvent ev, SvgWorkspace svgWorkspace)
        {
            if (SvgWorkspaceModel.SelectedItem == null)
                return;

            int action = (int)ev.Action;
            switch (action & (int) MotionEventActions.Mask)
            {
                case (int) MotionEventActions.Up:

                    var x = SvgWorkspaceModel.SelectedItem.X;
                    var y = SvgWorkspaceModel.SelectedItem.Y;

                    SvgWorkspaceModel.SelectedItem.X = (x - ((x % 100) % StepSize));
                    SvgWorkspaceModel.SelectedItem.Y = (y - ((y % 100) % StepSize));

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