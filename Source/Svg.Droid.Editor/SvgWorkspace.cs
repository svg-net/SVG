using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;
using Svg.Droid.Editor.Services;
using Svg.Droid.Editor.Tools;
using Color = Android.Graphics.Color;

namespace Svg.Droid.Editor
{
    public class SvgWorkspace : ImageView
    {
        private const int Size = 2000;

        public SvgWorkspaceModel ViewModel { get; } = new SvgWorkspaceModel();
        public AndroidBitmap MainBitmap { get; } = new AndroidBitmap(Size, Size);

        public SvgWorkspace(Context context, IAttributeSet attr) : base(context, attr)
        {
            if(ZoomTool.IsActive)
                SharedMasterTool.Instance.ScaleDetector = new ScaleGestureDetector(context, new ZoomTool.ScaleListener(this, ViewModel.SelectionService));
        }

        public void AddSvg(SvgDocument svgDocument)
        {
            // TODO PUT ME IN THE VIEWMODEL

            ViewModel.AddSvg(svgDocument);
            var bitmap = (AndroidBitmap) svgDocument.Draw();
            var x = (Width / 2) - (bitmap.Width / 2) - (int) SharedMasterTool.Instance.CanvasTranslatedPosX;
            var y = (Height / 2) - (bitmap.Height / 2) - (int) SharedMasterTool.Instance.CanvasTranslatedPosY;

            if (SnappingTool.IsActive)
            {
                x = (int) (Math.Round((x) / SnappingTool.StepSize) * SnappingTool.StepSize);
                y = (int) (Math.Round((y) / SnappingTool.StepSize) * SnappingTool.StepSize);
            }

            var selBitmap = new SelectableAndroidBitmap(bitmap, x, y);

            ViewModel.Elements.Add(selBitmap);
            //ViewModel.Select(selBitmap);

            ViewModel.Select(ViewModel.Elements.LastOrDefault());
            Invalidate();
        }

        public override bool OnTouchEvent(MotionEvent ev)
        {
            // Let the ScaleGestureDetector inspect all events.
            if(SharedMasterTool.Instance.ScaleDetector != null)
                SharedMasterTool.Instance.ScaleDetector.OnTouchEvent(ev); 

            foreach (var tool in ViewModel.Tools)
                tool.OnTouch(ev, this, ViewModel.SelectionService);

            return true;
        }

        public Paint Paint { get; } = new Paint() { Color = Color.Rgb(255, 0, 0), StrokeWidth = 5 };

        protected override void OnDraw(Canvas canvas)
        {
            canvas.DrawColor(Color.White);

            foreach (var tool in ViewModel.Tools)
                tool.OnDraw(canvas, ViewModel.SelectionService.SelectedItem);

            foreach (var bitmap in ViewModel.Elements)
                canvas.DrawBitmap(bitmap.Image, bitmap.X, bitmap.Y, null);

            base.OnDraw(canvas);
        }

        private void ResetTools()
        {
            foreach (var tool in ViewModel.Tools)
                tool.Reset();
        }

        public void ToggleGridVisibility()
        {
            GridTool.IsVisible = !GridTool.IsVisible;
            Invalidate();
        }

        public void RemoveSelectedItem()
        {
            ViewModel.Elements.Remove(ViewModel.SelectionService.SelectedItem);
            ViewModel.SelectionService.SelectedItem = null;
            ViewModel.SelectedItemChanged?.Invoke(false);
            Invalidate();
        }
    }
}