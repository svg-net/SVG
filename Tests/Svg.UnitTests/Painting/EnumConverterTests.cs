using Svg.FilterEffects;

using NUnit.Framework;

namespace Svg.UnitTests.Painting
{
    [TestFixture]
    public class EnumConverterTests : SvgTestHelper
    {
        private static readonly EnumBaseConverter<SvgBlendMode> DashedConverter = new EnumBaseConverter<SvgBlendMode>(SvgBlendMode.Normal, EnumBaseConverter<SvgBlendMode>.CaseHandling.DashedLowerCase);
        private static readonly EnumBaseConverter<SvgBlendMode> LowerCaseConverter = new EnumBaseConverter<SvgBlendMode>(SvgBlendMode.Normal, EnumBaseConverter<SvgBlendMode>.CaseHandling.LowerCase);
        private static readonly EnumBaseConverter<SvgBlendMode> CamelCaseConverter = new EnumBaseConverter<SvgBlendMode>(SvgBlendMode.Normal, EnumBaseConverter<SvgBlendMode>.CaseHandling.CamelCase);

        [Test]
        public void DashedToStringWorksForSingleWord()
        {
            var value = SvgBlendMode.Color;
            var str = DashedConverter.ConvertToString(value);
            Assert.AreEqual("color", str);
        }

        [Test]
        public void DashedToStringWorksForMultipleWords()
        {
            var value = SvgBlendMode.SoftLight;
            var str = DashedConverter.ConvertToString(value);
            Assert.AreEqual("soft-light", str);
        }

        [Test]
        public void LowerToStringWorksForSingleWord()
        {
            var value = SvgBlendMode.Color;
            var str = LowerCaseConverter.ConvertToString(value);
            Assert.AreEqual("color", str);
        }

        [Test]
        public void LowerToStringWorksForMultipleWords()
        {
            var value = SvgBlendMode.SoftLight;
            var str = LowerCaseConverter.ConvertToString(value);
            Assert.AreEqual("softlight", str);
        }

        [Test]
        public void CamelToStringWorksForSingleWord()
        {
            var value = SvgBlendMode.Color;
            var str = CamelCaseConverter.ConvertToString(value);
            Assert.AreEqual("color", str);
        }

        [Test]
        public void CamelToStringWorksForMultipleWords()
        {
            var value = SvgBlendMode.SoftLight;
            var str = CamelCaseConverter.ConvertToString(value);
            Assert.AreEqual("softLight", str);
        }
    }
}
