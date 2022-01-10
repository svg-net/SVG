using System;
using System.ComponentModel;
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
