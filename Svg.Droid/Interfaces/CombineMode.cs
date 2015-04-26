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

namespace Svg
{
    // Summary:
    //     Specifies how different clipping regions can be combined.
    public enum CombineMode
    {
        // Summary:
        //     One clipping region is replaced by another.
        Replace = 0,
        //
        // Summary:
        //     Two clipping regions are combined by taking their intersection.
        Intersect = 1,
        //
        // Summary:
        //     Two clipping regions are combined by taking the union of both.
        Union = 2,
        //
        // Summary:
        //     Two clipping regions are combined by taking only the areas enclosed by one
        //     or the other region, but not both.
        Xor = 3,
        //
        // Summary:
        //     Specifies that the existing region is replaced by the result of the new region
        //     being removed from the existing region. Said differently, the new region
        //     is excluded from the existing region.
        Exclude = 4,
        //
        // Summary:
        //     Specifies that the existing region is replaced by the result of the existing
        //     region being removed from the new region. Said differently, the existing
        //     region is excluded from the new region.
        Complement = 5,
    }
}