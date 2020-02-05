using System;
using System.ComponentModel;

namespace Svg
{
    /// <summary>The weight of a face relative to others in the same font family.</summary>
    [TypeConverter(typeof(SvgFontWeightConverter))]
    [Flags]
    public enum SvgFontWeight
    {
        /// <summary>The value is inherited from the parent element.</summary>
        Inherit,

        /// <summary>Same as <see cref="W400"/>.</summary>
        Normal = W400,

        /// <summary>Same as <see cref="W700"/>.</summary>
        Bold = W700,

        /// <summary>One font weight darker than the parent element.</summary>
        Bolder = 512,

        /// <summary>One font weight lighter than the parent element.</summary>
        Lighter = 1024,

        /// <summary></summary>
        W100 = 1,

        /// <summary></summary>
        W200 = 2,

        /// <summary></summary>
        W300 = 4,

        /// <summary>Same as <see cref="Normal"/>.</summary>
        W400 = 8,

        /// <summary></summary>
        W500 = 16,

        /// <summary></summary>
        W600 = 32,

        /// <summary>Same as <see cref="Bold"/>.</summary>
        W700 = 64,

        /// <summary></summary>
        W800 = 128,

        /// <summary></summary>
        W900 = 256,

        /// <summary>All font weights.</summary>
        All = W100 | W200 | W300 | W400 | W500 | W600 | W700 | W800 | W900,
    }
}
