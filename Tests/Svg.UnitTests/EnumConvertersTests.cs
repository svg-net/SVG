using NUnit.Framework;
using System;

namespace Svg.UnitTests
{
    /// <summary>
    /// Tests of EnumConverters.
    /// </summary>
    [TestFixture]
    public class EnumConvertersTests
    {
        [Test]
        [TestCase(typeof(SvgFillRuleConverter), "nonzero", "evenodd", "inherit")]
        [TestCase(typeof(SvgColourInterpolationConverter), "auto", "sRGB", "linearRGB", "inherit")]
        [TestCase(typeof(SvgClipRuleConverter), "nonzero", "evenodd", "inherit")]
        [TestCase(typeof(SvgTextAnchorConverter), "start", "middle", "end", "inherit")]
        [TestCase(typeof(SvgStrokeLineCapConverter), "butt", "round", "square", "inherit")]
        [TestCase(typeof(SvgStrokeLineJoinConverter), "miter", "round", "bevel", "inherit")]
        [TestCase(typeof(SvgMarkerUnitsConverter), "strokeWidth", "userSpaceOnUse")]
        [TestCase(typeof(SvgFontStyleConverter), "normal", "italic", "oblique", "inherit")]
        [TestCase(typeof(SvgOverflowConverter), "visible", "hidden", "scroll", "auto", "inherit")]
        [TestCase(typeof(SvgTextLengthAdjustConverter), "spacing", "spacingAndGlyphs")]
        [TestCase(typeof(SvgTextPathMethodConverter), "align", "stretch")]
        [TestCase(typeof(SvgTextPathSpacingConverter), "auto", "exact")]
        [TestCase(typeof(SvgShapeRenderingConverter), "auto", "optimizeSpeed", "crispEdges", "geometricPrecision", "inherit")]
        [TestCase(typeof(SvgTextRenderingConverter), "auto", "optimizeSpeed", "optimizeLegibility", "geometricPrecision", "inherit")]
        [TestCase(typeof(SvgImageRenderingConverter), "auto", "optimizeSpeed", "optimizeQuality", "inherit")]
        [TestCase(typeof(SvgFontVariantConverter), "normal", "small-caps", "inherit")]
        [TestCase(typeof(SvgCoordinateUnitsConverter), "userSpaceOnUse", "objectBoundingBox")]
        [TestCase(typeof(SvgGradientSpreadMethodConverter), "pad", "reflect", "repeat")]
        [TestCase(typeof(SvgTextDecorationConverter), "none", "underline", "overline", "line-through", "blink", "inherit")]
        [TestCase(typeof(SvgFontStretchConverter), "normal", "wider", "narrower", "ultra-condensed", "extra-condensed", "condensed", "semi-condensed", "semi-expanded", "expanded", "extra-expanded", "ultra-expanded", "inherit")]
        [TestCase(typeof(SvgFontWeightConverter), "normal", "bold", "bolder", "lighter", "100", "200", "300", "400", "500", "600", "700", "800", "900", "inherit")]
        [TestCase(typeof(SvgTextTransformationConverter), "none", "capitalize", "uppercase", "lowercase", "inherit")]
        [TestCase(typeof(SvgBlendModeConverter), "normal", "multiply", "screen", "overlay", "darken", "lighten", "color-dodge", "color-burn", "hard-light", "soft-light", "difference", "exclusion", "hue", "saturation", "color", "luminosity")]
        [TestCase(typeof(SvgColourMatrixTypeConverter), "matrix", "saturate", "hueRotate", "luminanceToAlpha")]
        [TestCase(typeof(SvgComponentTransferTypeConverter), "identity", "table", "discrete", "linear", "gamma")]
        [TestCase(typeof(SvgCompositeOperatorConverter), "over", "in", "out", "atop", "xor", "arithmetic")]
        [TestCase(typeof(SvgEdgeModeConverter), "duplicate", "wrap", "none")]
        [TestCase(typeof(SvgChannelSelectorConverter), "R", "G", "B", "A")]
        [TestCase(typeof(SvgMorphologyOperatorConverter), "erode", "dilate")]
        [TestCase(typeof(SvgStitchTypeConverter), "stitch", "noStitch")]
        [TestCase(typeof(SvgTurbulenceTypeConverter), "fractalNoise", "turbulence")]
        public void TestConvert(Type enumConverter, params string[] expectedList)
        {
            var converter = Activator.CreateInstance(enumConverter);
            var convertFrom = enumConverter.GetMethod("ConvertFrom", new Type[] { typeof(object) });
            var convertTo = enumConverter.GetMethod("ConvertTo", new Type[] { typeof(object), typeof(Type) });

            foreach (var expected in expectedList)
            {
                var converted = convertFrom.Invoke(converter, new object[] { expected });
                var result = convertTo.Invoke(converter, new object[] { converted, typeof(string) });

                Assert.AreEqual(expected, result);
            }
        }
    }
}
