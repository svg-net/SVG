using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Svg.DataTypes;

namespace Svg
{
    /// <summary>
    /// Description of SvgAspectRatio.
    /// </summary>
    [TypeConverter(typeof(SvgPreserveAspectRatioConverter))]
    public class SvgAspectRatio : ICloneable
    {
        public SvgAspectRatio() : this(SvgPreserveAspectRatio.none)
        {
        }

        public SvgAspectRatio(SvgPreserveAspectRatio align)
            : this(align, false)
        {
        }

        public SvgAspectRatio(SvgPreserveAspectRatio align, bool slice)
            : this(align, slice, false)
        {
        }

        public SvgAspectRatio(SvgPreserveAspectRatio align, bool slice, bool defer)
        {
            this.Align = align;
            this.Slice = slice;
            this.Defer = defer;
        }

        public SvgPreserveAspectRatio Align
        {
            get;
            set;
        }

        public bool Slice
        {
            get;
            set;
        }

        public bool Defer
        {
            get;
            set;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

#if NET6_0_OR_GREATER
        [DynamicDependency(DynamicallyAccessedMemberTypes.PublicMethods, typeof(SvgPreserveAspectRatioConverter))]
        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "DynamicDependency keeps converter safe")]
        [UnconditionalSuppressMessage("AOT", "IL3050")]
#endif
        public override string ToString()
        {
            return TypeDescriptor.GetConverter(typeof(SvgPreserveAspectRatio)).ConvertToString(this.Align) + (Slice ? " slice" : "");
        }
    }

    [TypeConverter(typeof(SvgPreserveAspectRatioConverter))]
    public enum SvgPreserveAspectRatio
    {
        xMidYMid, //default
        none,
        xMinYMin,
        xMidYMin,
        xMaxYMin,
        xMinYMid,
        xMaxYMid,
        xMinYMax,
        xMidYMax,
        xMaxYMax
    }
}
