using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Svg.Droid.Editor.Interfaces
{
    public interface ISelectionService
    {
        SelectableAndroidBitmap SelectedItem { get; set; }

        SelectableAndroidBitmap SelectNewItem(IEnumerable<SelectableAndroidBitmap> allBitmaps, int pointerX, int pointerY);
        bool IsInRangeOfSelected(SelectableAndroidBitmap selected, int pointerX, int pointerY);
    }
}