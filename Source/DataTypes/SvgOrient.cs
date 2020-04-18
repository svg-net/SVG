using System.ComponentModel;
using System.Globalization;
using Svg.DataTypes;

namespace Svg
{
    /// <summary>
    /// Represents an orientation in a Scalable Vector Graphics document.
    /// </summary>
    [TypeConverter(typeof(SvgOrientConverter))]
    public class SvgOrient
    {
        private bool _isAuto;
        private float _angle;

        public SvgOrient()
            : this(0f)
        {
        }

        public SvgOrient(bool isAuto)
        {
            IsAuto = isAuto;
        }

        public SvgOrient(bool isAuto, bool isAutoStartReverse)
        {
            IsAuto = isAuto;
            IsAutoStartReverse = isAutoStartReverse;
        }

        public SvgOrient(float angle)
        {
            Angle = angle;
        }

        /// <summary>
        /// Gets the value of the unit.
        /// </summary>
        public float Angle
        {
            get { return _angle; }
            set
            {
                _angle = value;
                _isAuto = false;
            }
        }

        /// <summary>
        /// Gets the value of the unit.
        /// </summary>
        public bool IsAuto
        {
            get { return _isAuto; }
            set
            {
                _isAuto = value;
                _angle = 0f;
            }
        }

        /// <summary>
        /// If IsAuto is true, indicates if the orientation of a 'marker-start' must be rotated of 180° from the original orientation
        /// </summary>
        /// This allows a single arrowhead marker to be defined that can be used for both the start and end of a path, point in the right directions.
        public bool IsAutoStartReverse { get; set; }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is SvgOrient))
                return false;

            var orient = (SvgOrient)obj;
            return (orient.IsAuto == IsAuto && orient.Angle == Angle);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            if (IsAuto)
                return IsAutoStartReverse ? "auto-start-reverse" : "auto";
            else
                return Angle.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Single"/> to <see cref="Svg.SvgOrient"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SvgOrient(float value)
        {
            return new SvgOrient(value);
        }
    }
}
