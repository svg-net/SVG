using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Svg.Droid.Editor.Tools;

namespace Svg.Droid.Editor
{
    public class SelectableAndroidBitmap : AndroidBitmap, IPosition
    {
        public bool IsSelected { get; set; } = false;

        public SelectableAndroidBitmap(Image inputImage, int x = 0, int y = 0) : base(inputImage)
        {
            X = x;
            Y = y;
            Width = inputImage.Width;
            Height = inputImage.Height;
        }

        public SelectableAndroidBitmap(Android.Graphics.Bitmap bitmap) : base(bitmap)
        {
        }

        public Rectangle Rect => new Rectangle(X, Y, Width, Height);
        
        public int X { get; set; }
        public int Y { get; set; }
    }

    public interface IPosition
    {
        Rectangle Rect { get; }

        int X { get; set; }
        int Y { get; set; }

        int Width { get; }
        int Height { get; }
    }
}