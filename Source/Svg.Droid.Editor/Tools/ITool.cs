using System;
using Android.Graphics;
using Android.Views;

namespace Svg.Droid.Editor.Tools
{
    public interface ITool
    {
        void OnDraw(Canvas canvas, IPosition anyItemSelected);
        void OnTouch(MotionEvent ev, SvgWorkspace svgWorkspace);
        void Reset();

        Action Command();
        Action UndoCommand();
    }
}