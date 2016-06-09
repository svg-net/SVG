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

namespace Svg.Interfaces
{
    public interface IPosition
    {
        Rectangle Rect { get; }

        int X { get; set; }
        int Y { get; set; }

        int Width { get; }
        int Height { get; }
    }
}