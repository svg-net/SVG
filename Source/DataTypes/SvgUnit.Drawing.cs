#if !NO_SDC
using System;
using System.Linq;
using System.Drawing;

namespace Svg
{
    public partial struct SvgUnit
    {
        /// <summary>
        /// Converts the current unit to one that can be used at render time.
        /// </summary>
        /// <returns>The representation of the current unit in a device value (usually pixels).</returns>
        public float ToDeviceValue(ISvgRenderer renderer, UnitRenderingType renderType, SvgElement owner)
        {
            // If it's already been calculated
            if (this._deviceValue.HasValue)
            {
                return this._deviceValue.Value;
            }

            if (this._value == 0.0f)
            {
                this._deviceValue = 0.0f;
                return this._deviceValue.Value;
            }

            // http://www.w3.org/TR/CSS21/syndata.html#values
            // http://www.w3.org/TR/SVG11/coords.html#Units

            const float cmInInch = 2.54f;
            var ppi = owner?.OwnerDocument?.Ppi ?? SvgDocument.PointsPerInch;

            var type = this.Type;
            var value = this.Value;

            float points;

            switch (type)
            {
                case SvgUnitType.Em:
                    using (var fontManager = owner?.OwnerDocument?.FontManager == null ? new SvgFontManager() : null)
                    using (var currFont = GetFont(renderer, owner, fontManager))
                    {
                        if (currFont == null)
                        {
                            points = (float)(value * 9);
                            _deviceValue = (points / 72.0f) * ppi;
                        }
                        else
                        {
                            _deviceValue = value * (currFont.SizeInPoints / 72.0f) * ppi;
                        }
                    }

                    break;
                case SvgUnitType.Ex:
                    using (var fontManager = owner?.OwnerDocument?.FontManager == null ? new SvgFontManager() : null)
                    using (var currFont = GetFont(renderer, owner, fontManager))
                    {
                        if (currFont == null)
                        {
                            points = (float)(value * 9);
                            _deviceValue = (points * 0.5f / 72.0f) * ppi;
                        }
                        else
                        {
                            _deviceValue = value * 0.5f * (currFont.SizeInPoints / 72.0f) * ppi;
                        }
                    }

                    break;
                case SvgUnitType.Centimeter:
                    _deviceValue = (float)((value / cmInInch) * ppi);
                    break;
                case SvgUnitType.Inch:
                    _deviceValue = value * ppi;
                    break;
                case SvgUnitType.Millimeter:
                    _deviceValue = (float)((value / 10) / cmInInch) * ppi;
                    break;
                case SvgUnitType.Pica:
                    _deviceValue = ((value * 12) / 72) * ppi;
                    break;
                case SvgUnitType.Point:
                    _deviceValue = (value / 72) * ppi;
                    break;
                case SvgUnitType.Pixel:
                    _deviceValue = value;
                    break;
                case SvgUnitType.User:
                    _deviceValue = value;
                    break;
                case SvgUnitType.Percentage:
                    // Can't calculate if there is no style owner
                    var boundable = (renderer == null
                        ? (owner == null ? null : owner.OwnerDocument)
                        : renderer.GetBoundable());
                    if (boundable == null)
                    {
                        _deviceValue = value;
                        break;
                    }

                    SizeF size = boundable.Bounds.Size;

                    switch (renderType)
                    {
                        case UnitRenderingType.Horizontal:
                            _deviceValue = size.Width * value / 100;
                            break;
                        case UnitRenderingType.HorizontalOffset:
                            _deviceValue = size.Width * value / 100 + boundable.Location.X;
                            break;
                        case UnitRenderingType.Vertical:
                            _deviceValue = size.Height * value / 100;
                            break;
                        case UnitRenderingType.VerticalOffset:
                            _deviceValue = size.Height * value / 100 + boundable.Location.Y;
                            break;
                        case UnitRenderingType.Other:
                            // Calculate a percentage value of the normalized viewBox diagonal length.
                            if (owner.OwnerDocument != null && owner.OwnerDocument.ViewBox.Width != 0 &&
                                owner.OwnerDocument.ViewBox.Height != 0)
                            {
                                _deviceValue =
                                    (float)(Math.Sqrt(Math.Pow(owner.OwnerDocument.ViewBox.Width, 2) +
                                                      Math.Pow(owner.OwnerDocument.ViewBox.Height, 2)) / Math.Sqrt(2) *
                                        value / 100.0);
                            }
                            else
                                _deviceValue = (float)(Math.Sqrt(Math.Pow(size.Width, 2) + Math.Pow(size.Height, 2)) /
                                    Math.Sqrt(2) * value / 100.0);

                            break;
                    }

                    break;
                default:
                    _deviceValue = value;
                    break;
            }

            return this._deviceValue.HasValue ? this._deviceValue.Value : 0f;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Svg.SvgUnit"/> to <see cref="System.Single"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator float(SvgUnit value)
        {
            return value.ToDeviceValue(null, UnitRenderingType.Other, null);
        }

        private IFontDefn GetFont(ISvgRenderer renderer, SvgElement owner, SvgFontManager fontManager)
        {
            var visual = owner?.Parents.OfType<SvgVisualElement>().FirstOrDefault();
            return visual?.GetFont(renderer, fontManager);
        }

        public static System.Drawing.PointF GetDevicePoint(SvgUnit x, SvgUnit y, ISvgRenderer renderer, SvgElement owner)
        {
            return new System.Drawing.PointF(x.ToDeviceValue(renderer, UnitRenderingType.Horizontal, owner),
                y.ToDeviceValue(renderer, UnitRenderingType.Vertical, owner));
        }
        public static System.Drawing.PointF GetDevicePointOffset(SvgUnit x, SvgUnit y, ISvgRenderer renderer, SvgElement owner)
        {
            return new System.Drawing.PointF(x.ToDeviceValue(renderer, UnitRenderingType.HorizontalOffset, owner),
                y.ToDeviceValue(renderer, UnitRenderingType.VerticalOffset, owner));
        }

        public static System.Drawing.SizeF GetDeviceSize(SvgUnit width, SvgUnit height, ISvgRenderer renderer, SvgElement owner)
        {
            return new System.Drawing.SizeF(width.ToDeviceValue(renderer, UnitRenderingType.HorizontalOffset, owner),
                height.ToDeviceValue(renderer, UnitRenderingType.VerticalOffset, owner));
        }
    }
}
#endif
