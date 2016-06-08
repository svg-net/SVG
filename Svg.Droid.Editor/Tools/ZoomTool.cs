using System;
using Android.Graphics;
using Android.Views;
using Svg.Droid.Editor.Interfaces;

namespace Svg.Droid.Editor.Tools
{
    public class ZoomTool : ITool
    {
        public static float ScaleFactor = 1.0f;
        public static bool IsActive = false;
        public const float MinScale = 0.5f;
        public const float MaxScale = 3.0f;

        public ZoomTool()
        {
            IsActive = true;
        }

        public class ScaleListener : ScaleGestureDetector.SimpleOnScaleGestureListener
        {
            private readonly View _view;
            private readonly ISelectionService _selectionService;

            public ScaleListener(View view, ISelectionService selectionService)
            {
                _view = view;
                _selectionService = selectionService;
            }

            public override bool OnScale(ScaleGestureDetector detector)
            {
                if (_selectionService.SelectedItem != null)
                    return false;

                ScaleFactor *= detector.ScaleFactor;

                // Don't let the object get too small or too large.
                ScaleFactor = Math.Max(MinScale, Math.Min(ScaleFactor, MaxScale));

                _view.Invalidate();
                return true;
            }
        }

        public void OnDraw(Canvas canvas, IPosition anyItemSelected)
        {
            if (SharedMasterTool.Instance.IsScaleDetectorInProgress())
                canvas.Scale(ScaleFactor, ScaleFactor, SharedMasterTool.Instance.ScaleDetector.FocusX, SharedMasterTool.Instance.ScaleDetector.FocusY);
            else
                canvas.Scale(ScaleFactor, ScaleFactor, SharedMasterTool.Instance.LastGestureX, SharedMasterTool.Instance.LastGestureY);
        }

        public void OnTouch(MotionEvent ev, SvgWorkspace svgWorkspace, ISelectionService selectionService)
        {
            if(selectionService.SelectedItem != null)
                return;

            var action = (int) ev.Action;
            switch (action & (int) MotionEventActions.Mask)
            {
                case (int) MotionEventActions.Pointer1Down:
                    if (SharedMasterTool.Instance.IsScaleDetectorInProgress())
                    {
                        var gx = SharedMasterTool.Instance.ScaleDetector.FocusX;
                        var gy = SharedMasterTool.Instance.ScaleDetector.FocusY;
                        SharedMasterTool.Instance.LastGestureX = gx;
                        SharedMasterTool.Instance.LastGestureY = gy;
                    }
                    break;
            }
        }

        public void Reset()
        {
            ScaleFactor = 1.0f;
        }

        public Action Command()
        {
            throw new NotSupportedException();
        }

        public Action UndoCommand()
        {
            throw new NotSupportedException();
        }

        public int DrawOrder => 5;
        public int CommandOrder => 5;
    }
}
