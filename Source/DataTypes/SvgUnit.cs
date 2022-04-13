﻿using System;
using System.ComponentModel;

namespace Svg
{
    /// <summary>
    /// Represents a unit in an Scalable Vector Graphics document.
    /// </summary>
    [TypeConverter(typeof(SvgUnitConverter))]
    public partial struct SvgUnit
    {
        private SvgUnitType _type;
        private float _value;
        private bool _isEmpty;
        private float? _deviceValue;

        /// <summary>
        /// Gets and empty <see cref="SvgUnit"/>.
        /// </summary>
        public static readonly SvgUnit Empty = new SvgUnit(SvgUnitType.User, 0) { _isEmpty = true };

        /// <summary>
        /// Gets an <see cref="SvgUnit"/> with a value of none.
        /// </summary>
        public static readonly SvgUnit None = new SvgUnit(SvgUnitType.None, 0f);

        /// <summary>
        /// Gets a value to determine whether the unit is empty.
        /// </summary>
        public bool IsEmpty
        {
            get { return this._isEmpty; }
        }

        /// <summary>
        /// Gets whether this unit is none.
        /// </summary>
        public bool IsNone
        {
            get { return _type == SvgUnitType.None; }
        }

        /// <summary>
        /// Gets the value of the unit.
        /// </summary>
        public float Value
        {
            get { return this._value; }
        }

        /// <summary>
        /// Gets the <see cref="SvgUnitType"/> of unit.
        /// </summary>
        public SvgUnitType Type
        {
            get { return this._type; }
        }

        /// <summary>
        /// Converts the current unit to a percentage, if applicable.
        /// </summary>
        /// <returns>An <see cref="SvgUnit"/> of type <see cref="SvgUnitType.Percentage"/>.</returns>
        public SvgUnit ToPercentage()
        {
            switch (this.Type)
            {
                case SvgUnitType.Percentage:
                    return this;
                case SvgUnitType.User:
                    return new SvgUnit(SvgUnitType.Percentage, this.Value * 100);
                default:
                    throw new NotImplementedException();
            }
        }

        #region Equals and GetHashCode implementation
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj.GetType() == typeof(SvgUnit))) return false;

            var unit = (SvgUnit)obj;
            return (unit.Value == this.Value && unit.Type == this.Type);
        }

        public bool Equals(SvgUnit other)
        {
            return this._type == other._type && (this._value == other._value);
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            unchecked
            {
                hashCode += 1000000007 * _type.GetHashCode();
                hashCode += 1000000009 * _value.GetHashCode();
                hashCode += 1000000021 * _isEmpty.GetHashCode();
                hashCode += 1000000033 * _deviceValue.GetHashCode();
            }
            return hashCode;
        }

        public static bool operator ==(SvgUnit lhs, SvgUnit rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(SvgUnit lhs, SvgUnit rhs)
        {
            return !(lhs == rhs);
        }
        #endregion

        public override string ToString()
        {
            string type = string.Empty;

            switch (this.Type)
            {
                case SvgUnitType.None:
                    return "none";
                case SvgUnitType.Pixel:
                    type = "px";
                    break;
                case SvgUnitType.Point:
                    type = "pt";
                    break;
                case SvgUnitType.Inch:
                    type = "in";
                    break;
                case SvgUnitType.Centimeter:
                    type = "cm";
                    break;
                case SvgUnitType.Millimeter:
                    type = "mm";
                    break;
                case SvgUnitType.Percentage:
                    type = "%";
                    break;
                case SvgUnitType.Em:
                    type = "em";
                    break;
            }

            return string.Concat(this.Value.ToSvgString(), type);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Single"/> to <see cref="Svg.SvgUnit"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SvgUnit(float value)
        {
            return new SvgUnit(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgUnit"/> struct.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        public SvgUnit(SvgUnitType type, float value)
        {
            this._isEmpty = false;
            this._type = type;
            this._value = value;
            this._deviceValue = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgUnit"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public SvgUnit(float value)
        {
            this._isEmpty = false;
            this._value = value;
            this._type = SvgUnitType.User;
            this._deviceValue = null;
        }
    }

    public enum UnitRenderingType
    {
        Other,
        Horizontal,
        HorizontalOffset,
        Vertical,
        VerticalOffset
    }

    /// <summary>
    /// Defines the various types of unit an <see cref="SvgUnit"/> can be.
    /// </summary>
    public enum SvgUnitType
    {
        /// <summary>
        /// Indicates that the unit holds no value.
        /// </summary>
        None,
        /// <summary>
        /// Indicates that the unit is in pixels.
        /// </summary>
        Pixel,
        /// <summary>
        /// Indicates that the unit is equal to the pt size of the current font.
        /// </summary>
        Em,
        /// <summary>
        /// Indicates that the unit is equal to the x-height of the current font.
        /// </summary>
        Ex,
        /// <summary>
        /// Indicates that the unit is a percentage.
        /// </summary>
        Percentage,
        /// <summary>
        /// Indicates that the unit has no unit identifier and is a value in the current user coordinate system.
        /// </summary>
        User,
        /// <summary>
        /// Indicates the the unit is in inches.
        /// </summary>
        Inch,
        /// <summary>
        /// Indicates that the unit is in centimeters.
        /// </summary>
        Centimeter,
        /// <summary>
        /// Indicates that the unit is in millimeters.
        /// </summary>
        Millimeter,
        /// <summary>
        /// Indicates that the unit is in picas.
        /// </summary>
        Pica,
        /// <summary>
        /// Indicates that the unit is in points, the smallest unit of measure, being a subdivision of the larger <see cref="Pica"/>. There are 12 points in the <see cref="Pica"/>.
        /// </summary>
        Point
    }
}
