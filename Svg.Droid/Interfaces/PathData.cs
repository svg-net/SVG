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

namespace Svg
{
    public interface PathData
    {
        // Summary:
        //     Gets or sets an array of System.Drawing.PointF structures that represents
        //     the points through which the path is constructed.
        //
        // Returns:
        //     An array of System.Drawing.PointF objects that represents the points through
        //     which the path is constructed.
        PointF[] Points { get; set; }
        //
        // Summary:
        //     Gets or sets the types of the corresponding points in the path.
        //
        // Returns:
        //     An array of bytes that specify the types of the corresponding points in the
        //     path.
        byte[] Types { get; set; }
    }
}