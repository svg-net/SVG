﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using Svg.DataTypes;
using Svg.FilterEffects;

namespace Svg
{
    public abstract class EnumBaseConverter<T> : TypeConverter
        where T : struct
    {
        public enum CaseHandling
        {
            CamelCase,
            PascalCase,
            LowerCase,
            KebabCase,
        }

        /// <summary>Defines if the enum literal shall be converted to camelCase, PascalCase or kebab-case.</summary>
        public CaseHandling CaseHandlingMode { get; }

        /// <summary>Creates a new instance.</summary>
        /// <param name="caseHandling">Defines if the value shall be converted to camelCase, PascalCase, lowercase or kebab-case.</param>
        public EnumBaseConverter(CaseHandling caseHandling = CaseHandling.CamelCase)
        {
            CaseHandlingMode = caseHandling;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>Attempts to convert the provided value to <typeparamref name="T"/>.</summary>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string stringValue)
            {
                if (CaseHandlingMode == CaseHandling.KebabCase)
                    stringValue = stringValue.Replace("-", string.Empty);

                if (Enum.TryParse<T>(stringValue, true, out T result))
                    return result;
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>Attempts to convert the value to the destination type.</summary>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is T)
            {
                var stringValue = ((T)value).ToString();
                if (CaseHandlingMode == CaseHandling.CamelCase)
                    return string.Format("{0}{1}", stringValue[0].ToString().ToLower(), stringValue.Substring(1));

                if (CaseHandlingMode == CaseHandling.PascalCase)
                    return stringValue;

                if (CaseHandlingMode == CaseHandling.KebabCase)
                    stringValue = Regex.Replace(stringValue, @"(\w)([A-Z])", "$1-$2", RegexOptions.CultureInvariant);

                return stringValue.ToLower();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public sealed class SvgFillRuleConverter : EnumBaseConverter<SvgFillRule>
    {
        public SvgFillRuleConverter() : base(CaseHandling.LowerCase) { }
    }

    public sealed class SvgColourInterpolationConverter : EnumBaseConverter<SvgColourInterpolation> { }

    public sealed class SvgClipRuleConverter : EnumBaseConverter<SvgClipRule>
    {
        public SvgClipRuleConverter() : base(CaseHandling.LowerCase) { }
    }

    public sealed class SvgTextAnchorConverter : EnumBaseConverter<SvgTextAnchor> { }

    public sealed class SvgStrokeLineCapConverter : EnumBaseConverter<SvgStrokeLineCap> { }

    public sealed class SvgStrokeLineJoinConverter : EnumBaseConverter<SvgStrokeLineJoin> { }

    public sealed class SvgMarkerUnitsConverter : EnumBaseConverter<SvgMarkerUnits> { }

    public sealed class SvgFontStyleConverter : EnumBaseConverter<SvgFontStyle> { }

    public sealed class SvgOverflowConverter : EnumBaseConverter<SvgOverflow> { }

    public sealed class SvgTextLengthAdjustConverter : EnumBaseConverter<SvgTextLengthAdjust> { }

    public sealed class SvgTextPathMethodConverter : EnumBaseConverter<SvgTextPathMethod> { }

    public sealed class SvgTextPathSpacingConverter : EnumBaseConverter<SvgTextPathSpacing> { }

    public sealed class SvgShapeRenderingConverter : EnumBaseConverter<SvgShapeRendering> { }

    public sealed class SvgTextRenderingConverter : EnumBaseConverter<SvgTextRendering> { }

    public sealed class SvgImageRenderingConverter : EnumBaseConverter<SvgImageRendering> { }

    public sealed class SvgFontVariantConverter : EnumBaseConverter<SvgFontVariant>
    {
        public SvgFontVariantConverter() : base(CaseHandling.KebabCase) { }
    }

    public sealed class SvgCoordinateUnitsConverter : EnumBaseConverter<SvgCoordinateUnits> { }

    public sealed class SvgGradientSpreadMethodConverter : EnumBaseConverter<SvgGradientSpreadMethod> { }

    public sealed class SvgTextDecorationConverter : EnumBaseConverter<SvgTextDecoration>
    {
        public SvgTextDecorationConverter() : base(CaseHandling.KebabCase) { }
    }

    public sealed class SvgFontStretchConverter : EnumBaseConverter<SvgFontStretch>
    {
        public SvgFontStretchConverter() : base(CaseHandling.KebabCase) { }
    }

    public sealed class SvgFontWeightConverter : EnumBaseConverter<SvgFontWeight>
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                switch ((string)value)
                {
                    case "100": return SvgFontWeight.W100;
                    case "200": return SvgFontWeight.W200;
                    case "300": return SvgFontWeight.W300;
                    case "400": return SvgFontWeight.W400;
                    case "500": return SvgFontWeight.W500;
                    case "600": return SvgFontWeight.W600;
                    case "700": return SvgFontWeight.W700;
                    case "800": return SvgFontWeight.W800;
                    case "900": return SvgFontWeight.W900;
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is SvgFontWeight)
            {
                switch ((SvgFontWeight)value)
                {
                    case SvgFontWeight.W100: return "100";
                    case SvgFontWeight.W200: return "200";
                    case SvgFontWeight.W300: return "300";
                    case SvgFontWeight.W400: return "400";
                    case SvgFontWeight.W500: return "500";
                    case SvgFontWeight.W600: return "600";
                    case SvgFontWeight.W700: return "700";
                    case SvgFontWeight.W800: return "800";
                    case SvgFontWeight.W900: return "900";
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public sealed class SvgTextTransformationConverter : EnumBaseConverter<SvgTextTransformation> { }

    public sealed class SvgBlendModeConverter : EnumBaseConverter<SvgBlendMode>
    {
        public SvgBlendModeConverter() : base(CaseHandling.KebabCase) { }
    }

    public sealed class SvgColourMatrixTypeConverter : EnumBaseConverter<SvgColourMatrixType> { }

    public sealed class SvgComponentTransferTypeConverter : EnumBaseConverter<SvgComponentTransferType> { }

    public sealed class SvgCompositeOperatorConverter : EnumBaseConverter<SvgCompositeOperator> { }

    public sealed class SvgEdgeModeConverter : EnumBaseConverter<SvgEdgeMode> { }

    public sealed class SvgChannelSelectorConverter : EnumBaseConverter<SvgChannelSelector>
    {
        public SvgChannelSelectorConverter() : base(CaseHandling.PascalCase) { }
    }

    public sealed class SvgMorphologyOperatorConverter : EnumBaseConverter<SvgMorphologyOperator> { }

    public sealed class SvgStitchTypeConverter : EnumBaseConverter<SvgStitchType> { }

    public sealed class SvgTurbulenceTypeConverter : EnumBaseConverter<SvgTurbulenceType> { }

    public static class Enums
    {
        [CLSCompliant(false)]
        public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct, IConvertible
        {
            try
            {
                result = (TEnum)Enum.Parse(typeof(TEnum), value, true);
                return true;
            }
            catch
            {
                result = default(TEnum);
                return false;
            }
        }
    }
}
