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
    public interface IToolService
    {
        IEnumerable<ITool> Tools { get; }
    }
}