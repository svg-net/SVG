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
        Normal = 1,

        /// <summary>Same as <see cref="W700"/>.</summary>
        Bold = 2,

        /// <summary>One font weight darker than the parent element.(do not use font-face.)</summary>
        Bolder = 4,

        /// <summary>One font weight lighter than the parent element.(do not use font-face.)</summary>
        Lighter = 8,

        /// <summary></summary>
        W100 = 1 << 8,

        /// <summary></summary>
        W200 = 2 << 8,

        /// <summary></summary>
        W300 = 4 << 8,

        /// <summary>Same as <see cref="Normal"/>.</summary>
        W400 = 8 << 8,

        /// <summary></summary>
        W500 = 16 << 8,

        /// <summary></summary>
        W600 = 32 << 8,

        /// <summary>Same as <see cref="Bold"/>.</summary>
        W700 = 64 << 8,

        /// <summary></summary>
        W800 = 128 << 8,

        /// <summary></summary>
        W900 = 256 << 8,

        /// <summary>All font weights.</summary>
        All = Normal | Bold | W100 | W200 | W300 | W400 | W500 | W600 | W700 | W800 | W900,
    }
}
