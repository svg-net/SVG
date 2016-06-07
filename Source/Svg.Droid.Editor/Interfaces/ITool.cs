using System;
using Android.Graphics;
using Android.Views;

namespace Svg.Droid.Editor.Interfaces
{
    public interface ITool
    {
        void OnDraw(Canvas canvas, IPosition anyItemSelected);
        void OnTouch(MotionEvent ev, SvgWorkspace svgWorkspace, ISelectionService selectionService);
        void Reset();

        Action Command();
        Action UndoCommand();
    }
}