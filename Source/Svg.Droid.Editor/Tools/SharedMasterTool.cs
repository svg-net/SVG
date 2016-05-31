using System;
using System.Collections.Generic;
using Android.Graphics;
using Android.Views;
using Svg.Droid.Editor.Interfaces;
using Svg.Droid.Editor.Services;

namespace Svg.Droid.Editor.Tools
{
    public class SharedMasterTool : ITool
    {
        public ScaleGestureDetector ScaleDetector;

        public bool IsScaleDetectorInProgress()
        {
            if(Instance.ScaleDetector == null)
                return false;

            return Instance.ScaleDetector.IsInProgress;
        }

        public float CanvasTranslatedPosX;
        public float CanvasTranslatedPosY;

        public float LastTouchX;
        public float LastTouchY;

        public float LastGestureX;
        public float LastGestureY;

        public const int InvalidPointerId = -1;
        public int ActivePointerId = InvalidPointerId;

        private static SharedMasterTool _instance;

        private SharedMasterTool()
        {
            
        }

        public static SharedMasterTool Instance => _instance ?? (_instance = new SharedMasterTool());

        public void OnDraw(Canvas canvas, IPosition anyItemSelected)
        {
            
        }

        public void OnTouch(MotionEvent ev, SvgWorkspace svgWorkspace, ISelectionService selectionService)
        {
            int action = (int)ev.Action;
            switch (action & (int)MotionEventActions.Mask)
            {
                case (int)MotionEventActions.Down:
                    if (!IsScaleDetectorInProgress())
                    {
                        var x = ev.GetX();
                        var y = ev.GetY();

                        Instance.LastTouchX = x;
                        Instance.LastTouchY = y;
                        Instance.ActivePointerId = ev.GetPointerId(0);
                    }
                    break;

                case (int)MotionEventActions.Up:
                    Instance.ActivePointerId = InvalidPointerId;
                    break;

                case (int)MotionEventActions.Cancel:
                    Instance.ActivePointerId = InvalidPointerId;
                    break;

                case (int)MotionEventActions.PointerUp:

                    int pointerIndex2 = ((int)ev.Action & (int)MotionEventActions.PointerIndexMask)
                            >> (int)MotionEventActions.PointerIndexShift;

                    int pointerId = ev.GetPointerId(pointerIndex2);
                    if (pointerId == Instance.ActivePointerId)
                    {
                        // This was our active pointer going up. Choose a new
                        // active pointer and adjust accordingly.
                        int newPointerIndex = pointerIndex2 == 0 ? 1 : 0;
                        Instance.LastTouchX = ev.GetX(newPointerIndex);
                        Instance.LastTouchY = ev.GetY(newPointerIndex);
                        Instance.ActivePointerId = ev.GetPointerId(newPointerIndex);
                    }
                    else
                    {
                        int tempPointerIndex = ev.FindPointerIndex(Instance.ActivePointerId);
                        Instance.LastTouchX = ev.GetX(tempPointerIndex);
                        Instance.LastTouchY = ev.GetY(tempPointerIndex);
                    }

                    break;
            }
        }

        public void Reset()
        {
            LastTouchX = 0;
            LastTouchY = 0;

            LastGestureX = 0;
            LastGestureY = 0;

            ActivePointerId = InvalidPointerId;
        }

        public Action Command()
        {
            throw new NotSupportedException();
        }

        public Action UndoCommand()
        {
            throw new NotSupportedException();
        }
    }
}