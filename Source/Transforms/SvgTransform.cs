using System;
using System.Drawing.Drawing2D;

namespace Svg.Transforms
{
    public abstract class SvgTransform : ICloneable, IEquatable<SvgTransform>
    {
        public abstract Matrix Matrix { get; }
        public abstract string WriteToString();

        public abstract object Clone();

        #region Equals implementation

        public override bool Equals(object obj)
        {
            return obj is SvgTransform other && Equals(other);
        }

        public bool Equals(SvgTransform other)
        {
            return other != null && Matrix.Equals(other.Matrix);
        }

        public override int GetHashCode()
        {
            return Matrix.GetHashCode();
        }

        public static bool operator ==(SvgTransform lhs, SvgTransform rhs)
        {
            if (ReferenceEquals(lhs, rhs))
                return true;
            if (lhs is null || rhs is null)
                return false;
            return lhs.Equals(rhs);
        }

        public static bool operator !=(SvgTransform lhs, SvgTransform rhs)
        {
            return !(lhs == rhs);
        }

        #endregion

        public override string ToString()
        {
            return WriteToString();
        }
    }
}
